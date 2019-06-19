using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WorldOfTheThreeKingdoms.GamePanels.Scrollbar
{
    public interface IContorlinFrame
    {
        Vector2 Position { get; set; }
        float Scale { get; set; }
        float Depth { get; set; }
    }

    class Frame
    {
        public Vector2 Position;
        public int Width, Height;
        public Texture2D BackgroundPic;
        public List<IContorlinFrame> Contorl;
        protected Texture2D WholeCanvas;
        public Rectangle? View;

        public Frame(Vector2 pos,string bgPicPath)
        {
            Contorl = new List<IContorlinFrame>();
            WholeCanvas = null;
        }
    }


}
