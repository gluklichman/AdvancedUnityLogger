using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AdvancedLogWindow : EditorWindow {
	
	private static List<string> _items = new List<string>() {"obj1", "obj2", "obj3","obj1", "obj2", "obj3","obj1", "obj2", "obj3","obj1", "obj2", "o	bj3"};
	private const int _listItemHeight = 15;
	private const int _messageHeight = 30;
	
	private const string _description = @"Assets/Scripts/Editor
/CustomUIComponents.cs(96,125): error CS1502: The best overloaded method match for `UnityEngine.Rect.Rect(float, float, float, float)' has some invalid argumentsAssets/Scripts/Editor/CustomUIComponents.cs(96,125): error CS1502: The best overloaded method match for `UnityEngine.Rect.Rect(float, float, float, float)' has some invalid arguments";
	
	private bool _collapse = false;
	
	[MenuItem("Window/AdvancedLog")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(AdvancedLogWindow));
	}
	
	void OnGUI()
	{
		int id = CustomUIComponents.CheckList(new Rect(0,0,100,_items.Count*_listItemHeight), _items, position);
		CustomUIComponents.DetailedDescriptionArea(new Rect(0, position.height*0.6f+10, position.width, 300), position, _description);
		MessageList.CreateMessageList(new Rect(130, 20, position.width-150, _items.Count*_messageHeight), _items, position);
		_collapse = GUI.Toggle(new Rect(130,0,100,20), _collapse, "Collapse");
		if (GUI.Button(new Rect(230,0,80,17), "Clear"))
		{
		}
		GUI.Box(new Rect(130, 0, position.width - 150, 20), "");
		
	}
}
