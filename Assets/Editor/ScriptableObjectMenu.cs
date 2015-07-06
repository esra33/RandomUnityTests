using hvs.attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom menu for fast creation of scriptable objects
/// </summary>
public class ScriptableObjectMenu : EditorWindow
{
    private int selectedScriptableTypeIdx = 0;
    private List<string> names;
    private List<Type> typesList;
    private bool isSetup = false;

	/// <summary>
	/// Fast instantiation of scriptable objects
	/// </summary>
	[MenuItem("Assets/ScriptableObjects/InstantiateScriptableObject")]
	public static void FastInstantiation()
	{
		MonoScript script = Selection.activeObject as MonoScript;
		if(script == null)
		{
			Debug.LogError("Selected item is not a ScriptableObject script");
			return;
		}
		Attribute[] customAttributes = script.GetClass().GetCustomAttributes(false) as Attribute[];
		foreach(Attribute attribute in customAttributes)
		{
			if(attribute is ScriptableAttribute)
			{
				ScriptableObjectMenu.CreateItem(script.GetClass());
				return;
			}
		}

		Debug.LogError("Selected item is not a valid ScriptableObject or doesn't have the ScriptableAttribute attribute tag");
	}

	/// <summary>
	/// Static Request on the creation menu to create a custom scriptable object
	/// </summary>
	[MenuItem("Assets/ScriptableObjects/ScriptableObject")]
    public static void RequestInstantiationWindow()
    {
        EditorWindow.GetWindow<ScriptableObjectMenu>();
    }

	/// <summary>
	/// Setup and hash the list of available scriptable objects using reflection
	/// </summary>
    public void Setup()
    {
        if (isSetup)
        {
            return;
        }
        isSetup = true;

		Assembly []referencedAssemblies = System.AppDomain.CurrentDomain.GetAssemblies();

		typesList = new List<Type>();
		names = new List<string>();

		typesList.Add(null);
		names.Add("none");

		foreach (Assembly assembly in referencedAssemblies)
		{
			IEnumerable<Type> rawTypes = from type in assembly.GetTypes() where Attribute.IsDefined(type, typeof(ScriptableAttribute)) select type;
			IEnumerable<string> rawNames = from type in rawTypes select type.ToString();

			typesList.AddRange(rawTypes);
			names.AddRange(rawNames);
		}
    }

	/// <summary>
	/// Let the user select what king of scriptable object it desires to create and do so
	/// </summary>
    public void OnGUI()
    {
        Setup();

		// Check if we can go directly to the Scriptable Object instance
		if(selectedScriptableTypeIdx == 0)
		{
			MonoScript script = Selection.activeObject as MonoScript;
			if(script != null)
			{
				int idx = typesList.IndexOf(script.GetClass());
				if(idx > 0)
				{
					selectedScriptableTypeIdx = idx;
				}
			}
		}

        EditorGUILayout.BeginVertical();
        {
            selectedScriptableTypeIdx = EditorGUILayout.Popup(selectedScriptableTypeIdx, names.ToArray<string>());
            EditorGUILayout.Space();
            
			EditorGUI.BeginDisabledGroup(selectedScriptableTypeIdx == 0);
			if(GUILayout.Button("Enter"))
			{
				ScriptableObjectMenu.CreateItem(typesList[selectedScriptableTypeIdx]);
				Close();
			}
			EditorGUI.EndDisabledGroup();
			            
        }
        EditorGUILayout.EndVertical();
    }

	/// <summary>
	/// Helper function to create a scriptable object based on a given type
	/// </summary>
	protected static void CreateItem(Type type)
	{
		string rawPath = AssetDatabase.GetAssetPath(Selection.activeObject);
		string path = rawPath;

		if(type.IsSubclassOf(typeof(ScriptableObject)))
		{
			int idx = rawPath.IndexOf("/" + type.ToString());
			path = rawPath.Substring(0, idx);
		}

		ScriptableObject obj = ScriptableObject.CreateInstance(type);
		AssetDatabase.CreateAsset(obj, string.Format("{0}/{1}.asset", path, type.ToString()));
	}
}
