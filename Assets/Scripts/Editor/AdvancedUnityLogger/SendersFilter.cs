using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class SendersFilter
{
	private static Vector2 _objectsListScrollPosition;
	private static Vector2 _detailedViewScrollPosition;
	
	private static bool _buttonPressed = false;
	private const int _listItemHeight = 15;
	private static Rect _lastRect;
	public static int _lastIndex = -1;
	
	private static Texture2D _highlightTexture;
	private static GUIStyle _pressed;
	
	static SendersFilter()
	{
		//_highlightTexture = Resources.Load("highlight") as Texture2D;
		_highlightTexture = TexturesGenerator.GenerateMonotonicTexture(Color.yellow);
		_pressed = new GUIStyle(GUI.skin.box);
		_pressed.normal.background = _highlightTexture;
	}
	
	private static bool SelectableListItem(int id, string label, Rect rect)
	{
		int controlId = GUIUtility.GetControlID(FocusType.Passive, rect);
		
		switch(Event.current.GetTypeForControl(controlId))
		{
		case EventType.MouseDown:
        	if (rect.Contains(Event.current.mousePosition))
            {
	            GUIUtility.hotControl = controlId;
                Event.current.Use();
                _buttonPressed = true;
            }
            break;
        case EventType.MouseUp:
            if (GUIUtility.hotControl == controlId)
            {
                GUIUtility.hotControl = 0;
                Event.current.Use();
                _buttonPressed = false;
                if (rect.Contains(Event.current.mousePosition)) return true;
            }
            return false;
		case EventType.Repaint:
			GUI.Label(rect, label);
			break;
		}
		return false;
	}
	
	public static int CheckList(Rect rect, List<string> content, Rect parent)
	{
		bool [] results = new bool[content.Count];
		float y = rect.y;
		int id = -1;
		_objectsListScrollPosition = GUI.BeginScrollView(new Rect(rect.x, rect.y, rect.width+20, parent.height*0.6f)
			, _objectsListScrollPosition, rect);
		
		for (int i=0; i<content.Count; i++)
		{
			results[i] = SelectableListItem(i, content[i], new Rect(rect.x, y, rect.width, _listItemHeight));
			y += _listItemHeight;
		}
		
		int index = ArrayUtility.FindIndex(results, (res)=>res==true);
		if (index >= 0)
		{
			if (_pressed.normal.background == null)
				_pressed.normal.background = TexturesGenerator.GenerateMonotonicTexture(Color.yellow);
			GUI.Box(new Rect(rect.x, rect.y+index*_listItemHeight, rect.width, _listItemHeight), "", _pressed);
			SelectableListItem(index, content[index], new Rect(rect.x, rect.y+index*_listItemHeight, rect.width, _listItemHeight));
			_lastRect = new Rect(rect.x, rect.y+index*_listItemHeight, rect.width, _listItemHeight);
			_lastIndex = index;
		}
		else 
		{
			if (_lastRect != null && _lastIndex != -1)
			{
				if (_pressed.normal.background == null)
					_pressed.normal.background = TexturesGenerator.GenerateMonotonicTexture(Color.yellow);
				GUI.Box(_lastRect, "", _pressed);
				SelectableListItem(_lastIndex, content[_lastIndex], _lastRect);
			}
		}
			
		GUI.EndScrollView();

		return ArrayUtility.FindIndex(results, (res)=>res==true);
	}
	
	public static void DetailedDescriptionArea(Rect rect, Rect parent, string description)
	{
		_detailedViewScrollPosition = GUI.BeginScrollView(new Rect(rect.x, rect.y, rect.width+20, parent.height*0.4f),
			_detailedViewScrollPosition, rect);
		GUI.TextArea(rect, description);
		GUI.EndScrollView();
	}
	
	public static void Destroy()
	{
		_lastIndex = -1;
		//_generator = null;
	}
}


