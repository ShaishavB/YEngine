using UnityEngine;
using System.Collections;
using Yudiz.Gaming.Engine;

public class EngineInputMBox : _EngineInputMBox
{
}
namespace Yudiz.Gaming.Engine
{
	public abstract class _EngineInputMBox : MonoBehaviour
	{
		private static Vector2 touchPressed;
		private static Vector2 touchDown;
		private static Vector2 touchUp;
		private static Vector2 mousePosition;
		public static Vector2 InvalidTouch = new Vector2 (int.MinValue, int.MinValue);

		public static Vector2 TouchPressed {
			get {
				return touchPressed;
			}
		}

		public static Vector2 TouchDown {
			get {
				return touchDown;
			}
		}

		public static Vector2 TouchUp {
			get {
				return touchUp;
			}
		}

		public static Vector2 MousePosition {
			get {
				return mousePosition;
			}
		}

		public static bool IsTouchDown = false;
		public static bool IsTouchPressed = false;
		public static bool IsTouchUp = false;
		// Update is called once per frame
		protected virtual void Update ()
		{
			mousePosition.x = UnityEngine.Input.mousePosition.x;
			mousePosition.y = Screen.height - UnityEngine.Input.mousePosition.y;
			#if !UNITY_EDITOR && !UNITY_STANDALONE
			if (!UnityEngine.Input.GetMouseButtonDown (0) && 
			    !UnityEngine.Input.GetMouseButton (0) && 
			    !UnityEngine.Input.GetMouseButtonUp (0)) {
				mousePosition.x = int.MinValue;
				mousePosition.y = int.MinValue;
			}
			#endif
			
			if (UnityEngine.Input.GetMouseButtonDown (0)) {
				touchDown.x = UnityEngine.Input.mousePosition.x;
				touchDown.y = Screen.height - UnityEngine.Input.mousePosition.y;
				IsTouchDown = true;
			} else {
				IsTouchDown = false;
				touchDown = InvalidTouch;
			}
			
			if (UnityEngine.Input.GetMouseButton (0)) {
				touchPressed.x = UnityEngine.Input.mousePosition.x;
				touchPressed.y = Screen.height - UnityEngine.Input.mousePosition.y;
				IsTouchPressed = true;
			} else {
				touchPressed = InvalidTouch;
				IsTouchPressed = false;
			}
			
			if (UnityEngine.Input.GetMouseButtonUp (0)) {
				touchUp.x = UnityEngine.Input.mousePosition.x;
				touchUp.y = Screen.height - UnityEngine.Input.mousePosition.y;
				IsTouchUp = true;
			} else {
				touchUp = InvalidTouch;
				IsTouchUp = false;
			}
		}
	}
}