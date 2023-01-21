using System;
using System.Reflection;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.UI;
using Terraria.GameInput;
using Terraria.WorldBuilding;
using Terraria.IO;
using Terraria.GameContent.Generation;

using Onyxia.Content.DamageClasses.Technician;
using Onyxia.Assets;
using Onyxia.Core.Utils;

namespace Onyxia.Content.Commands
{
    public class TestGen : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "noisegen";
        public override string Usage => "noisegen [width] [height]";
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            //input = literal string value of what's given
            //args = every value after the command itself
            //Ex, command run = /noisegen Obj1 Obj2
            //input = /noisegen Obj1 Obj2
            //args = { Obj1, Obj2 }

            WorldGen.noTileActions = true;
            WorldGen.gen = true;
            int w = 250;
            int h = 100;
            if(args.Length > 0)
                int.TryParse(args[0], out w);
            if(args.Length > 1)
                int.TryParse(args[1], out h);
            Point start = (caller.Player.position*0.0625f).ToPoint();
            WorldUtils.Gen(start, new Shapes.Rectangle(w, h), new Actions.Clear());
            NoiseGen gen1 = new NoiseGen(w, h, w^h);
            float[,] vals = gen1.GenerateNoise();
            gen1.CreateImage();
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    if (vals[i, j] > .5f)
                        WorldGen.PlaceTile(start.X + i, start.Y + j, TileID.IceBrick, default, true);
                    else if (vals[i, j] > .3f)
                        WorldGen.PlaceTile(start.X + i, start.Y + j, TileID.SnowBrick, default, true);
                    //For some reason this is basically blank, check NoiseGen class probably
                    //But for real, not sure what the problem is.
                }
            }
            WorldUtils.Gen(start, new Shapes.Rectangle(w, h), new Actions.SetFrames(true));
            WorldGen.noTileActions = false;
            WorldGen.gen = false;
        }
    }
}
