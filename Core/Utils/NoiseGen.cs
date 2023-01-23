using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Onyxia.Core.Utils
{
    public class NoiseGen
    {
        //I don't claim to know how I got this to work.
        //I kept trying things until I got something I liked.
        //Seriously was just trial and error, and there was a good few hours of error.

        UnifiedRandom randObj;
        private float[,] arr;
        private int _width;
        private int _height;

        private float strMod;
        private float combMod;
        public NoiseGen(int w, int h, int seed = default, float strMult = .3f, float addAdj = .77f)
        {
            randObj = new UnifiedRandom(seed);
            w = w > 0 ? w : 1;
            h = h > 0 ? h : 1;
            arr = new float[w, h];
            _width = w;
            _height = h;

            strMod = strMult;
            combMod = addAdj;
        }
        public void Reassign(int width, int height)
        {
            arr = new float[width, height];
            _width = width;
            _height = height;
        }
        public float[,] GenerateNoise()
        {
            for(int j = 0; j < _height; j++)
            {
                for(int i = 0; i < _width; i++)
                {
                    int val = randObj.Next(40) + 1;
                    if (val < 5)
                        Add(i, j, val*strMod);
                }
            }
            return arr;
        }
        public float[,] GetCurrentNoise() { return arr; }
        private void Add(int i, int j, float str)
        {
            int strMod = (int)(20 * str);

            for(int y = j - strMod; y < j + strMod; y++)
            {
                if (y < 0 || y >= _height)
                    continue;

                for(int x = i - strMod; x < i + strMod; x++)
                {
                    if (x < 0 || x >= _width)
                        continue;

                    int diff = Math.Abs(i - x) + Math.Abs(j - y);
                    if (diff > strMod)
                        continue;

                    float newVal = (float)(strMod - diff) / (float)strMod;
                    float oldVal = arr[x, y];
                    arr[x, y] = FCombine(oldVal, newVal);
                }
            }
        }
        public void Combine(NoiseGen other)
        {
            if (other._width != _width || other._height != _height)
                return;
            for(int j = 0; j < _height; j++)
            {
                for(int i = 0; i < _width; i++)
                {
                    arr[i, j] = Math.Min((arr[i, j] + other.arr[i, j]), 1);
                }
            }
        }
        private float FCombine(float float1, float float2)
        {
            return MathHelper.Lerp((float1 + float2) * .5f, Math.Max(float1, float2), combMod);
        }

        internal void CreateImage()
        {
            if (!Main.IsGraphicsDeviceAvailable)
                return;
            Texture2D text = new Texture2D(Main.graphics.GraphicsDevice, _width, _height);
            List<Color> c = new List<Color>();
            for(int j = 0; j < _height; j++)
            {
                for(int i = 0; i < _width; i++)
                {
                    c.Add(Color.Lerp(Color.Black, Color.White, arr[i, j]));
                }
            }
            text.SetData(c.ToArray());
            string path = System.IO.Directory.GetCurrentDirectory();
            using (System.IO.FileStream fStream = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite))
            {
                text.SaveAsPng(fStream, _width, _height);
            }
        }
    }
}
