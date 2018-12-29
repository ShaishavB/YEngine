using UnityEngine;
using System.Collections.Generic;
using Yudiz.Gaming.Engine;
using System.Net.Configuration;

//public class Button: _Button {
//}
namespace Yudiz.Gaming.Engine
{
	[RequireComponent (typeof(MeshRenderer))]
	public  class Button3D  : MonoBehaviour
	{
		private bool isDisabled = false;
		private bool isSelected = false;
		public List<string> Layer = new List<string> () { "Base" };
		public Rect TouchBuffer = new Rect (0, 0, 0, 0);
		public List<ButtonEvent> DownEvent;
		public List<ButtonEvent> PressEvent;
		public List<ButtonEvent> UpEvent;
		public List<ButtonEvent> ClickCancelEvent;
		public List<ButtonEvent> ClickEvent;
		public List<ButtonEvent> DisableClick;
		public List<ButtonEvent> DoubleClick;
		public List<ButtonEvent> GotFocusEvent;
		public List<ButtonEvent> LostFocusEvent;

		public bool IsDisabled {
			get { 
				return isDisabled;
			}
			set {
				isDisabled = value;
				SetDisable ();
			}
		}

		public bool IsSelected {
			get {
				return isSelected;
			}
			set {
				isSelected = value;
				CheckIsSelcted ();
			}
		}

		public ButtonEffects3D RegularEffect;
		public ButtonEffects3D ClickEffect;
		public ButtonEffects3D HoverEffect;
		public ButtonEffects3D DisableEffect = new ButtonEffects3D (true);
		public ButtonEffects3D SelectedEffect;
		public ButtonEffects3D SelectedHoverEffect;
		//public ButtonEffects OnDownEffect;
		private TextMesh chkTextMesh;
		private MeshFilter meshFilter;
		private Camera camMain;
		private Rect rectButton;
		private bool isMouseIn = false;
		private bool isClickStarts = false;
		private float clickTime;
		private float lastClickTime;
		// Use this for initialization
		void Awake ()
		{
			camMain = GameObject.FindWithTag (Tags.GUI_CAM).GetComponent<Camera> ();
		}

		void Start ()
		{
			InvokeRepeating ("DrawRect", 0, 0.20f);
			DrawRect ();
		}
		// Update is called once per frame
		void Update ()
		{
			if (!Layer.Contains (GameEngine.CurrentLayer)) {
				return;
			}
			#region "Mouse / touch enter Envents for Hover in effect"
			if (rectButton.Contains (EngineInput.MousePosition) && !isMouseIn) {
				if (!isDisabled) {
					this.ExecuteEvents (GotFocusEvent);
					if (!isSelected) {
						this.ChangeButtonUI (HoverEffect);
					} else {
						this.ChangeButtonUI (SelectedHoverEffect);
					}
				}
				isMouseIn = true;
			}
			#endregion

			#region "Mouse / touch down envents for click start"
			if (rectButton.Contains (EngineInput.TouchDown)) {
				if (!isDisabled) {
					if (!isClickStarts) {
						this.ExecuteEvents (DownEvent);
						this.ChangeButtonUI (ClickEffect);
					}
				}
				if (!isClickStarts) {
					isClickStarts = true;
				}
			}
			#endregion

			#region  "Mouse / touch continue pressed envents for click continue"
			if (rectButton.Contains (EngineInput.TouchPressed)) {
				if (!isDisabled) {
					this.ExecuteEvents (PressEvent);
				}
			}
			#endregion

			#region "Mouse / touch up and click envents"
			if (rectButton.Contains (EngineInput.TouchUp)) {
				if (!isDisabled) {
					this.ExecuteEvents (UpEvent);
				}

				#region "Click and Double Clikc Event Settings"
				if (isClickStarts) {
					clickTime = Time.time;
					if (!isDisabled) {
						this.ExecuteEvents (ClickEvent);
					} else {
						this.ExecuteEvents (DisableClick);
					}

					#region "Double Click Time Setting"
					if (!isDisabled) {
						if (clickTime - lastClickTime <= GameEngine.DOUBLE_CLICK_THRESH_HOLD) {
							this.ExecuteEvents (DoubleClick);
						}
						lastClickTime = clickTime;
					}
					#endregion
				}
				#endregion
				isClickStarts = false;
			}
			#endregion

			#region "Mouse / touch leave envents for Hover end & Click Cancel Setting"
			if (!rectButton.Contains (EngineInput.MousePosition)) {
				#region "condition for Mouse Hover end"
				if (!isDisabled) {
					if (isMouseIn) {
						if (!isSelected) {
							this.ChangeButtonUI (RegularEffect);
						} else {
							this.ChangeButtonUI (SelectedEffect);
						}

						isMouseIn = false;
						this.ExecuteEvents (LostFocusEvent);
						CheckIsSelcted ();
					}
				}
				#endregion

				#region "condition for click cancel"
				if (isClickStarts) {
					isClickStarts = false;
					if (!isDisabled) {
						this.ExecuteEvents (ClickCancelEvent);
					}
				}
				#endregion
			}
			#endregion
		}

		void SetDisable ()
		{
			if (isDisabled) {
				this.ChangeButtonUI (DisableEffect);
			} else {
				this.ChangeButtonUI (RegularEffect);
			} 
		}

		void CheckIsSelcted ()
		{
			if (isDisabled) {
				return;
			}
			if (isSelected) {
				this.ChangeButtonUI (SelectedEffect);
			}
		}

		void DrawRect ()
		{
			rectButton = GameEngine.GetBoundingRect (transform, camMain, TouchBuffer);
		}

		void ExecuteEvents (List<ButtonEvent> events)
		{
			foreach (ButtonEvent b in events) {
				b.EventData.Container = gameObject;
				b.EventData.Sender = this;
				NotificationCentre.PostNotification (new Notification (this, b.Function, b.EventData));
			}
		}

		void ChangeButtonUI (ButtonEffects3D effect)
		{
			chkTextMesh = gameObject.GetComponent<TextMesh> ();
			gameObject.GetComponent<Renderer> ().material.SetColor ("_Color", effect.Color);
			if (effect.Texture != null) {
				gameObject.GetComponent<Renderer> ().material.SetTexture ("_MainTex", effect.Texture);
			}
			if (chkTextMesh == null && effect.Mesh != null) {
				meshFilter = gameObject.GetComponent<MeshFilter> ();
				if (meshFilter != null) {
					meshFilter.mesh = effect.Mesh;
				}
			}

			foreach (ButtonChildEffect3D bce in effect.ChildObjects) {
				if (bce.Child != null && bce.Child.GetComponent<Renderer> () != null) {
					bce.Child.GetComponent<Renderer> ().material.SetColor ("_Color", bce.Color);
					if (bce.Texture != null) {
						bce.Child.GetComponent<Renderer> ().material.SetTexture ("_MainTex", bce.Texture);
					}
					chkTextMesh = bce.Child.GetComponent<TextMesh> ();
					if (chkTextMesh == null && bce.Mesh != null) {
						meshFilter = gameObject.GetComponent<MeshFilter> ();
						if (meshFilter != null) {
							meshFilter.mesh = bce.Mesh;
						}
					}
				}
			}

		}

		void OnBecameVisible ()
		{
			DrawRect ();
		}

		void OnBecameInvisible ()
		{
			rectButton = new Rect (int.MinValue, int.MinValue, int.MinValue, int.MinValue);
		}
		//		#if UNITY_EDITOR
		//		void OnGUI() {
		//			GUI.Box(rectButton, gameObject.name);
		//		}
		//		#endif
	}

	[System.Serializable]
	public class ButtonEvent
	{
		public string Function = string.Empty;
		public bool IsSound = false;
		public int SFXClipNo = -1;
		public ButtonEventArgs EventData;
	}

	[System.Serializable]
	public class ButtonEventArgs
	{
		public string Data;
		public GameObject ExtraGameObject;
		[HideInInspector]
		public Component
			Sender;
		[HideInInspector]
		public GameObject
			Container;

		public ButtonEventArgs () :
			this (null, string.Empty, null)
		{
		}

		public ButtonEventArgs (Component sender, string data, GameObject container)
		{
			this.Sender = sender;
			this.Data = data;
			this.Container = container;
		}
	}

	[System.Serializable]
	public class ButtonEffects3D
	{
		//public bool IsEffect;
		public Mesh Mesh;
		public Texture Texture;
		public Color Color;
		public List<ButtonChildEffect3D> ChildObjects;
		private bool isDisable;

		public ButtonEffects3D () : this (false)
		{
				
		}

		public ButtonEffects3D (bool disable)
		{
			isDisable = disable;
			if (!isDisable) {
				Color = new Color (1, 1, 1, 1);
			} else {
				Color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
			}
		}

		public ButtonEffects3D (Color mainColor)
		{
			Color = mainColor;
		}
	}

	[System.Serializable]
	public class ButtonChildEffect3D
	{
		public Texture Texture;
		public Color Color = new Color (1f, 1f, 1f, 1f);
		public Mesh Mesh;
		public GameObject Child;
		private bool isDisable;

		public ButtonChildEffect3D () : this (false)
		{

		}

		public ButtonChildEffect3D (bool disable)
		{
			isDisable = disable;
			if (!isDisable) {
				Color = new Color (1, 1, 1, 1);
			} else {
				Color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
			}
		}

		public ButtonChildEffect3D (Color mainColor)
		{
			Color = mainColor;
		}
	}
}