using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FontStashSharp;
using GameManager;
namespace GamePanels.Scrollbar
{
    class TextContent : IFrameContent
    {
        public Vector2 OffsetPos { get; set; }
        public float Scale { get; set; }
        public float Depth { get; set; }
        public List<Bounds> bounds { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Frame baseFrame { get; set; }
        public Texture2D Texture { get; set; }
        public string Text;
        public Color TextColor;
        public float Alpha { get; set; }
        public void DrawToCanvas(SpriteBatch batch)
        {
            CacheManager.DrawStringReturnBounds(batch, Session.Current.Font, Text, OffsetPos, TextColor * Alpha, 0f, Vector2.Zero, Scale, SpriteEffects.None, Depth);

        }
        public void CalculateControlSize()
        {
            bounds = CacheManager.CalculateTextBounds(Session.Current.Font, Text, OffsetPos, Scale);
            Width = 0;
            bounds.ForEach(b => Width = Width > b.Width ? Width : b.Width);
            Height = bounds[bounds.Count - 1].Y2 - bounds[0].Y;
        }

        public TextContent(Vector2 offsetPos, string text, Frame baseframe, Color? textColor, float scale = 1f, float depth = 0)
        {
            OffsetPos = offsetPos;
            Text = text;
            baseFrame = baseframe;
            Scale = scale;
            Depth = depth;
            Alpha = 1f;
            TextColor = textColor ?? Color.White;
        }
    }
}
