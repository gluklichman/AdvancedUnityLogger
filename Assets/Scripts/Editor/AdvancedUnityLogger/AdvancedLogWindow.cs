using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AdvancedLogWindow : EditorWindow {
	
	private static List<string> _senders = new List<string>(){"1","2","3","4","5","6", "7", "8", "9", "10", "11", "12"};
	private const int _listItemHeight = 15;
	private const int _messageHeight = 30;
	
	private bool _collapse = false;
	
	private List<string> _logMessages ;
	private List<string> _detailedDescriptions;
	
	private int _indexToShow = -1;
	private int _categoryToShow = -1;
	
	private static bool _applicationStarted = false;
	
	private static Dictionary<string, List<string>> _categorizedMessages;
	private static Dictionary<string, List<string>> _categorizedDescriptions;
	
	private int _resizeWidth = 120;
	private bool _mouseDrag = false;
	
	[MenuItem("Window/AdvancedLog")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(AdvancedLogWindow));
	}
	
	#region GUI	
	
	private void MouseProcess()
	{
		switch (Event.current.type)
		{
		case EventType.MouseDown:
			if (new Rect(_resizeWidth+20, 0 ,10, position.height*0.6f).Contains(Event.current.mousePosition))
				_mouseDrag = true;
			break;
		case EventType.MouseUp:
			_mouseDrag = false;
			break;
		case EventType.MouseDrag:
			if (Event.current.mousePosition.x > position.width*0.3f)
				return;
			if (_mouseDrag)
			{
				_resizeWidth = (int)Event.current.mousePosition.x-20 ;
				Repaint();
			}
			break;
			
		}
	}
	
	void OnGUI()
	{
		MouseProcess();
		GUI.Box(new Rect(0, 0, _resizeWidth+20, position.height*0.6f), "");
		int id = SendersFilter.CheckList(new Rect(0,0,_resizeWidth,_senders.Count*_listItemHeight), _senders, position);
		EditorGUIUtility.AddCursorRect(new Rect(_resizeWidth+20,0,10, position.height*0.6f), MouseCursor.ResizeHorizontal);
		GetCheckedCategory(id);
		if (_categoryToShow >= 0 && _categoryToShow<_senders.Count)
		{
			string key = _senders[_categoryToShow];
			if (_categorizedMessages.ContainsKey(key))
			{
				int messageId = -1;
				messageId = MessageList.CreateMessageList(new Rect(_resizeWidth+20, 20, position.width-_resizeWidth-20, 
					_categorizedMessages[key].Count*_messageHeight), _categorizedMessages[key], position);
				GetCheckedMessage(messageId);
			}
		}
		else
		{
			SendersFilter.DetailedDescriptionArea(new Rect(0, position.height*0.6f+10, position.width, 300),
			position, "");
		}
		CreateButtons();
		
	}
	
	private void GetCheckedMessage(int messageId)
	{
		if (messageId != -1 && messageId != _indexToShow)
			_indexToShow = messageId;
		string key = _senders[_categoryToShow];
		string detailed = (_indexToShow >= 0 && _indexToShow < _categorizedDescriptions[key].Count)?
			//_detailedDescriptions[_indexToShow] : "";
			_categorizedDescriptions[key][_indexToShow] : "";
		SendersFilter.DetailedDescriptionArea(new Rect(0, position.height*0.6f+10, position.width, 300),
			position, detailed);
		
		/*if (MessageList._doubleClickIndex >=0 && MessageList._doubleClickIndex < _categorizedDescriptions[key].Count)
		{
			Debug.Log("Open editor");
			OpenEditor(key);
			MessageList._doubleClickIndex = -1;
		}*/
	}
	
	private void OpenEditor(string key)
	{
		string[] log = _categorizedDescriptions[key][_indexToShow].Split('\n');
		string senderString = log[log.Length-2];
		int atIndex = senderString.IndexOf("at");
		senderString = senderString.Substring(atIndex+3);
		senderString = senderString.Remove(senderString.Length-1);
		string [] parts = senderString.Split(':');
		string fileName = parts[0];
		int lineNumber = int.Parse(parts[1]);
		Object asset = AssetDatabase.LoadMainAssetAtPath(fileName);
		AssetDatabase.OpenAsset(asset, lineNumber);
		//Debug.Log(guid);
	}
	
	private void GetCheckedCategory(int categoryId)
	{
		if (categoryId != -1 && categoryId != _categoryToShow)
			_categoryToShow = categoryId;
	}
	
	private void CreateButtons()
	{
		if (GUI.Button(new Rect(_resizeWidth+150,0,80,17), "Clear"))
		{
			ClearLists();
			MessageList._lastIndex = -1;
			SendersFilter._lastIndex = -1;
		}
		GUI.Box(new Rect(_resizeWidth+20, 0, position.width - _resizeWidth, 20), "");
		_collapse = GUI.Toggle(new Rect(_resizeWidth+40,0,100,20), _collapse, "Collapse");
	}
	#endregion
	
	#region loggingFunctionality
	private void LogCallback(string logText, string stackTrace, LogType type)
	{
		int fromIndex = logText.IndexOf("###");
		int toIndex = logText.IndexOf("###", 1);
		if (fromIndex == -1 || toIndex == -1)
			return;
		string senderString = logText.Substring(fromIndex, toIndex);
		senderString = senderString.Replace("###","");
		logText = logText.Remove(0, toIndex+3);
		
		if (!_senders.Contains(senderString))
			_senders.Add(senderString);
		AddMessageToDictionary(_categorizedMessages, logText, senderString);
		AddMessageToDictionary(_categorizedDescriptions, stackTrace, senderString);
	}
	
	private void AddMessageToDictionary(Dictionary<string, List<string>> dict, string message, string key)
	{
		if (dict.ContainsKey(key))
		{
			dict[key].Add(message);
		}
		else
		{
			dict.Add(key, new List<string>());
			dict[key].Add(message);
		}
	}
	#endregion
	
	void OnDestroy()
	{
		Application.RegisterLogCallback(null);
		MessageList.Destroy();
		SendersFilter.Destroy();
	}
	
	void OnEnable()
	{
		if (_categorizedMessages == null && _categorizedDescriptions == null)
		{
			_categorizedMessages = new Dictionary<string, List<string>>();
			_categorizedDescriptions = new Dictionary<string, List<string>>();
		}
		//if (!_applicationStarted)
		//	ClearLists();
		Application.RegisterLogCallback(LogCallback);
	}
	
	void ClearLists()
	{
		_senders.Clear();
		
		_categorizedMessages.Clear();
		_categorizedDescriptions.Clear();
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
