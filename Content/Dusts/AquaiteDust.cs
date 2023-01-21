using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Onyxia.Content.Dusts
{
    class AquaiteDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noLight = true;
            dust.noGravity = true;
            dust.fadeIn = 30f;
            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            dust.frame = new Rectangle(0, Main.rand.Next(0, 3) * 12, 12, 12);
        }
        public override bool Update(Dust dust)
        {
            /*if (dust.noGravity)
            {
                dust.position.Y += dust.velocity.Y;
                dust.velocity *= (1 - .00164f);
            }
            else
                dust.position.Y += .4f;
            dust.position.X += dust.velocity.X;
            if (dust.velocity.LengthSquared() < 20f)
                dust.scale -= .006f;
            else
                dust.scale -= .018f;
            dust.rotation += .01f;
            if (dust.scale < .4f)
                dust.active = false;*/
            if (dust.fadeIn > 0)
            {
                dust.scale += .01f;
                dust.fadeIn -= .5f;
            }
            dust.position += dust.velocity;
            if (dust.noGravity)
            {
                dust.scale -= .0146f;
            }
            else
            {
                dust.velocity.Y += .2f;
            }
            dust.scale -= .016f;
            dust.rotation += .087f;
            dust.alpha += 5;
            if (dust.scale < .5f || dust.alpha > 225)
                dust.active = false;
            return false;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            if (dust.noLight)
                return new Color(255, 255, 255, 200);
            else
                return lightColor;
        }
    }
}
