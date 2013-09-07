using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AdvancedLogWindow : EditorWindow {
	
	private Vector2 scrollView;
	private List<string> items = new List<string>() {"obj1", "obj2", "obj3"};
	
	
	[MenuItem("Window/AdvancedLog")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(AdvancedLogWindow));
	}
	
	void OnGUI()
	{
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.BeginVertical();
		scrollView = EditorGUILayout.BeginScrollView(scrollView, GUILayout.Width(300));
		foreach (string str in items)
			EditorGUILayout.LabelField(str);
		/*EditorGUILayout.LabelField("obj1");
		EditorGUILayout.LabelField("obj2");
		EditorGUILayout.LabelField("obj3");
		EditorGUILayout.LabelField("obj4");
		EditorGUILayout.LabelField("obj5");
		EditorGUILayout.LabelField("obj6");
		EditorGUILayout.LabelField("obj7");
		EditorGUILayout.LabelField("obj8");
		EditorGUILayout.LabelField("obj9");*/
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
		
		/*EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField("obj4");
		EditorGUILayout.LabelField("obj5");
		EditorGUILayout.LabelField("obj6");
		EditorGUILayout.EndVertical();*/
		
		EditorGUILayout.EndHorizontal();
		//EditorGUI.LabelField(new Rect(0,0, 50, 30), "obj1");
		//EditorGUI.LabelField(new Rect(0,30, 10, 50), "obj2");
		//EditorGUI.LabelField(new Rect(0,60, 10, 50), "obj3");
	}
}
