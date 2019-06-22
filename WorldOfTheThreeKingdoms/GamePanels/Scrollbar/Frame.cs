using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FontStashSharp;
using Tools;
using Platforms;
namespace GamePanels.Scrollbar
{
    public interface IFrameContent
    {
        /// <summary>
        /// 在框架内的偏移量
        /// </summary>
        Vector2 OffsetPos { get; set; }
        float Scale { get; set; }
        float Depth { get; set; }
        List<Bounds> bounds { get; set; }
        float Width { get; set; }
        float Height { get; set; }
        Frame baseFrame { get; set; }
        Texture2D Texture { get; set; }
        void DrawTexture();
    }

    public class Frame
    {
        //public Vector2 Position;
        public float CanvasWidth, CanvasHeight;
        public Texture2D BackgroundPic;
        public List<IFrameContent> ContentContorl;
        protected Texture2D Canvas;
        public Rectangle? VisualFrame;
        public Color BackgroundColor=Color.LightSkyBlue;

        public Frame(Rectangle visualFrame, string bgPicPath)
        {
            ContentContorl = new List<IFrameContent>();
            Canvas = null;
            VisualFrame = visualFrame;
            if (bgPicPath != null)
                BackgroundPic= Platform.Current.LoadTexture(bgPicPath);
        }

        public void Draw()
        {
            ContentContorl.ForEach(cc => cc.DrawTexture());
            CalculateCanvasSize();
            Canvas = new Texture2D(Platform.GraphicsDevice, CanvasWidth.ConvertToIntPlus(), CanvasHeight.ConvertToIntPlus());
        }
        private void CalculateCanvasSize()
        {
            CanvasWidth = CanvasHeight = 0;
            ContentContorl.ForEach(cc => cc.bounds.ForEach(b =>
            {
                CanvasWidth = b.X2 > CanvasWidth ? b.X2 : CanvasWidth;
                CanvasHeight = b.Y2 > CanvasHeight ? b.Y2 : CanvasHeight;
            }));
        }
        public void AddContentContorl(IFrameContent contentContorl)
        {
            ContentContorl.Add(contentContorl);
        }
        private void DrawFrame()
        {

        }
        
        private void DrawBackground()
        {

        }
    }


}
