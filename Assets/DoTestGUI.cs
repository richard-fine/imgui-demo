using UnityEngine;
using System.Collections;
//using UnityEditor;

[ExecuteInEditMode]
public class DoTestGUI : MonoBehaviour {

	public GUIStyle labelStyle;
	public float labelWidth;
	
	public float value1;
	public float value2;
	public float value3;

	public GUISkin _skin;
	/*private static void InitSkin()
	{
		_skin = .LoadAssetAtPath<GUISkin> ("Assets/New GUISkin.guiskin");
	}*/

	public void OnGUI()
	{
		//if (!_skin)
		//	InitSkin ();

		GUILayout.BeginHorizontal (GUILayout.Width(Screen.width - 20f));
		GUILayout.Space (10f);
		GUILayout.Label ("Value 1", labelStyle, GUILayout.Width(labelWidth));
		value1 = DoCustomSlider (value1, controlStyle);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal (GUILayout.Width(Screen.width - 20f));
		GUILayout.Space (10f);
		GUILayout.Label ("Value 2", labelStyle, GUILayout.Width(labelWidth));
		value2 = DoCustomSlider (value2, controlStyle);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal (GUILayout.Width(Screen.width - 20f));
		GUILayout.Space (10f);
		GUILayout.Label ("Value 3", labelStyle, GUILayout.Width(labelWidth));
		value3 = DoCustomSlider (value3, controlStyle);
		GUILayout.EndHorizontal ();

		//FlashingButton (new GUIContent("Flashy!"));
	}

	public GUIStyle controlStyle;

	public float DoCustomSlider(float value, GUIStyle style, params GUILayoutOption[] layoutOptions)
	{
		var rect = GUILayoutUtility.GetRect(GUIContent.none, controlStyle, layoutOptions);
		return MyCustomSlider (rect, value, style);
	}

	public static float MyCustomSlider(Rect controlRect, float value, GUIStyle style)
	{
		int controlID = GUIUtility.GetControlID (FocusType.Passive);

		switch (Event.current.GetTypeForControl(controlID))
		{
			case EventType.repaint:
			{
				// Work out the width of the bar in pixels by lerping
				int pixelWidth = (int)Mathf.Lerp (1, controlRect.width, value);

				// Build up the rectangle that the bar will cover
				Rect targetRect = new Rect (controlRect){ width = pixelWidth };

				// Tint whatever we draw to be red/green depending on the value
				GUI.color = Color.Lerp (Color.red, Color.green, value);

				// Draw the texture from the GUIStyle, applying the tint we just set
				GUI.DrawTexture (targetRect, style.normal.background);

				// Reset the tint back to white
				GUI.color = Color.white;

				break;
			}

			case EventType.mouseDown:
			{
				// Only capture the mouse if the click is actually on us
				if (controlRect.Contains (Event.current.mousePosition)
					&& GUIUtility.hotControl == 0
					&& Event.current.button == 0)
					GUIUtility.hotControl = controlID;
	
				break;
			}

			case EventType.mouseUp:
			{
				// If we were the hotControl, we aren't any more.
				if (GUIUtility.hotControl == controlID)
					GUIUtility.hotControl = 0;

				break;
			}

		}

		if (Event.current.isMouse && GUIUtility.hotControl == controlID) {
			// Calculate mouse X position relative to the left edge of the control
			float relativeX = Event.current.mousePosition.x - controlRect.x;
			// Divide by control width to get this as a value between 0 and 1
			value = Mathf.Clamp01 (relativeX / controlRect.width);
			// Report that the data in the GUI has (probably) changed
			GUI.changed = true;
			// Mark the event as 'used' so that other controls don't respond to it
			Event.current.Use ();
		}

		return value;
	}

	/*public class FlashingButtonInfo
	{
		public double mouseDownAt;

		public void MouseDownNow()
		{
			mouseDownAt = EditorApplication.timeSinceStartup;
		}

		public bool IsFlashing(int controlID)
		{
			if (GUIUtility.hotControl != controlID)
				return false;

			double elapsedTime = EditorApplication.timeSinceStartup - mouseDownAt;
			if (elapsedTime < 2f)
				return false;
			return (int)((elapsedTime - 2f) / 0.1f) % 2 == 0;
		}
	}

	public static bool FlashingButton(GUIContent content)
	{
		var style = _skin.GetStyle("flashingButton");
		var rect = GUILayoutUtility.GetRect (content, style);
		return FlashingButton (rect, content, style);
	}

public static bool FlashingButton(Rect rc, GUIContent content, GUIStyle style)
{
	int controlID = GUIUtility.GetControlID (FocusType.Native);

	// Get (or create) the state object
	var state = (FlashingButtonInfo)GUIUtility.GetStateObject (typeof(FlashingButtonInfo), 
								   controlID);

	switch (Event.current.GetTypeForControl(controlID)) {
		case EventType.Repaint:
		{
			GUI.color = state.IsFlashing (controlID) ? Color.red : Color.white;
			style.Draw (rc, content, controlID);
			break;
		}
		case EventType.mouseDown:
		{
			if (rc.Contains (Event.current.mousePosition) 
			 && Event.current.button == 0
			 && GUIUtility.hotControl == 0) 
			{
				GUIUtility.hotControl = controlID;
				state.mouseDownAt = UnityEditor.EditorApplication.timeSinceStartup;
			}
			break;
		}
		case EventType.mouseUp:
		{
			if (GUIUtility.hotControl == controlID)
				GUIUtility.hotControl = 0;
			break;
		}
	}

	return GUIUtility.hotControl == controlID;
}*/
}

