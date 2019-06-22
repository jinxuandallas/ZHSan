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
        public Texture2D Texture { get; set; }
        public string TexturePath;
        public void DrawTexture()
        {
            if (TexturePath != null)
                Texture = Platform.Current.LoadTexture(TexturePath);
            //Session.Current.SpriteBatch.Draw(Texture, new Vector2(200, 200), Color.Yellow);
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
        }
    }
}
