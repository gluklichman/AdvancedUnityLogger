using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class CustomUIComponents
{
	private static Vector2 scrollPosition;
	private static bool buttonPressed = false;
	private const int listItemHeight = 15;
	private static Rect lastRect;
	private static int lastIndex = -1;
	
	private static Texture2D highlightTexture;
	private static GUIStyle pressed;
	
	static CustomUIComponents()
	{
		highlightTexture = Resources.Load("highlight") as Texture2D;
		pressed = new GUIStyle(GUI.skin.box);
		pressed.normal.background = highlightTexture;
	}
	
	private static bool SelectableListItem(int id, string label, Rect rect)
	{
		int controlId = GUIUtility.GetControlID(FocusType.Passive, rect);
		bool selected = false;
		
		switch(Event.current.GetTypeForControl(controlId))
		{
		case EventType.MouseDown:
        	if (rect.Contains(Event.current.mousePosition))
            {
	            GUIUtility.hotControl = controlId;
                Event.current.Use();
                buttonPressed = true;
            }
            break;
        case EventType.MouseUp:
            if (GUIUtility.hotControl == controlId)
            {
                GUIUtility.hotControl = 0;
                Event.current.Use();
                buttonPressed = false;
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
		scrollPosition = GUI.BeginScrollView(new Rect(rect.x, rect.y, rect.width+20, parent.height*0.8f)
			, scrollPosition, rect);
		
		for (int i=0; i<content.Count; i++)
		{
			results[i] = SelectableListItem(i, content[i], new Rect(rect.x, y, rect.width, listItemHeight));
			y += listItemHeight;
		}
		
		int index = ArrayUtility.FindIndex(results, (res)=>res==true);
		if (index >= 0)
		{
			GUI.Box(new Rect(rect.x, rect.y+index*listItemHeight, rect.width, listItemHeight), "", pressed);
			SelectableListItem(index, content[index], new Rect(rect.x, rect.y+index*listItemHeight, rect.width, listItemHeight));
			lastRect = new Rect(rect.x, rect.y+index*listItemHeight, rect.width, listItemHeight);
			lastIndex = index;
		}
		else 
		{
			if (lastRect != null && lastIndex != -1)
			{
				GUI.Box(lastRect, "", pressed);
				SelectableListItem(lastIndex, content[lastIndex], lastRect);
			}
		}
			
		GUI.EndScrollView();

		return ArrayUtility.FindIndex(results, (res)=>res==true);
	}
}


