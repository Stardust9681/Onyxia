using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Onyxia.Content.Dusts
{
    public class SparkleDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noLight = true;
            dust.noGravity = true;
            dust.fadeIn = 300; //Time Left
            //dust.scale *= .1f;
            dust.customData = dust.scale; //Orig Scale
            dust.frame = new Rectangle(0, Main.rand.Next(0, 3) * 10, 10, 10);
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            if(!dust.noGravity)
            {
                dust.velocity.Y += .062f;
            }
            else if(dust.velocity.Y != 0)
            {
                dust.velocity.Y *= .98f;
                if (MathF.Abs(dust.velocity.Y) < .1f)
                    dust.velocity.Y = 0;
            }
            if(dust.velocity.X != 0)
            {
                dust.velocity.X *= .98f;
                if (MathF.Abs(dust.velocity.X) < .1f)
                    dust.velocity.X = 0;
            }
            dust.fadeIn--;
            dust.rotation += .02175f;
            dust.scale = (1 + MathF.Sin(dust.rotation)) * (float)dust.customData;
            dust.customData = (float)(dust.customData) - .001f;
            if ((float)(dust.customData) < .005f || dust.fadeIn < 0)
                dust.active = false;
            /*if ((int)dust.fadeIn % 20 < 2)
            {
                dust.frame.X = (dust.frame.X + 10) % 30;
            }
            dust.frame.X = ((int)(dust.fadeIn)%90)/30;*/
            return false;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return Color.White;
        }
    }
}