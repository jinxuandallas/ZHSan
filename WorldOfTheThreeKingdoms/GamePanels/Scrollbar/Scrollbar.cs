using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platforms;
using GameManager;

namespace GamePanels.Scrollbar
{
    public enum ScrollbarType
    {
        Horizontal,
        Vertical,
    }
    public class Scrollbar
    {
        public Frame baseFrame;
        public float DisValue
        {
            get
            {
                return _disValue;
            }
            set
            {
                if (value < 0)
                    _disValue = 0;
                else if (value > (scrollbarType == ScrollbarType.Horizontal ? BarTexture.Width - ButtonTexture.Width : BarTexture.Height - ButtonTexture.Height))
                    _disValue = scrollbarType == ScrollbarType.Horizontal ? BarTexture.Width - ButtonTexture.Width : BarTexture.Height - ButtonTexture.Height;
                else
                    _disValue = value;

            }
        }
        public float Value
        {
            get
            {
                _value = DisValue / (scrollbarType == ScrollbarType.Horizontal ? BarTexture.Width - ButtonTexture.Width : BarTexture.Height - ButtonTexture.Height);
                if (_value < 0)
                    _value = 0;
                if (_value > 1)
                    _value = 1;
                return _value;
            }

            set
            {
                if (value < 0)
                    _value = 0;
                else if (value > 1)
                    _value = 1;
                else
                    _value = value;
            }
        }
        private float _value;
        private float _disValue;
        public Rectangle ScrollButton;
        public Color ButtonColor = Color.DeepSkyBlue;
        public Color BarColor = Color.DimGray;
        public ScrollbarType scrollbarType;
        protected Texture2D ButtonTexture;
        protected Texture2D BarTexture;
        public bool Visible = true;
        public bool Enable = true;
        protected bool MouseOver = false;
        protected bool MouseOverButton = false;
        protected Vector2 BarPos, ButtonPos;
        protected DateTime? prePressTime;
        public int RollingDistance;
        //protected bool prePress;
        //protected int prePoX = -1;
        //protected int prePoY = -1;
        protected bool preDown = false;
        public Scrollbar(Frame frame, Rectangle scrollButton, Color buttonColor, Color barColor, ScrollbarType type = ScrollbarType.Vertical, int rollingDistance = 20)
        {
            baseFrame = frame;

            scrollbarType = type;
            ScrollButton = scrollButton;
            ButtonColor = buttonColor;
            BarColor = barColor;
            BarPos = new Vector2();
            ButtonPos = new Vector2();
            RollingDistance = rollingDistance;
            //if (type == ScrollbarType.Vertical)
            //    BarColor = Color.Black;
            ButtonTexture = CreateScollbarTexture(ScrollButton.Width, ScrollButton.Height, ButtonColor);
            switch (scrollbarType)
            {
                case ScrollbarType.Horizontal:
                    baseFrame.VisualFrame.Height -= ScrollButton.Height;
                    BarTexture = CreateScollbarTexture(baseFrame.VisualFrame.Width, ScrollButton.Height, BarColor);
                    break;
                case ScrollbarType.Vertical:
                    baseFrame.VisualFrame.Width -= ScrollButton.Width;
                    BarTexture = CreateScollbarTexture(ScrollButton.Width, baseFrame.VisualFrame.Height, BarColor);
                    break;

            }

            DisValue = 0f;//此处必须放在BarTexture生成之后
        }

        public Scrollbar(Frame frame, ScrollbarType type = ScrollbarType.Vertical) : this(frame, type == ScrollbarType.Vertical ? new Rectangle(0, 0, 13, 25) : new Rectangle(0, 0, 25, 13), Color.DeepSkyBlue, Color.DimGray, type)
        {
        }

        protected Texture2D CreateScollbarTexture(int width, int height, Color color)
        {
            Texture2D tex = new Texture2D(Platform.GraphicsDevice, width, height);
            Color[] texColor = Enumerable.Repeat(color, width * height).ToArray();
            tex.SetData(texColor);
            return tex;
        }

        public void Draw()
        {
            //Vector2 buttonPos = new Vector2(), barPos = new Vector2();

            switch (scrollbarType)
            {
                case ScrollbarType.Horizontal:
                    BarPos.X = baseFrame.Position.X;
                    BarPos.Y = baseFrame.Position.Y + baseFrame.VisualFrame.Height;
                    ButtonPos = BarPos + new Vector2(DisValue, 0);
                    break;
                case ScrollbarType.Vertical:
                    BarPos.X = baseFrame.Position.X + baseFrame.VisualFrame.Width;
                    BarPos.Y = baseFrame.Position.Y;
                    ButtonPos = BarPos + new Vector2(0, DisValue);
                    break;
            }

            Session.Current.SpriteBatch.Draw(BarTexture, BarPos, Color.White);
            Session.Current.SpriteBatch.Draw(ButtonTexture, ButtonPos, Color.White);
        }
        //public void HandleBothScrollbar(Scrollbar anotherScrollbar)
        //{
        //    if (scrollbarType == ScrollbarType.Horizontal)
        //        BarTexture = CreateScollbarTexture(BarTexture.Width - anotherScrollbar.BarTexture.Width, BarTexture.Height, BarColor);
        //    else
        //        BarTexture = CreateScollbarTexture(BarTexture.Width, BarTexture.Height, BarColor);
        //}

        public bool IsInTexture(float poX, float poY, Vector2? basePos)
        {
            if (Visible && Enable)
            {
                //PreMouseOver = MouseOver;
                if (basePos == null)
                {
                    MouseOver = BarPos.X <= poX && poX <= BarPos.X + BarTexture.Width
                        && BarPos.Y <= poY && poY <= BarPos.Y + BarTexture.Height;
                }
                else
                {
                    MouseOver = this.BarPos.X + ((Vector2)basePos).X <= poX && poX <= this.BarPos.X + ((Vector2)basePos).X + BarTexture.Width
                        && this.BarPos.Y + ((Vector2)basePos).Y <= poY && poY <= this.BarPos.Y + ((Vector2)basePos).Y + BarTexture.Height;
                }
            }
            else
            {
                MouseOver = false;
            }

            if (MouseOver)
            {
                if (basePos == null)
                {
                    MouseOverButton = ButtonPos.X <= poX && poX <= ButtonPos.X + ButtonTexture.Width
                        && ButtonPos.Y <= poY && poY <= ButtonPos.Y + ButtonTexture.Height;
                }
                else
                {
                    MouseOverButton = ButtonPos.X + ((Vector2)basePos).X <= poX && poX <= ButtonPos.X + ((Vector2)basePos).X + ButtonTexture.Width
                        && ButtonPos.Y + ((Vector2)basePos).Y <= poY && poY <= ButtonPos.Y + ((Vector2)basePos).Y + ButtonTexture.Height;
                }
            }
            return MouseOver;
        }

        public void Update()
        {
            Update(null);
        }
        public void Update(Vector2? basePos)
        {
            Update(InputManager.PoX, InputManager.PoY, basePos);
        }
        public void Update(int poX, int poY, Vector2? basePos)
        {
            Update(poX, poY, basePos, InputManager.IsPressed, InputManager.IsDown, false);
        }
        //SoundPlayer player = new SoundPlayer("Content/Textures/Resources/Start/Select");
        public void Update(int poX, int poY, Vector2? basePos, bool press, bool down, bool sound)
        {
            MouseOver = Enable && IsInTexture(Convert.ToSingle(poX), Convert.ToSingle(poY), basePos) && (poX != 0 || poY != 0);

            if (scrollbarType == ScrollbarType.Vertical && InputManager.PinchMove != 0 && IsInFrame(poX, poY))
            {
                DisValue -= InputManager.PinchMove * RollingDistance;
                InputManager.PinchMove = 0;
                return;

                //Platform.Current.PlayEffect(@"Content\Sound\Select");
            }
            if (InputManager.IsReleased)//IsReleased一定要放在preDown判定之前，否则永远不会访问到
            {
                preDown = false;
                return;
            }
            if (preDown)
            {
                PressBarButton();
                return;
            }


            if (MouseOver)
                if (MouseOverButton && down && InputManager.IsMoved)
                    PressBarButton();
                //Platform.Current.PlayEffect(@"Content\Sound\Select");
                else if (!MouseOverButton && press)
                    //单击滚动条空白处
                    PressBarBlank(poX, poY);


            //if (OnMouseOver != null) OnMouseOver.Invoke(null, null);
            //if (press)
            //{
            //    PressButton();
            //    press = false;
            //}

        }

        protected bool IsInFrame(int poX, int poY)
        {
            bool inFrame = false;

            if (Visible && Enable)
            {
                Scrollbar scrollbar;
                scrollbar = baseFrame.Scrollbars.Where(sb => sb.scrollbarType == ScrollbarType.Horizontal).FirstOrDefault();
                int horizontalButtonHeight = scrollbar == null ? 0 : scrollbar.ButtonTexture.Height;

                scrollbar = baseFrame.Scrollbars.Where(sb => sb.scrollbarType == ScrollbarType.Vertical).FirstOrDefault();
                int verticalButtonWidth = scrollbar == null ? 0 : scrollbar.ButtonTexture.Width;

                inFrame = baseFrame.Position.X <= poX && poX <= baseFrame.Position.X + baseFrame.VisualFrame.Width + verticalButtonWidth
                        && baseFrame.Position.Y <= poY && poY <= baseFrame.Position.Y + baseFrame.VisualFrame.Height + horizontalButtonHeight;
            }
            return inFrame;
        }

        protected void PressBarButton()
        {
            //InputManager.ClickTime++;
            //DateTime now = DateTime.Now;
            //if (prePressTime != null)
            //{
            //    TimeSpan ts = now - (DateTime)prePressTime;
            //    if (ts.TotalSeconds < 0.3f || !Platform.IsActive)
            //    {
            //        return;
            //    }
            //}
            //prePressTime = now;

            //if (!prePress)
            //    return;
            //Value++;
            //if (!moved)
            //    return;


            //if (prePoX == -1)
            //    prePoX = poX;
            //if (prePoY == -1)
            //    prePoY = poY;

            if (scrollbarType == ScrollbarType.Horizontal)
                DisValue += InputManager.PoXMove;
            //else

            if (scrollbarType == ScrollbarType.Vertical)
                DisValue += InputManager.PoYMove;
            preDown = true;
            //prePoX = poX;
            //prePoY = poY;
        }

        protected void PressBarBlank(int poX, int poY)
        {
            if (scrollbarType == ScrollbarType.Horizontal)
            {
                if (poX < ButtonPos.X)
                    DisValue -= BarTexture.Width * 0.15f;
                if (poX > ButtonPos.X + ButtonTexture.Width)
                    DisValue += BarTexture.Width * 0.15f;
            }

            if (scrollbarType == ScrollbarType.Vertical)
            {
                if (poY < ButtonPos.Y)
                    DisValue -= BarTexture.Height * 0.15f;
                if (poY > ButtonPos.Y + ButtonTexture.Height)
                    DisValue += BarTexture.Height * 0.15f;
                //DisValue += ((float)baseFrame.VisualFrame.Height / baseFrame.CanvasHeight) * BarTexture.Height;
            }
        }
    }
}
