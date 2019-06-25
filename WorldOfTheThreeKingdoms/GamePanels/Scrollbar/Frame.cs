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
        public Vector2 Position;
        public float CanvasWidth, CanvasHeight;
        public Texture2D BackgroundPic = null;
        public List<IFrameContent> ContentContorl;
        protected Texture2D Canvas;
        public Rectangle? VisualFrame;
        public Color BackgroundColor = new Color(0, 0, 0, 0);
        /// <summary>
        /// 透明度
        /// </summary>
        public float BackgroundAlpha;
        public float Scale;
        public Color color;
        public float Rotation;
        public Vector2 Origin;
        public SpriteEffects spriteEffects;
        public float Depth;
        private SpriteBatch batch;
        private RenderTarget2D renderTarget2D;
        public Frame(Vector2 pos, Rectangle visualFrame, string bgPicPath = null)
        {
            ContentContorl = new List<IFrameContent>();
            Canvas = null;
            Position = pos;
            VisualFrame = visualFrame;
            if (bgPicPath != null)
                BackgroundPic = Platform.Current.LoadTexture(bgPicPath);

            color = Color.White;
            Rotation = 0f;
            Scale = 1f;
            Origin = Vector2.Zero;
            Depth = 0f;
            spriteEffects = SpriteEffects.None;
            CanvasWidth = CanvasHeight = 0;

            batch = new SpriteBatch(Platform.GraphicsDevice);
        }

        public void Draw()
        {
            if (ContentContorl.Count < 1)
                return;
            //RenderTarget2D renderTarget2D = new RenderTarget2D(Platform.GraphicsDevice, CanvasWidth.ConvertToIntPlus(), CanvasHeight.ConvertToIntPlus());


            Platform.GraphicsDevice.SetRenderTarget(renderTarget2D);

            batch.Begin();

            if (BackgroundPic == null)
                Platform.GraphicsDevice.Clear(BackgroundColor);//背景填充颜色
            else
                batch.Draw(BackgroundPic, new Vector2(0, 0), Color.White * BackgroundAlpha);//用图片填充背景

            ContentContorl.ForEach(cc => cc.DrawToCanvas(batch));
            batch.End();

            Platform.GraphicsDevice.SetRenderTarget(null);

            Canvas = renderTarget2D;

            GameManager.Session.Current.SpriteBatch.Draw(Canvas, Position, VisualFrame, color, Rotation, Origin, Scale, spriteEffects, Depth);
            //GameManager.Session.Current.SpriteBatch.Draw(Canvas, Position, color);
            //GameManager.Session.Current.SpriteBatch.Draw(Canvas, new Vector2(50, 50), Color.White);
        }
        private void CalculateCanvasSize(IFrameContent contentContorl)
        {
            contentContorl.CalculateControlSize();
            CanvasWidth = contentContorl.OffsetPos.X + contentContorl.Width > CanvasWidth ? contentContorl.OffsetPos.X + contentContorl.Width : CanvasWidth;
            CanvasHeight = contentContorl.OffsetPos.Y + contentContorl.Height > CanvasHeight ? contentContorl.OffsetPos.Y + contentContorl.Height : CanvasHeight;
            if (renderTarget2D !=null)
                renderTarget2D.Dispose();
            renderTarget2D = new RenderTarget2D(Platform.GraphicsDevice, CanvasWidth.ConvertToIntPlus(), CanvasHeight.ConvertToIntPlus());
            //ContentContorl.ForEach(cc =>
            //{
            //    cc.CalculateControlSize();
            //    CanvasWidth = cc.OffsetPos.X + cc.Width > CanvasWidth ? cc.OffsetPos.X + cc.Width : CanvasWidth;
            //    CanvasHeight = cc.OffsetPos.Y + cc.Height > CanvasHeight ? cc.OffsetPos.Y + cc.Height : CanvasHeight;
            //});
        }
        public void AddContentContorl(IFrameContent contentContorl)
        {
            ContentContorl.Add(contentContorl);
            CalculateCanvasSize(contentContorl);
        }

    }


}
