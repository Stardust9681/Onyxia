using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Onyxia.Content.Dusts
{
    public class NecromanticDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noLight = true;
            dust.frame = new Rectangle(0, Main.rand.Next(0, 3) * 12, 12, 12);
            dust.noGravity = true;
        }
        public override bool Update(Dust dust)
        {
            if (!dust.noGravity)
                dust.velocity.Y += .112f;
            dust.position += dust.velocity;
            if (dust.fadeIn == 0)
                dust.velocity = dust.velocity.RotatedByRandom(MathHelper.ToRadians(8f)) * .964f;
            dust.rotation += MathHelper.ToRadians(6f);
            dust.scale -= .03f;
            dust.alpha += 5;
            if (dust.scale < .2f || dust.alpha > 225)
                dust.active = false;
            Lighting.AddLight(dust.position, new Vector3(.35f, .45f, .25f) * dust.scale);
            return false;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return dust.noLight ? Color.White : lightColor;
        }
    }
}
