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
    public interface IContent
    {
        /// <summary>
        /// 在框架内的偏移量
        /// </summary>
        Vector2 OffsetPos { get; set; }
        float Scale { get; set; }
        float Depth { get; set; }
        Bounds[] bounds { get; set; }
        float Width { get; set; }
        float Height { get; set; }
        Frame baseFrame { get; set; } 
    }

    public class Frame
    {
        public Vector2 Position;
        public float Width, Height;
        public Texture2D BackgroundPic;
        public List<IContent> Contorl;
        protected Texture2D Canvas;
        public Rectangle? View;
        public Color BackgroundColor=Color.LightSkyBlue;

        public Frame(Vector2 pos,string bgPicPath)
        {
            Contorl = new List<IContent>();
            Canvas = null;
        }
    }


}
