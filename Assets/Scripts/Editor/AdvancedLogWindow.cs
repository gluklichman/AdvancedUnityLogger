using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AdvancedLogWindow : EditorWindow {
	
	private static List<string> _items = new List<string>() {"obj1", "obj2", "obj3","obj1", "obj2", "obj3","obj1", "obj2", "obj3","obj1", "obj2", "o	bj3"};
	private const int _listItemHeight = 15;
	private const int _messageHeight = 30;
	
	private bool _collapse = false;
	
	private List<string> _logMessages = new List<string>();
	private List<string> _detailedDescriptions = new List<string>();
	private int _indexToShow = -1;
	
	private bool _applicationStarted = false;
	
	[MenuItem("Window/AdvancedLog")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(AdvancedLogWindow));
	}
	
	#region GUI	
	void OnGUI()
	{
		int id = CustomUIComponents.CheckList(new Rect(0,0,100,_items.Count*_listItemHeight), _items, position);
		CreateButtons();
		int messageId = MessageList.CreateMessageList(new Rect(130, 20, position.width-150, _logMessages.Count*_messageHeight), _logMessages, position);
		GetCheckedMessage(messageId);
	}
	
	private void GetCheckedMessage(int messageId)
	{
		if (messageId != -1 && messageId != _indexToShow)
			_indexToShow = messageId;
		string detailed = (_indexToShow >= 0 && _indexToShow < _detailedDescriptions.Count)?
			_detailedDescriptions[_indexToShow] : "";
		CustomUIComponents.DetailedDescriptionArea(new Rect(0, position.height*0.6f+10, position.width, 300),
			position, detailed);
	}
	
	private void CreateButtons()
	{
		if (GUI.Button(new Rect(230,0,80,17), "Clear"))
		{
		}
		GUI.Box(new Rect(130, 0, position.width - 150, 20), "");
		_collapse = GUI.Toggle(new Rect(130,0,100,20), _collapse, "Collapse");
	}
	#endregion
	
	private void LogCallback(string logText, string stackTrace, LogType type)
	{
		_logMessages.Add(logText);
		_detailedDescriptions.Add(stackTrace);
	}
	
	void OnDestroy()
	{
		Application.RegisterLogCallback(null);
		MessageList.Destroy();
	}
	
	void OnEnable()
	{
		ClearLists();
		Application.RegisterLogCallback(LogCallback);
	}
	
	void ClearLists()
	{
		_logMessages.Clear();
		_detailedDescriptions.Clear();
		Repaint();
	}
	
	void OnInspectorUpdate()
	{
		if (Application.isPlaying && !_applicationStarted)
		{
			_applicationStarted = true;
			ClearLists();
		}
		if (Application.isPlaying && _applicationStarted)
		{
			Repaint();
		}
		if (!Application.isPlaying && _applicationStarted)
		{
			_applicationStarted = false;
		}
	}
	
}
