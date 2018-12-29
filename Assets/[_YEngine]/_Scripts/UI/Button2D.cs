using UnityEngine;
using System.Collections.Generic;
using Yudiz.Gaming.Engine;
using System.Net.Configuration;

namespace Yudiz.Gaming.Engine
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Button2D : MonoBehaviour
    {
        private bool isDisabled = false;
        private bool isSelected = false;
        public List<string> Layer = new List<string>() { "Base" };
        public Rect TouchBuffer = new Rect(0, 0, 0, 0);
        public List<ButtonEvent> DownEvent;
        public List<ButtonEvent> PressEvent;
        public List<ButtonEvent> UpEvent;
        public List<ButtonEvent> ClickCancelEvent;
        public List<ButtonEvent> ClickEvent;
        public List<ButtonEvent> DisableClick;
        public List<ButtonEvent> DoubleClick;
        public List<ButtonEvent> GotFocusEvent;
        public List<ButtonEvent> LostFocusEvent;

        public bool IsDisabled
        {
            get
            {
                return isDisabled;
            }
            set
            {
                isDisabled = value;
                SetDisable();
            }
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                CheckIsSelcted();
            }
        }

        public ButtonEffects2D RegularEffect;
        public ButtonEffects2D ClickEffect;
        public ButtonEffects2D HoverEffect;
        public ButtonEffects2D DisableEffect = new ButtonEffects2D(true);
        public ButtonEffects2D SelectedEffect;
        public ButtonEffects2D SelectedHoverEffect;
        //public ButtonEffects OnDownEffect;
        private SpriteRenderer spriteRendarer;
        private Camera camMain;
        private Rect rectButton;
        private bool isMouseIn = false;
        private bool isClickStarts = false;
        private float clickTime;
        private float lastClickTime;
        // Use this for initialization
        void Awake()
        {
            camMain = GameObject.FindWithTag(Tags.GUI_CAM).GetComponent<Camera>();
        }

        void Start()
        {
            //InvokeRepeating("DrawRect", 0, 0.20f);
            DrawRect();
        }

        float delay = 0.20f;

        // Update is called once per frame
        void Update()
        {
            if (!Layer.Contains(GameEngine.CurrentLayer))
            {
                return;
            }

            if (delay > 0)
            {
                delay -= 0.015f;
            }
            else
            {
                DrawRect();
                delay = 0.20f;
            }

            #region "Mouse / touch enter Envents for Hover in effect"
            if (rectButton.Contains(EngineInput.MousePosition) && !isMouseIn)
            {
                if (!isDisabled)
                {
                    this.ExecuteEvents(GotFocusEvent);
                    if (!isSelected)
                    {
                        this.ChangeButtonUI(HoverEffect);
                    }
                    else
                    {
                        this.ChangeButtonUI(SelectedHoverEffect);
                    }
                }
                isMouseIn = true;
            }
            #endregion

            #region "Mouse / touch down envents for click start"
            if (rectButton.Contains(EngineInput.TouchDown))
            {
                if (!isDisabled)
                {
                    if (!isClickStarts)
                    {
                        this.ExecuteEvents(DownEvent);
                        this.ChangeButtonUI(ClickEffect);
                    }
                }
                if (!isClickStarts)
                {
                    isClickStarts = true;
                }
            }
            #endregion

            #region  "Mouse / touch continue pressed envents for click continue"
            if (rectButton.Contains(EngineInput.TouchPressed))
            {
                if (!isDisabled)
                {
                    this.ExecuteEvents(PressEvent);
                }
            }
            #endregion

            #region "Mouse / touch up and click envents"
            if (rectButton.Contains(EngineInput.TouchUp))
            {
                if (!isDisabled)
                {
                    this.ExecuteEvents(UpEvent);
                }

                #region "Click and Double Clikc Event Settings"
                if (isClickStarts)
                {
                    clickTime = Time.time;
                    if (!isDisabled)
                    {
                        this.ExecuteEvents(ClickEvent);
                    }
                    else
                    {
                        this.ExecuteEvents(DisableClick);
                    }

                    #region "Double Click Time Setting"
                    if (!isDisabled)
                    {
                        if (clickTime - lastClickTime <= GameEngine.DOUBLE_CLICK_THRESH_HOLD)
                        {
                            this.ExecuteEvents(DoubleClick);
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
            if (!rectButton.Contains(EngineInput.MousePosition))
            {
                #region "condition for Mouse Hover end"
                if (!isDisabled)
                {
                    if (isMouseIn)
                    {
                        if (!isSelected)
                        {
                            this.ChangeButtonUI(RegularEffect);
                        }
                        else
                        {
                            this.ChangeButtonUI(SelectedEffect);
                        }

                        isMouseIn = false;
                        this.ExecuteEvents(LostFocusEvent);
                        CheckIsSelcted();
                    }
                }
                #endregion

                #region "condition for click cancel"
                if (isClickStarts)
                {
                    isClickStarts = false;
                    if (!isDisabled)
                    {
                        this.ExecuteEvents(ClickCancelEvent);
                    }
                }
                #endregion
            }
            #endregion
        }

        void SetDisable()
        {
            if (isDisabled)
            {
                this.ChangeButtonUI(DisableEffect);
            }
            else
            {
                this.ChangeButtonUI(RegularEffect);
            }
        }

        void CheckIsSelcted()
        {
            if (isDisabled)
            {
                return;
            }
            if (isSelected)
            {
                this.ChangeButtonUI(SelectedEffect);
            }
        }

        void DrawRect()
        {
            rectButton = GameEngine.GetBoundingRect(transform, camMain, TouchBuffer);
        }

        void ExecuteEvents(List<ButtonEvent> events)
        {
            foreach (ButtonEvent b in events)
            {
                b.EventData.Container = gameObject;
                b.EventData.Sender = this;
                NotificationCentre.PostNotification(new Notification(this, b.Function, b.EventData));
            }
        }

        void ChangeButtonUI(ButtonEffects2D effect)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", effect.Color);
            if (effect.sprite != null)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = effect.sprite;
            }

            if (effect.isScale)
            {
                //iTween.ScaleTo (this.gameObject, iTween.Hash ("scale", (Vector3)effect.scale, "time", effect.speed, "easetype", iTween.EaseType.easeOutElastic, "ignoretimescale", true));
            }

            foreach (ButtonChildEffect2D bce in effect.ChildObjects)
            {
                if (bce.Child != null && bce.Child.GetComponent<Renderer>() != null)
                {
                    SpriteRenderer rend = bce.Child.GetComponent<SpriteRenderer>();
                    rend.color = bce.Color;
                    if (bce.sprite != null)
                    {
                        rend.sprite = bce.sprite;
                    }
                    if (effect.isScale)
                    {
                        //iTween.ScaleTo (this.gameObject, iTween.Hash ("scale", (Vector3)effect.scale, "time", effect.speed, "easetype", iTween.EaseType.easeOutElastic, "ignoretimescale", true));
                    }

                }
            }
        }

        void OnBecameVisible()
        {
            DrawRect();
        }

        void OnBecameInvisible()
        {
            rectButton = new Rect(int.MinValue, int.MinValue, int.MinValue, int.MinValue);
        }
        //#if UNITY_EDITOR
        //        void OnGUI()
        //        {
        //            GUI.Box(rectButton, gameObject.name);
        //        }
        //#endif
    }


    [System.Serializable]
    public class ButtonEffects2D
    {
        //public bool IsEffect;
        private bool isDisable;
        //		public SpriteRenderer spriteRend;
        public Sprite sprite;
        public Color Color;
        public List<ButtonChildEffect2D> ChildObjects;
        public bool isScale;
        public Vector2 scale;
        public float speed;

        public ButtonEffects2D() : this(false)
        {

        }

        public ButtonEffects2D(bool disable)
        {
            isDisable = disable;
            if (!isDisable)
            {
                Color = new Color(1, 1, 1, 1);
            }
            else
            {
                Color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }

        public ButtonEffects2D(Color mainColor)
        {
            Color = mainColor;
        }
    }

    [System.Serializable]
    public class ButtonChildEffect2D
    {
        private bool isDisable;
        //		public SpriteRenderer spriteRend;
        public Sprite sprite;
        public Color Color = new Color(1f, 1f, 1f, 1f);
        public bool isScale;
        public Vector2 scale;
        public GameObject Child;

        public ButtonChildEffect2D() : this(false)
        {

        }

        public ButtonChildEffect2D(bool disable)
        {
            isDisable = disable;
            if (!isDisable)
            {
                Color = new Color(1, 1, 1, 1);
            }
            else
            {
                Color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }

        public ButtonChildEffect2D(Color mainColor)
        {
            Color = mainColor;
        }
    }
}