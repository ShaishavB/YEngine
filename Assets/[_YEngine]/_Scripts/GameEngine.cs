using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

//Global settings
namespace Yudiz.Gaming.Engine
{
	public class GameEngine
	{

		public const int SIZE = 8;
		public const string EMPTY_STRING = "";
		public const string NEW_LINE = "\n";
		public const string FIRST_PALY = "FirstPlay";
		public const float DOUBLE_CLICK_THRESH_HOLD = 0.5f;
		public const float PI = 3.1415f;
		public const float VELOCITY_2_SPEED = 3.6f;
		public const float GRAVITY = 9.81f;
		public static string CurrentLayer = "Base";
		public static bool IsPaused = false;
		public static string ErrorMessages = string.Empty;
		private static Vector3[] points = new Vector3[SIZE];
		private static Vector3[] pos = new Vector3[SIZE];
		private static bool isMessageOn = false;
		public static Vector2 MessageOffset = new Vector2 (0f, 0f);

		public delegate void DrawRectDelegate ();

		public delegate void ResetRectDelegate ();

		public static List<DrawRectDelegate> DrawRects;
		public static List<ResetRectDelegate> ResetRects;

		public static void  DrawRect ()
		{
			foreach (DrawRectDelegate dr in DrawRects) {
				dr ();
			}
		}

		public static void  ResetRect ()
		{
			foreach (ResetRectDelegate rr in ResetRects) {
				rr ();
			}
		}

		public static bool IsMessageOn {
			get {
				return isMessageOn;
			}
			set {
				isMessageOn = value;
			}
		}

		public static Rect GetBoundingRect (Transform obj, Camera cam)
		{
			return GetBoundingRect (obj, cam, new Rect (0, 0, 0, 0));
		}

		public static Rect GetBoundingRect (Transform obj, Camera cam, Rect buffer)
		{
			Vector3 center = obj.GetComponent<Renderer> ().bounds.center;
			Vector3 extents = obj.GetComponent<Renderer> ().bounds.extents;
			pos [0] = new Vector3 (center.x - extents.x, center.y - extents.y, center.z - extents.z);

			pos [1] = new Vector3 (center.x - extents.x, center.y - extents.y, center.z + extents.z);
			pos [2] = new Vector3 (center.x - extents.x, center.y + extents.y, center.z - extents.z);
			pos [3] = new Vector3 (center.x - extents.x, center.y + extents.y, center.z + extents.z);
			pos [4] = new Vector3 (center.x + extents.x, center.y - extents.y, center.z - extents.z);
			pos [5] = new Vector3 (center.x + extents.x, center.y - extents.y, center.z + extents.z);
			pos [6] = new Vector3 (center.x + extents.x, center.y + extents.y, center.z - extents.z);
			pos [7] = new Vector3 (center.x + extents.x, center.y + extents.y, center.z + extents.z);
		
			for (int i = 0; i < points.Length; i++) {
				points [i] = cam.WorldToScreenPoint (pos [i]);
				points [i].y = Screen.height - points [i].y;
			}
		
			float xMin, xMax, yMin, yMax;
			xMin = xMax = points [0].x;
			yMin = yMax = points [0].y;
		
			for (int i = 0; i < points.Length - 1; i++) {
				if (points [i].x < xMin) {
					xMin = points [i].x;
				} else if (points [i].x > xMax) {
					xMax = points [i].x;
				}
			
				if (points [i].y < yMin) {
					yMin = points [i].y;
				} else if (points [i].y > yMax) {
					yMax = points [i].y;
				}
			}
			return new Rect (xMin - buffer.xMin, yMin - buffer.yMin, xMax - xMin + buffer.xMax, yMax - yMin + buffer.yMax);
		}

		public static bool IsInternetConnection ()
		{
			bool isConnectedToInternet = false;
			if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
			   Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
				isConnectedToInternet = true;
			}
			return isConnectedToInternet;
		}
	}

}