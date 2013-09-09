using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AdvancedLogWindow : EditorWindow {
	
	private Vector2 scrollView;
	private List<string> items = new List<string>() {"obj1", "obj2", "obj3"};
	int selected = 0;
	public GUIStyle style;
	
	
	[MenuItem("Window/AdvancedLog")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(AdvancedLogWindow));
	}
	
	//public Rect position = new Rect(0, 0, 100, 100);

    private bool pressed = false;

 	private bool SelectableListItem(int id, string label, Rect rect)
	{
		int controlId = GUIUtility.GetControlID(id, FocusType.Passive, rect);
		switch(Event.current.GetTypeForControl(controlId))
		{
		case EventType.MouseDown:
        	if (rect.Contains(Event.current.mousePosition))
            {
	            GUIUtility.hotControl = controlId;
                Event.current.Use();
                pressed = true;
            }
            break;
        case EventType.MouseUp:
            if (GUIUtility.hotControl == controlId)
            {
                GUIUtility.hotControl = 0;
                Event.current.Use();
                pressed = false;
                if (rect.Contains(Event.current.mousePosition)) return true;
            }
            return false;
		case EventType.Repaint:
			//GUI.Box(rect, "");
			GUI.Label(rect, label);
			break;
		}
		return false;
	}
	
	void OnGUI()
	{
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.BeginVertical();
		scrollView = EditorGUILayout.BeginScrollView(scrollView, GUILayout.Width(300));
		if (SelectableListItem(1, items[0], new Rect(0, 0, 100, 15)))
			Debug.Log(items[0]);
		if (SelectableListItem(1, items[1], new Rect(0, 16, 100, 15)))
			Debug.Log(items[1]);
		
		
		//selected = GUI.SelectionGrid(new Rect(0,0, 300, 100), selected, items.ToArray(), 1);
		
		//foreach (string str in items)
		//		GUI.SelectionGrid(
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
