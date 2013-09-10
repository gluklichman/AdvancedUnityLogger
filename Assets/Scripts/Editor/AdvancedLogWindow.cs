using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AdvancedLogWindow : EditorWindow {
	
	private Vector2 scrollView;
	private static List<string> items = new List<string>() {"obj1", "obj2", "obj3","obj1", "obj2", "obj3","obj1", "obj2", "obj3","obj1", "obj2", "obj3"};
	private bool pressed = false;
	
	private const int listItemHeight = 15;
	
	
	
	[MenuItem("Window/AdvancedLog")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(AdvancedLogWindow));
	}
	
	void OnGUI()
	{
		int id = CustomUIComponents.CheckList(new Rect(0,0,100,items.Count*listItemHeight), items, position);
		if (id >= 0)
			Debug.Log(id);
	}
	
	public static void AddListener(string listener)
	{
		items.Add(listener);
	}
}
