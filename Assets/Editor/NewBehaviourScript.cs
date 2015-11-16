using UnityEngine;
using System.Collections;
using UnityEditor;

public class MySliderDrawer : PropertyDrawer
{
	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		return 17f;
	}

	private GUISkin _sliderSkin;

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		if (_sliderSkin == null)
			_sliderSkin = (GUISkin)EditorGUIUtility.LoadRequired ("MyCustomSlider Skin");

		label = EditorGUI.BeginProperty (position, label, property);
		position = EditorGUI.PrefixLabel (position, label);
		MyCustomSlider (position, property, _sliderSkin.GetStyle ("MyCustomSlider"));
		EditorGUI.EndProperty ();
	}

	public static void MyCustomSlider(Rect controlRect, SerializedProperty property, GUIStyle style)
	{
		int controlID = GUIUtility.GetControlID (FocusType.Passive);

		float value = property.floatValue;

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
			property.floatValue = Mathf.Clamp01 (relativeX / controlRect.width);
			// Report that the data in the GUI has (probably) changed
			GUI.changed = true;
			// Mark the event as 'used' so that other controls don't respond to it
			Event.current.Use ();
		}
	}
}