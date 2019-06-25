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
        public float Value;
        public Rectangle ScrollButton;
        public Color ButtonColor =Color.DeepSkyBlue;
        public Color BarColor = Color.DimGray;
        public ScrollbarType scrollbarType;
        protected Texture2D ButtonTexture;
        protected Texture2D BarTexture;
        public Scrollbar(Frame frame, Rectangle scrollButton, Color buttonColor, Color barColor, ScrollbarType type = ScrollbarType.Vertical)
        {
            baseFrame = frame;
            Value = 0f;
            scrollbarType = type;
            ScrollButton = scrollButton;
            ButtonColor = buttonColor;
            BarColor = barColor;

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
        }

        public Scrollbar(Frame frame, ScrollbarType type = ScrollbarType.Vertical) : this(frame, type == ScrollbarType.Vertical ? new Rectangle(0, 0, 10, 20) : new Rectangle(0, 0, 20, 10), Color.DeepSkyBlue, Color.DimGray, type)
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
            Vector2 buttonPos = new Vector2(), barPos = new Vector2();

            switch (scrollbarType)
            {
                case ScrollbarType.Horizontal:
                    barPos.X = baseFrame.Position.X;
                    barPos.Y = baseFrame.Position.Y + baseFrame.VisualFrame.Height;
                    buttonPos = barPos + new Vector2(Value, 0);
                    break;
                case ScrollbarType.Vertical:
                    barPos.X = baseFrame.Position.X + baseFrame.VisualFrame.Width;
                    barPos.Y = baseFrame.Position.Y;
                    buttonPos = barPos + new Vector2(0, Value);
                    break;
            }

            Session.Current.SpriteBatch.Draw(BarTexture, barPos, Color.White);
            Session.Current.SpriteBatch.Draw(ButtonTexture, buttonPos, Color.White);
        }
    }
}
