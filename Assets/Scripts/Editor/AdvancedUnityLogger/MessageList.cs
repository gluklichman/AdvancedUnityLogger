using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class MessageList
{
	private static Texture2D _highlightTexture;
	private static GUIStyle _chooseStyle;
	
	private static bool _messagePressed = false;
	private static float _messageHeight = 30;
	
	private static Vector2 _messageListScrollPosition;
	
	private static Rect _lastRect;
	public static int _lastIndex;
	
	private static Texture2D _secondMessageBack;
	private static GUIStyle _secondMessageStyle;
	
	private static int[] _doubleClickIndexes;
	public static int _doubleClickIndex = -1;
	
	
	static MessageList()
	{
		_highlightTexture = TexturesGenerator.GenerateMonotonicTexture(new Color(50/255.0f, 135/255.0f, 255/255.0f));
		_chooseStyle = new GUIStyle(GUI.skin.box);
		_chooseStyle.normal.background = _highlightTexture;
		
		_secondMessageBack = TexturesGenerator.GenerateMonotonicTexture(new Color(60/255.0f, 60/255.0f, 60/255.0f));
		_secondMessageStyle = new GUIStyle(GUI.skin.label);
		_secondMessageStyle.normal.background = _secondMessageBack;
		_secondMessageStyle.alignment = TextAnchor.UpperLeft;
	}
	
	private static bool MessageListItem(int id, string message, Rect rect, bool overrideBackground = true)
	{
		int controlId = GUIUtility.GetControlID(FocusType.Passive, rect);
		if (rect.Contains(Event.current.mousePosition))
			_doubleClickIndexes[id] = Event.current.clickCount;
		switch(Event.current.GetTypeForControl(controlId))
		{
		case EventType.MouseDown:
        	if (rect.Contains(Event.current.mousePosition))
            {
	            GUIUtility.hotControl = controlId;
                Event.current.Use();
                _messagePressed = true;
            }
            break;
        case EventType.MouseUp:
            if (GUIUtility.hotControl == controlId)
            {
                GUIUtility.hotControl = 0;
                Event.current.Use();
                _messagePressed = false;
                if (rect.Contains(Event.current.mousePosition)) return true;
            }
            return false;
		case EventType.Repaint:
			if (overrideBackground)
			{
				if (id % 2 == 0)
					GUI.Label(rect, message);
				else
				{
					_secondMessageStyle.normal.background = TexturesGenerator.GenerateMonotonicTexture(new Color(60/255.0f, 60/255.0f, 60/255.0f));
					GUI.Label(rect, message, _secondMessageStyle);
				}
			}
			else
			{
				GUI.Label(rect, message);
			}
			break;
		}
		return false;
	}
	
	public static int CreateMessageList(Rect rect, List<string> content, Rect parent)
	{
		bool [] results = new bool[content.Count];
		_doubleClickIndexes = new int[content.Count];
		float y = rect.y;
		int id = -1;
		_messageListScrollPosition = GUI.BeginScrollView(new Rect(rect.x, rect.y, rect.width, parent.height*0.6f-10)
			, _messageListScrollPosition, 
			new Rect(rect.x, rect.y, rect.width-20, rect.height));
		
		for (int i=0; i<content.Count; i++)
		{
			results[i] = MessageListItem(i, content[i], new Rect(rect.x, y, rect.width, _messageHeight));
			y += _messageHeight;
		}
		
		int index = ArrayUtility.FindIndex(results, (res)=>res==true);
		if (index >= 0 && content.Count > 0)
		{
			_chooseStyle.normal.background = TexturesGenerator.GenerateMonotonicTexture(new Color(50/255.0f, 135/255.0f, 255/255.0f));
			GUI.Box(new Rect(rect.x, rect.y+index*_messageHeight, rect.width, _messageHeight), "", _chooseStyle);
			MessageListItem(index, content[index], new Rect(rect.x, rect.y+index*_messageHeight, rect.width, _messageHeight));
			_lastRect = new Rect(rect.x, rect.y+index*_messageHeight, rect.width, _messageHeight);
			_lastIndex = index;
		}
		else 
		{
			if (content.Count == 0)
			{
				GUI.EndScrollView();
				return -1;
			}
			if (_lastRect != null && _lastIndex != -1 && _lastIndex < content.Count)
			{
				_chooseStyle.normal.background = TexturesGenerator.GenerateMonotonicTexture(new Color(50/255.0f, 135/255.0f, 255/255.0f));
				GUI.Box(_lastRect, "", _chooseStyle);
				MessageListItem(_lastIndex, content[_lastIndex], _lastRect, false);
			}
		}
			
		GUI.EndScrollView();
		for (int i=0; i<content.Count; i++)
		{
			if (_doubleClickIndexes[i] >= 2)
				_doubleClickIndex = i;
		}

		return ArrayUtility.FindIndex(results, (res)=>res==true);
	}
	
	public static void Destroy()
	{
		_lastIndex = -1;
	}
	
	
}


