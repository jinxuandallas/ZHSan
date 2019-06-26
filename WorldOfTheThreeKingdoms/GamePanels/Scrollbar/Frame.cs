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
        /// <summary>
        /// 在画布上绘制控件
        /// </summary>
        /// <param name="batch">SpriteBatch</param>
        void DrawToCanvas(SpriteBatch batch);
        /// <summary>
        /// 计算每一个控件的尺寸
        /// </summary>
        void CalculateControlSize();
    }

    public class Frame
    {
        public Vector2 Position;
        public float CanvasWidth, CanvasHeight;
        public Texture2D BackgroundPic = null;
        public List<IFrameContent> ContentContorls;
        protected Texture2D Canvas;
        public Rectangle VisualFrame;
        public Color BackgroundColor = new Color(0, 0, 0, 0);
        /// <summary>
        /// 透明度
        /// </summary>
        public float BackgroundAlpha;
        public Color color;
        public float Depth;
        private SpriteBatch batch;
        private RenderTarget2D renderTarget2D;
        public float Aplha;
        public bool HasHorizontalScrollbar;
        public bool HasVerticalScrollbar;
        protected List<Scrollbar> Scrollbars;
        protected static object batchlock = new object();
        public Frame(Vector2 pos, Rectangle visualFrame, string bgPicPath = null, float alpha = 1f, bool horizontalScrollbar = true, bool verticalScrollbar = true)
        {
            ContentContorls = new List<IFrameContent>();
            Canvas = null;
            Position = pos;
            VisualFrame = visualFrame;
            if (bgPicPath != null)
                BackgroundPic = Platform.Current.LoadTexture(bgPicPath);

            color = Color.White;
            Aplha = alpha;
            Depth = 0f;

            CanvasWidth = CanvasHeight = 0;

            batch = new SpriteBatch(Platform.GraphicsDevice);

            Scrollbars = new List<Scrollbar>();
            HasHorizontalScrollbar = HasVerticalScrollbar = true;

            //Scrollbar HorizontalScrollbar=
            if (HasHorizontalScrollbar)
                Scrollbars.Add(new Scrollbar(this, ScrollbarType.Horizontal));
            if (HasVerticalScrollbar)
                Scrollbars.Add(new Scrollbar(this));

            
            //if(HasHorizontalScrollbar&&HasVerticalScrollbar)
            //{
            //    Scrollbars[0].HandleBothScrollbar(Scrollbars[1]);
            //    Scrollbars[1].HandleBothScrollbar(Scrollbars[0]);
            //}

        }

        public void Draw()
        {
            if (ContentContorls.Count < 1)
                return;
            lock (batchlock)
            {
                Platform.GraphicsDevice.SetRenderTarget(renderTarget2D);

                batch.Begin();

                //if (BackgroundPic == null)
                Platform.GraphicsDevice.Clear(BackgroundColor);//背景填充颜色

                ContentContorls.ForEach(cc => cc.DrawToCanvas(batch));
                batch.End();

                Platform.GraphicsDevice.SetRenderTarget(null);

                Canvas = renderTarget2D;

                if (BackgroundPic != null) //绘制背景图片，未测试
                    GameManager.Session.Current.SpriteBatch.Draw(BackgroundPic, Position, VisualFrame, Color.White * BackgroundAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
                GameManager.Session.Current.SpriteBatch.Draw(Canvas, Position, VisualFrame, color * Aplha, 0f, Vector2.Zero, 1f, SpriteEffects.None, Depth);
            }
            Scrollbars.ForEach(sb => sb.Draw());
            //GameManager.Session.Current.SpriteBatch.Draw(Canvas, Position, color);
            //GameManager.Session.Current.SpriteBatch.Draw(Canvas, new Vector2(50, 50), Color.White);
        }
        private void CalculateCanvasSize(IFrameContent contentContorl)
        {
            contentContorl.CalculateControlSize();
            if (contentContorl.OffsetPos.X + contentContorl.Width > CanvasWidth || contentContorl.OffsetPos.Y + contentContorl.Height > CanvasHeight)
            {
                CanvasWidth = contentContorl.OffsetPos.X + contentContorl.Width > CanvasWidth ? contentContorl.OffsetPos.X + contentContorl.Width : CanvasWidth;
                CanvasHeight = contentContorl.OffsetPos.Y + contentContorl.Height > CanvasHeight ? contentContorl.OffsetPos.Y + contentContorl.Height : CanvasHeight;
                if (renderTarget2D != null)
                    renderTarget2D.Dispose();
                renderTarget2D = new RenderTarget2D(Platform.GraphicsDevice, CanvasWidth.ConvertToIntPlus(), CanvasHeight.ConvertToIntPlus());
            }
            //ContentContorls.ForEach(cc =>
            //{
            //    cc.CalculateControlSize();
            //    CanvasWidth = cc.OffsetPos.X + cc.Width > CanvasWidth ? cc.OffsetPos.X + cc.Width : CanvasWidth;
            //    CanvasHeight = cc.OffsetPos.Y + cc.Height > CanvasHeight ? cc.OffsetPos.Y + cc.Height : CanvasHeight;
            //});
        }
        public void AddContentContorl(IFrameContent contentContorl)
        {
            ContentContorls.Add(contentContorl);
            CalculateCanvasSize(contentContorl);
        }
        public void Clear()
        {
            ContentContorls.Clear();
            CanvasWidth = CanvasHeight = 0;
        }

        public void Update()
        {
            Scrollbars.ForEach(sb => sb.Update());
        }

    }


}
