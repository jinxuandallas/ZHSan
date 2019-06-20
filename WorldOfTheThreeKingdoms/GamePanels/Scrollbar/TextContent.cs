using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FontStashSharp;
namespace WorldOfTheThreeKingdoms.GamePanels.Scrollbar
{
    class TextContent:IContent
    {
        public Vector2 OffsetPos { get; set; }
        public float Scale { get; set; }
        public float Depth { get; set; }
        public Bounds[] bounds { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Frame baseFrame { get; set; }
        public Texture2D Texture { get; set; }
        public void DrawtoTexture()
        {
        }
    }
}
