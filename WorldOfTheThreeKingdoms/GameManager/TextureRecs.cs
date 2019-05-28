using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platforms;

namespace GameManager
{
    public struct TextureRecs
    {
        /// <summary>
        /// Cache类型
        /// </summary>
        public string CacheType;
        /// <summary>
        /// 扩展名
        /// </summary>
        public string Ext;
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width;
        /// <summary>
        /// 高度
        /// </summary>
        public int Height;
        /// <summary>
        /// 材质的矩形集合
        /// </summary>
        public Rectangle[] Recs;
    }

    public static class TextureRecsManager
    {
        /// <summary>
        /// 读取材质配置文件
        /// </summary>
        /// <returns></returns>
        public static string ReadTextureMaps()
        {
            return Platform.Current.LoadText("Content/Data/TextureRecs.txt");
        }

        public static Rectangle[] SplitRectangle(this Texture2D texture, bool isHorizontal)
        {
            if (isHorizontal)
            {
                int width = texture.Width / 3; int height = texture.Height;
                return new Rectangle[] { new Rectangle(0, 0, width, height), new Rectangle(width, 0, width, height), new Rectangle(width * 2, 0, width, height) };
            }
            else
            {
                int width = texture.Width; int height = texture.Height / 3;
                return new Rectangle[] { new Rectangle(0, 0, width, height), new Rectangle(0, height, width, height), new Rectangle(0, height * 2, width, height) };
            }
        }

        /// <summary>
        /// 生成一个按钮的所有矩形区域
        /// </summary>
        /// <param name="index">按钮的index</param>
        /// <param name="repeat">重复的次数</param>
        /// <param name="width">按钮宽度</param>
        /// <param name="imageWidth">总宽度</param>
        /// <param name="height">按钮高度</param>
        /// <returns>返回生成的矩形集合</returns>
        public static Rectangle[] FindOneTexRectangles(int index, int repeat, int width, int imageWidth, int height)
        {
            int alreadyWidth = index * repeat * width;
            List<Rectangle> recs = new List<Rectangle>();
            for (int i = 0; i < repeat; i++)
            {
                int totalWidth = alreadyWidth + i * width;
                int row = totalWidth / imageWidth;
                int col = totalWidth % imageWidth;
                Rectangle rec = new Rectangle(col, height * row, width, height);
                recs.Add(rec);
            }
            return recs.ToArray();
        }

        /// <summary>
        /// 读取一行配置中的材质信息
        /// </summary>
        /// <param name="dic">返回存储材质信息的字典</param>
        /// <param name="oneMetas">一行配置的字符串材质元信息</param>
        public static void FindTextureRectangles(Dictionary<string, TextureRecs> dic, string[] oneMetas)
        {
            string name = oneMetas[0].Trim();
            string ext = oneMetas[1].Trim();
            string[] texts = oneMetas[2].Trim().Split(new string[] { "," }, StringSplitOptions.None);
            //總寬度
            int imageWidth = int.Parse(oneMetas[3].Trim());
            //總高度
            int imageHeight = int.Parse(oneMetas[4].Trim());
            //單寬度
            int width = int.Parse(oneMetas[5].Trim());
            //單高度
            int height = int.Parse(oneMetas[6].Trim());
            //重復
            int repeat = int.Parse(oneMetas[7].Trim());
            for (int index = 0; index < texts.Length; index++)
            {
                Rectangle[] recs = FindOneTexRectangles(index, repeat, width, imageWidth, height);
                dic.Add(name + "#" + texts[index], new TextureRecs() { Width = imageWidth, Height = imageHeight, CacheType = oneMetas[8], Ext = ext, Recs = recs });
            }
        }

        public static Dictionary<string, TextureRecs> FindTextureRectangles(Dictionary<string, TextureRecs> dic, string texture)
        {
            Dictionary<string, TextureRecs> textureDic = new Dictionary<string, TextureRecs>();
            foreach (var di in dic)
            {
                if (di.Key.StartsWith(texture)) textureDic.Add(di.Key.Replace(texture + "#", ""), di.Value);
            }
            return textureDic;
        }

        /// <summary>
        /// 获取所有材质键值
        /// </summary>
        /// <returns>返回包含材质的键值对字典</returns>
        public static Dictionary<string, TextureRecs> AllTextureRectangles()
        {
            string[] oneTexs = ReadTextureMaps().Split(new string[] { ";" }, StringSplitOptions.None);
            Dictionary<string, TextureRecs> dic = new Dictionary<string, TextureRecs>();
            foreach (string oneTex in oneTexs)
            {
                FindTextureRectangles(dic, oneTex.Trim().Split(new string[] { ":" }, StringSplitOptions.None));
            }
            return dic;
        }
    }
}
