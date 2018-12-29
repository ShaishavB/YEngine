using System.ComponentModel;
using UnityEngine;
using Yudiz.Gaming.Engine;

public class EngineInput : _EngineInput
{
}
namespace Yudiz.Gaming.Engine
{
	public abstract class _EngineInput : MonoBehaviour
	{
		private static Vector2 touchPressed;
		private static Vector2 touchDown;
		private static Vector2 touchUp;
		private static Vector2 mousePosition;
		public static Vector2 InvalidTouch = new Vector2 (int.MinValue, int.MinValue);

		public static Vector2 TouchPressed {
			get {
				if (GameEngine.IsMessageOn) {
					return InvalidTouch;
				} else {
					return touchPressed;
				}
			}
		}

		public static Vector2 TouchDown {
			get {
				if (GameEngine.IsMessageOn) {
					return InvalidTouch;
				} else {
					return touchDown;
				}
			}
		}

		public static Vector2 TouchUp {
			get {
				if (GameEngine.IsMessageOn) {
					return InvalidTouch;
				} else {
					return touchUp;
				}
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
				touchDown = mousePosition;
				IsTouchDown = true;
			} else {
				IsTouchDown = false;
				touchDown = InvalidTouch;
			}

			if (UnityEngine.Input.GetMouseButton (0)) {
				touchPressed = mousePosition;
				IsTouchPressed = true;
			} else {
				touchPressed = InvalidTouch;
				IsTouchPressed = false;
			}

			if (UnityEngine.Input.GetMouseButtonUp (0)) {
				touchUp = mousePosition;
				IsTouchUp = true;
			} else {
				touchUp = InvalidTouch;
				IsTouchUp = false;
			}

			//if (Input.touchCount> 0 && Input.touchCount < 3)
			//{
			//    foreach (Touch t in Input.touches)
			//    {
			//        if (t.phase == TouchPhase.Began)
			//        {
			//            touchDown = mousePosition;
			//            IsTouchDown = true;
			//        }
			//        else
			//        {
			//            IsTouchDown = false;
			//            touchDown = InvalidTouch;
			//        }

			//        if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Moved)
			//        {
			//            touchPressed = mousePosition;
			//            IsTouchPressed = true;
			//        }
			//        else
			//        {
			//            touchPressed = InvalidTouch;
			//            IsTouchPressed = false;
			//        }

			//        if (t.phase == TouchPhase.Ended)
			//        {
			//            touchUp = mousePosition;
			//            IsTouchUp = true;
			//        }
			//        else
			//        {
			//            touchUp = InvalidTouch;
			//            IsTouchUp = false;
			//        }
			//    }
			//}
		}
	}
}