using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Onyxia.Content.Projectiles.DamageClasses.Thrown
{
    public class AstralyteSpear : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.timeLeft = 900;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 2;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            if(Main.rand.NextFloat() < .075f)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.SparkleDust>());
                d.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
                d.rotation = d.position.X;
                d.fadeIn = 90;
            }
        }
        public override void Kill(int timeLeft)
        {
            for(int i = 0; i < 3 + Main.rand.Next(5); i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center, 1, 1, ModContent.DustType<Dusts.SparkleDust>());
                d.velocity = Vector2.UnitX.RotateRandom(MathHelper.TwoPi) * Main.rand.NextFloat() * 4f;
                d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                d.fadeIn = 60;
            }
        }
    }
}
