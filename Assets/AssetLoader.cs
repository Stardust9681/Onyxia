using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace Onyxia.Assets
{
    /*public static class AssetLoader
    {
        private static Dictionary<string, Asset<Texture2D>> Assets
        {
            get;
            set;
        }
        public static string GetPath(string assetName) => "Onyxia/Assets/" + assetName;
        public static void Initialise()
        {
            //Run IO logic here to get all image resources that don't have an associated class (or cs file? idk)
            Assets = new Dictionary<string, Asset<Texture2D>>();
            /*foreach (string s in Directory.GetFiles(typeof(AssetLoader).FullName.Replace('.', '/')))
            {
                if (!new FileInfo(s).Extension.Equals(".png")) continue;
                Assets.Add(new FileInfo(s).FullName,
                    Terraria.ModLoader.ModContent.Request<Texture2D>(s));
            } / / / / / END MULTILINE COMMENT HERE / / / / /
        }
        private static void CacheTexture(string path)
        {
            if (TextureCached(path)) return;
            Assets.Add(path, ModContent.Request<Texture2D>(path));
        }
        public static void Disengage()
        {
            if (Assets == null)
                return;
            string[] keys = Assets.Keys.ToArray();
            foreach(string s in keys)
            {
                //Assets[s].Dispose();
                Assets.Remove(s);
            }
            Assets.Clear();
            Assets = null;
        }
        /// <summary>
        /// Throws an exception when texture is missing, can cause problems when used normally.
        /// Use <see cref="GetTexture(string, out Asset{Texture2D})"/> instead.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="Terraria.ModLoader.Exceptions.MissingResourceException"></exception>
        public static Asset<Texture2D> Request(string path)
        {
            if(!Assets.ContainsKey(path))
            {
                throw new Terraria.ModLoader.Exceptions.MissingResourceException($"Designated resource, {path}, could not be found (Onyxia/Assets/AssetLoader.cs, line 53)");
            }
            string[] keys = Assets.Keys.ToArray();
            for(int i = 0; i < keys.Length; i++)
            {
                if (keys[i].EndsWith(path) || keys[i].EndsWith(path + ".png"))
                    return Assets[keys[i]];
            }
            return Terraria.GameContent.TextureAssets.MagicPixel;
        }
        private static bool TextureCached(string path)
        {
            string[] keys = Assets.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].EndsWith(path) || keys[i].EndsWith(path + ".png"))
                    return true;
            }
            return false;
        }

        public static bool GetTexture(string path, out Asset<Texture2D> texture)
        {
            path = GetPath(path);
            bool ret = true;
            if(!TextureCached(path))
            {
                CacheTexture(path);
                ret = false;
            }
            texture = Request(path);
            return ret;
        }
    }*/
}
