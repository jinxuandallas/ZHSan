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
        //Texture2D Texture { get; set; }
        float Alpha { get; set; }
        void DrawToCanvas(SpriteBatch batch);
        void CalculateControlSize();
    }

    public class Frame
    {
        //public Vector2 Position;
        public float CanvasWidth, CanvasHeight;
        public Texture2D BackgroundPic=null;
        public List<IFrameContent> ContentContorl;
        protected Texture2D Canvas;
        public Rectangle? VisualFrame;
        public Color BackgroundColor=Color.White;
        /// <summary>
        /// 透明度
        /// </summary>
        public float BackgroundAlpha;

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
            if (ContentContorl.Count < 1)
                return;
            CalculateCanvasSize();
            SpriteBatch batch = new SpriteBatch(Platform.GraphicsDevice);
            RenderTarget2D renderTarget2D = new RenderTarget2D(Platform.GraphicsDevice, CanvasWidth.ConvertToIntPlus(), CanvasHeight.ConvertToIntPlus());
            Platform.GraphicsDevice.SetRenderTarget(renderTarget2D);

            if (BackgroundPic == null)
                Platform.GraphicsDevice.Clear(BackgroundColor);
            else
                batch.Draw(BackgroundPic, new Vector2(0, 0), Color.White * BackgroundAlpha);

            batch.Begin();
            DrawBorder();
            ContentContorl.ForEach(cc => cc.DrawToCanvas(batch));
            batch.End();

            Platform.GraphicsDevice.SetRenderTarget(null);
            Canvas = renderTarget2D;

            GameManager.Session.Current.SpriteBatch.Draw(Canvas, new Vector2(50, 50), Color.White);
        }
        private void CalculateCanvasSize()
        {
            CanvasWidth = CanvasHeight = 0;
            ContentContorl.ForEach(cc =>
            {
                cc.CalculateControlSize();
                CanvasWidth = cc.OffsetPos.X+cc.Width > CanvasWidth ? cc.OffsetPos.X + cc.Width : CanvasWidth;
                CanvasHeight = cc.OffsetPos.Y+cc.Height > CanvasHeight ? cc.OffsetPos.Y + cc.Height : CanvasHeight;
            });
        }
        public void AddContentContorl(IFrameContent contentContorl)
        {
            ContentContorl.Add(contentContorl);
        }
        private void DrawBorder()
        {

        }
        
    }


}
