using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProcessListOfNumbers : MonoBehaviour {

	public List<int> numbers = new List<int>();
	List<int> GetTopNumbers(List<int> numbers, int k)
	{
		Dictionary<int, int> counter = new  Dictionary<int, int>();
		
		foreach(int i in numbers)
		{
			if(!counter.ContainsKey(i))
			{
				counter[i] = 0;
			}
			counter[i] += 1;
		}
		
		List<int> sortedValues = new List<int>(counter.Keys);
		sortedValues.Sort((int i, int j) => { return counter[i] < counter[j]? 1 : -1; });	
				
		return sortedValues.GetRange(0, k);
	}

	void Update()
	{
		if(!Input.GetKeyDown(KeyCode.Space))
			return;

		List<int> val = GetTopNumbers(numbers, 1);
		foreach (int i in val)
		{
			Debug.Log(i);
		}
		Debug.Log("Break");

		val = GetTopNumbers(numbers, 2);
		foreach (int i in val)
		{
			Debug.Log(i);
		}
		Debug.Log("Break");

		val = GetTopNumbers(numbers, 3);
		foreach (int i in val)
		{
			Debug.Log(i);
		}
		Debug.Log("Break");
	}
}
