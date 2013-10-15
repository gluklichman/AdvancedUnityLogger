using UnityEngine;
using System.Collections;

public class gamegui : MonoBehaviour {
	
	private int counter = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(0,0, 100, 50), "push"))
		{
			AdvancedUnityLogger.Log("Hello!", "some class");
		}
		if (GUI.Button(new Rect(0,50, 100, 50), "push2"))
		{
			AdvancedUnityLogger.Log("Ololo!", this);
		}
		if (GUI.Button(new Rect(0,100, 100, 50), "push2"))
		{
			AdvancedUnityLogger.Log("123!", this);
		}
	}
	
	void LogTestMessage()
	{
		counter++;
		AdvancedUnityLogger.Log("Counter: "+counter, this);
	}
}
