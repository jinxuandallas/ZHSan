using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FontStashSharp;
using Microsoft.Xna.Framework.Graphics;
using Tools;
using GameManager;
using Platforms;
namespace GamePanels.Scrollbar
{
    class TextureContent : IFrameContent
    {
        public Vector2 OffsetPos { get; set; }
        public float Scale { get; set; }
        public float Depth { get; set; }
        public List<Bounds> bounds { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Frame baseFrame { get; set; }
        public Texture2D Texture;
        public string TexturePath;
        public float Alpha { get; set; }
        public Rectangle? source;
        public Color color;
        public float Rotation;
        public Vector2 Origin;
        public SpriteEffects spriteEffects;
        public void DrawToCanvas(SpriteBatch batch)
        {
            batch.Draw(Texture, OffsetPos, source, color, Rotation, Origin, Scale, spriteEffects, Depth);
        }

        public void CalculateControlSize()
        {
            Width = Texture.Width * Scale;
            Height = Texture.Height * Scale;
            bounds = new List<Bounds>();
            bounds.Add(new Bounds() { X = OffsetPos.X, Y = OffsetPos.Y, X2 = OffsetPos.X + Width, Y2 = OffsetPos.Y + Height });
        }
        public TextureContent(Vector2 offsetPos, string texturePath, Frame baseframe, float scale = 1f, float depth = 0)
        {
            OffsetPos = offsetPos;
            TexturePath = texturePath;
            baseFrame = baseframe;
            Scale = scale;
            Depth = depth;
            source = null;
            color = Color.White;
            Rotation = 0f;
            Origin = Vector2.Zero;
            spriteEffects = SpriteEffects.None;
            if (TexturePath != null)
                Texture = Platform.Current.LoadTexture(TexturePath);
        }
    }
}
