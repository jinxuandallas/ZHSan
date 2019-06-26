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
        Vertical
    }
    public class Scrollbar
    {
        public Frame baseFrame;
        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value < 0)
                    _value = 0;
                else if (value > (scrollbarType == ScrollbarType.Horizontal ? BarTexture.Width-ButtonTexture.Width : BarTexture.Height-ButtonTexture.Height))
                    _value = scrollbarType == ScrollbarType.Horizontal ? BarTexture.Width - ButtonTexture.Width : BarTexture.Height - ButtonTexture.Height;
                else
                    _value = value;

            }
        }
        private float _value;
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
        protected bool prePress;
        protected int prePoX = -1;
        protected int prePoY = -1;
        public Scrollbar(Frame frame, Rectangle scrollButton, Color buttonColor, Color barColor, ScrollbarType type = ScrollbarType.Vertical)
        {
            baseFrame = frame;

            scrollbarType = type;
            ScrollButton = scrollButton;
            ButtonColor = buttonColor;
            BarColor = barColor;
            BarPos = new Vector2();
            ButtonPos = new Vector2();

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

            Value = 0f;//此处必须放在BarTexture生成之后
            prePress = false;
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
                    ButtonPos = BarPos + new Vector2(Value, 0);
                    break;
                case ScrollbarType.Vertical:
                    BarPos.X = baseFrame.Position.X + baseFrame.VisualFrame.Width;
                    BarPos.Y = baseFrame.Position.Y;
                    ButtonPos = BarPos + new Vector2(0, Value);
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
            Update(poX, poY, basePos, InputManager.IsPressed, false);
        }
        //SoundPlayer player = new SoundPlayer("Content/Textures/Resources/Start/Select");
        public void Update(int poX, int poY, Vector2? basePos, bool press, bool sound)
        {
            MouseOver = Enable && IsInTexture(Convert.ToSingle(poX), Convert.ToSingle(poY), basePos) && (poX != 0 || poY != 0);
            if (MouseOver)
            {
                if (MouseOverButton)
                {
                    if (press)
                        PressBarButton(poX, poY);
                    
                    prePress = press;
                }
                //Platform.Current.PlayEffect(@"Content\Sound\Select");
                else
                {
                    prePoX = prePoY = -1;
                    //Platform.Current.PlayEffect(@"Content\Sound\Close");

                }

                //if (OnMouseOver != null) OnMouseOver.Invoke(null, null);
                //if (press)
                //{
                //    PressButton();
                //    press = false;
                //}
            }

        }

        protected void PressBarButton(int poX, int poY)
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
            if (prePoX == -1)
                prePoX = poX;
            if (prePoY == -1)
                prePoY = poY;

            if (scrollbarType == ScrollbarType.Horizontal && prePoX != poX)
                Value += (poX - prePoX);

            if (scrollbarType == ScrollbarType.Vertical && prePoY != poY)
                Value += (poY - prePoY);

            prePoX = poX;
            prePoY = poY;
        }
    }
}
