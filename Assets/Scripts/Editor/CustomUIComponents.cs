using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class CustomUIComponents
{
	private static Vector2 scrollPosition;
	private static bool buttonPressed = false;
	private const int listItemHeight = 15;
	
	private static bool SelectableListItem(int id, string label, Rect rect)
	{
		int controlId = GUIUtility.GetControlID(id, FocusType.Passive, rect);
		bool selected = false;
		
		GUIStyle pressed = new GUIStyle(GUI.skin.box);
		pressed.normal.background = Resources.Load("highlight") as Texture2D;
		
		GUIStyle curstyle = new GUIStyle(GUI.skin.box);
		
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
				curstyle = pressed;
                if (rect.Contains(Event.current.mousePosition)) return true;
            }
            return false;
		case EventType.Repaint:
			GUI.Box(rect, "", curstyle);
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
		GUI.EndScrollView();

		return ArrayUtility.FindIndex(results, (res)=>res==true);
	}
}


