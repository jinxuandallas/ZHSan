using System;
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
        public void DrawTexture()
        {
            List<Texture2D> textures = new List<Texture2D>();
            textures = CacheManager.DrawStringToTexture(Session.Current.Font, "试试吧看看s试试就试试", new Vector2(60, 60), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            foreach (Texture2D texture in textures)
            {
                Session.Current.SpriteBatch.Draw(texture, new Vector2(200, 200), Color.White);
            }
        }
    }
}
