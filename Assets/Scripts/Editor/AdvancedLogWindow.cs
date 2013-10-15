using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AdvancedLogWindow : EditorWindow {
	
	private static List<string> _senders = new List<string>();
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
	
	[MenuItem("Window/AdvancedLog")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(AdvancedLogWindow));
	}
	
	#region GUI	
	void OnGUI()
	{
		int id = CustomUIComponents.CheckList(new Rect(0,0,100,_senders.Count*_listItemHeight), _senders, position);
		GetCheckedCategory(id);
		if (_categoryToShow >= 0 && _categoryToShow<_senders.Count)
		{
			string key = _senders[_categoryToShow];
			if (_categorizedMessages.ContainsKey(key))
			{
				int messageId = MessageList.CreateMessageList(new Rect(130, 20, position.width-150, _categorizedMessages[key].Count*_messageHeight), _categorizedMessages[key], position);
				GetCheckedMessage(messageId);
			}
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
		CustomUIComponents.DetailedDescriptionArea(new Rect(0, position.height*0.6f+10, position.width, 300),
			position, detailed);
	}
	
	private void GetCheckedCategory(int categoryId)
	{
		if (categoryId != -1 && categoryId != _categoryToShow)
			_categoryToShow = categoryId;
	}
	
	private void CreateButtons()
	{
		if (GUI.Button(new Rect(230,0,80,17), "Clear"))
		{
			ClearLists();
			MessageList._lastIndex = -1;
			CustomUIComponents._lastIndex = -1;
		}
		GUI.Box(new Rect(130, 0, position.width - 150, 20), "");
		_collapse = GUI.Toggle(new Rect(130,0,100,20), _collapse, "Collapse");
	}
	#endregion
	
	private void LogCallback(string logText, string stackTrace, LogType type)
	{
		//prepare message
		int fromIndex = logText.IndexOf("###");
		int toIndex = logText.IndexOf("###", 1);
		string senderString = logText.Substring(fromIndex, toIndex);
		senderString = senderString.Replace("###","");
		logText = logText.Remove(0, toIndex+3);
		
		if (!_senders.Contains(senderString))
			_senders.Add(senderString);
		//_logMessages.Add(logText);
		//_detailedDescriptions.Add(stackTrace);
		
		if (_categorizedMessages.ContainsKey(senderString))
			_categorizedMessages[senderString].Add(logText);
		else
		{
			_categorizedMessages.Add(senderString, new List<string>());
			_categorizedMessages[senderString].Add(logText);
		}
		if (_categorizedDescriptions.ContainsKey(senderString))
			_categorizedDescriptions[senderString].Add(stackTrace);
		else
		{
			_categorizedDescriptions.Add(senderString, new List<string>());
			_categorizedDescriptions[senderString].Add(stackTrace);
		}
	}
	
	void OnDestroy()
	{
		Application.RegisterLogCallback(null);
		MessageList.Destroy();
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
		//_logMessages.Clear();
		//_detailedDescriptions.Clear();
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
