using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager;

namespace GamePanels
{
    /// <summary>
    /// 复选框，可以改变字体和字号，还有文字与复选框之间的距离
    /// </summary>
    public class CheckBox
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Path { get; set; }
        public Vector2 Position { get; set; }
        public string Key { get; set; }
        public TextureRecs cbTextureRecs { get; set; }
        public float Alpha = 1f;
        public bool Visible = true;
        public float DrawScale = 1f;
        public float Scale = 1f;
        public SpriteFont ViewFont;
        public bool MouseOver = false;
        public bool PreMouseOver { get; set; }
        public bool Selected = false;
        public Color ViewTextColor1 = Color.White;
        public Color ViewTextColor2 = Color.White;
        public float ViewTextScale = 1f;

        public Rectangle? cbRectangle
        {
            get
            {
                if (cbTextureRecs.Recs != null)
                {
                    return cbTextureRecs.Recs[0];
                }
                else return null;
            }
        }
        public CheckBox(string path, string name, string text, Vector2? pos)
        {
            Text = text;
            Name = name;
            Path = path;
            Key = path + "#" + name;
            cbTextureRecs = Session.TextureRecs[Key];
            if (pos != null) Position = (Vector2)pos;
        }

        public void Draw()
        {
            Draw(null, Color.White * Alpha,1f, 1,null);
        }
        public void Draw(Vector2? basePos, Color color, float alpha, int? texIndex,float ? space)
        {
            Alpha = alpha;
            if (Visible)
            {
                
                Rectangle bounds= CacheManager.Draw(Path, (basePos == null ? Position : (Vector2)(Position + basePos)) * DrawScale, texIndex == null ? cbRectangle : cbTextureRecs.Recs[(int)texIndex], color * Alpha, SpriteEffects.None, Scale);

                CacheManager.DrawString(ViewFont == null ? Session.Current.Font : ViewFont, Text, ((basePos == null ? Position+new Vector2((float)(space??0)+bounds.Width,5) : (Vector2)(Position + basePos+new Vector2((float)(space??0) + bounds.Width,5)))) * DrawScale, (MouseOver || Selected) ? ViewTextColor2 * Alpha : ViewTextColor1 * Alpha, 0f, Vector2.Zero, Scale * ViewTextScale, SpriteEffects.None, 0f);

            }
        }
    }
}
