using System;

namespace hvs.attributes
{
	/// <summary>
	/// Simple attribute tag for HVS scriptable objects
	/// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ScriptableAttribute : Attribute
    {
    }
}