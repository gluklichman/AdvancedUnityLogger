using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AdvancedLogWindow : EditorWindow {
	
	private static List<string> _items = new List<string>() {"obj1", "obj2", "obj3","obj1", "obj2", "obj3","obj1", "obj2", "obj3","obj1", "obj2", "obj3"};
	private const int _listItemHeight = 15;
	
	private const string _description = @"Assets/Scripts/Editor/CustomUIComponents.cs(96,125): error CS1502: The best overloaded method match for `UnityEngine.Rect.Rect(float, float, float, float)' has some invalid argumentsAssets/Scripts/Editor/CustomUIComponents.cs(96,125): error CS1502: The best overloaded method match for `UnityEngine.Rect.Rect(float, float, float, float)' has some invalid arguments";
	
	
	
	[MenuItem("Window/AdvancedLog")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(AdvancedLogWindow));
	}
	
	void OnGUI()
	{
		int id = CustomUIComponents.CheckList(new Rect(0,0,100,_items.Count*_listItemHeight), _items, position);
		CustomUIComponents.DetailedDescriptionArea(new Rect(0, position.height*0.6f, position.width, 300), position, _description);
		MessageList.CreateMessageList(new Rect(130, 0, position.width-150, _items.Count*_listItemHeight), _items, position);
	}
	
	public static void AddListener(string listener)
	{
		_items.Add(listener);
	}
}
