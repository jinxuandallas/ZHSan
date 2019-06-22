﻿using System;
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
    class TextContent:IFrameContent
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
        public void DrawTexture()
        {
            Texture = CacheManager.DrawStringToTexture(Session.Current.Font, Text, Color.Red, 0f);
            Session.Current.SpriteBatch.Draw(Texture, new Vector2(200, 200), Color.Yellow);
            Width = Texture.Width*Scale;
            Height = Texture.Height*Scale;
            bounds = new List<Bounds>();
            bounds.Add(new Bounds() {X=OffsetPos.X,Y=OffsetPos.Y,X2= OffsetPos.X+Width,Y2=OffsetPos.Y+Height });
        }

        public TextContent(Vector2 offsetPos,string text, Frame baseframe,float scale=1f,float depth=0)
        {
            OffsetPos = offsetPos;
            Text = text;
            baseFrame = baseframe;
            Scale = scale;
            Depth = depth;
        }
    }
}
