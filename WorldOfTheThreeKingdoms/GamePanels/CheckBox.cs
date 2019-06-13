using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager;
using Platforms;
using FontStashSharp;

namespace GamePanels
{
    public class CheckBoxPressEventArgs : EventArgs
    {
        public CheckBoxPressEventArgs()
        {
        }
    }
    public delegate void CheckBoxPressEventHandler(object sender, ButtonPressEventArgs e);
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
        public bool Sound = true;
        public float Scale = 0.8f;
        public SpriteFont ViewFont;
        public bool MouseOver = false;
        public bool PreMouseOver { get; set; }
        public bool Selected = false;
        public Color ViewTextColor1 = Color.White;
        public Color ViewTextColor2 = Color.Yellow;
        public DateTime? prePressTime;
        public float ViewTextScale = 1f;
        public bool Enable = true;
        public bool Locked = false;
        public int Width { get; set; }
        public int Height { get; set; }
        public int ExtDis = 0;
        public bool FireEventWhenUnEnable = false;
        public event CheckBoxPressEventHandler OnMouseOver, OnButtonPress;

        public Rectangle? cbRectangle
        {
            get
            {
                if (cbTextureRecs.Recs != null)
                {
                    if (Locked)
                    {
                        return cbTextureRecs.Recs.Length > 3 ? cbTextureRecs.Recs[3] : cbTextureRecs.Recs[0];
                    }
                    else if (Enable)
                    {
                        return (!Selected && !MouseOver ? cbTextureRecs.Recs[0] : (cbTextureRecs.Recs.Length > 1 ? cbTextureRecs.Recs[1] : cbTextureRecs.Recs[0]));
                    }
                    else
                    {
                        if (cbTextureRecs.Recs.Length > 3)
                        {
                            return Selected ? cbTextureRecs.Recs[3] : cbTextureRecs.Recs[2];
                        }
                        else if (cbTextureRecs.Recs.Length > 2)
                        {
                            return cbTextureRecs.Recs[2];
                        }
                        else
                        {
                            return Selected ? cbTextureRecs.Recs[1] : cbTextureRecs.Recs[0];
                        }
                    }
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
        public void PressButton()
        {
            InputManager.ClickTime++;
            DateTime now = DateTime.Now;
            if (prePressTime != null)
            {
                TimeSpan ts = now - (DateTime)prePressTime;
                if (ts.TotalSeconds < 0.3f || !Platform.IsActive)
                {
                    return;
                }
            }
            prePressTime = now;
            if (OnButtonPress != null) OnButtonPress.Invoke(this, null);
        }

        public bool IsInTexture(float poX, float poY, Vector2? basePos)
        {
            if (Visible && (Enable || FireEventWhenUnEnable))
            {
                PreMouseOver = MouseOver;
                if (basePos == null)
                {
                    MouseOver = this.Position.X - ExtDis <= poX && poX <= this.Position.X + Width * Scale + ExtDis
                        && this.Position.Y - ExtDis <= poY && poY <= this.Position.Y + Height * Scale + ExtDis;
                }
                else
                {
                    MouseOver = this.Position.X + ((Vector2)basePos).X - ExtDis <= poX && poX <= this.Position.X + ((Vector2)basePos).X + Width * Scale + ExtDis
                        && this.Position.Y + ((Vector2)basePos).Y - ExtDis <= poY && poY <= this.Position.Y + ((Vector2)basePos).Y + Height * Scale + ExtDis;
                }
            }
            else
            {
                MouseOver = false;
            }
            return MouseOver;
        }
        public bool IsInTexture(int poX, int poY, Vector2? basePos)
        {
            if (Visible && (Enable || FireEventWhenUnEnable))
            {
                PreMouseOver = MouseOver;
                if (basePos == null)
                {
                    MouseOver = this.Position.X - ExtDis <= poX && poX <= this.Position.X + Width + ExtDis
                        && this.Position.Y - ExtDis <= poY && poY <= this.Position.Y + Height + ExtDis;
                }
                else
                {
                    MouseOver = this.Position.X + ((Vector2)basePos).X - ExtDis <= poX && poX <= this.Position.X + ((Vector2)basePos).X + Width + ExtDis
                        && this.Position.Y + ((Vector2)basePos).Y - ExtDis <= poY && poY <= this.Position.Y + ((Vector2)basePos).Y + Height + ExtDis;
                }
            }
            else
            {
                MouseOver = false;
            }
            return MouseOver;
        }
        public void Update()
        {
            Update(null);
        }
        public void Update(Vector2? basePos)
        {
            Update(InputManager.PoX, InputManager.PoY, basePos);
        }
        public void Update(int poX, int poY, Vector2? basePos)
        {
            Update(poX, poY, basePos, InputManager.IsPressed, Sound);
        }
        //SoundPlayer player = new SoundPlayer("Content/Textures/Resources/Start/Select");
        public void Update(int poX, int poY, Vector2? basePos, bool press, bool sound)
        {
            MouseOver = Enable && IsInTexture(Convert.ToSingle(poX) / DrawScale, Convert.ToSingle(poY) / DrawScale, basePos) && (poX != 0 || poY != 0);
            if (MouseOver)
            {
                if (!PreMouseOver && sound)
                {
                    //player.Play();
                    Platform.Current.PlayEffect(@"Content\Sound\Select");
                }
                if (OnMouseOver != null) OnMouseOver.Invoke(null, null);
                if (press)
                {
                    PressButton();
                    press = false;
                }
            }
        }

        public void Draw()
        {
            Draw(null, Color.White * Alpha, 1f, null, null);
        }
        public void Draw(Vector2? basePos, Color color, float alpha, int? texIndex, float? space)
        {
            Alpha = alpha;
            if (Visible)
            {

                Bounds bounds = CacheManager.Draw(Path, (basePos == null ? Position : (Vector2)(Position + basePos)) * DrawScale, texIndex == null ? cbRectangle : cbTextureRecs.Recs[(int)texIndex], color * Alpha, SpriteEffects.None, Scale);

                Width =(int)(bounds.Width);
                Height = (int)(bounds.Height);

                CacheManager.DrawString(ViewFont ?? Session.Current.Font, Text, ((basePos == null ? Position + new Vector2((float)(space ?? 0) + Width, 2) : (Vector2)(Position + basePos + new Vector2((float)(space ?? 0) + Width, 2)))) * DrawScale, (MouseOver || Selected) ? ViewTextColor2 * Alpha : ViewTextColor1 * Alpha, 0f, Vector2.Zero, Scale * ViewTextScale, SpriteEffects.None, 0f);


            }

        }
    }
}
