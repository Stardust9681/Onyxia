using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Onyxia.Content.Projectiles.DamageClasses.Magic
{
    public class StarSurge : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_0";
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 2;
            Projectile.width = 30;
            Projectile.height = 40;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            if (!Framing.GetTileSafely(Projectile.Center).HasTile)
                Projectile.tileCollide = true;
            float normDust = Main.rand.NextFloat();
            if (normDust < .05f)
            {
                Dust d = Dust.NewDustDirect(new Vector2(Projectile.position.X + MathHelper.Lerp(0, Projectile.width, normDust * 20), Projectile.Center.Y), 1, 1, ModContent.DustType<Dusts.SparkleDust>());
                d.velocity = Vector2.Zero;
                d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            }
            if(normDust < .6f)
            {
                /*Dust d = Dust.NewDustDirect(new Vector2(Projectile.position.X + MathHelper.Lerp(0, Projectile.width, normDust * 4 / 3), Projectile.Center.Y), 1, 1, ModContent.DustType<Dusts.SparkleDust>());
                d.color = new Color(246, 116, 247);
                d.scale = MathHelper.Lerp(.7f, .9f, normDust * 4 / 3);
                d.frame.X = 10;
                d.velocity = Projectile.velocity * .67f;
                d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                d.fadeIn = 12;
                d.alpha = 50;*/

                Dust d = Dust.NewDustDirect(new Vector2(Projectile.position.X + MathHelper.Lerp(Projectile.width/4, Projectile.width*3/4, normDust * 5 / 3), Projectile.Center.Y), 1, 1, DustID.YellowStarDust);
                d.velocity = Projectile.velocity * .67f;
                d.scale *= normDust * 10 / 3;
                d.noGravity = true;
                d.rotation = normDust * MathHelper.TwoPi * 5 / 3;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 5 + Main.rand.Next(5); i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center, 1, 1, ModContent.DustType<Dusts.SparkleDust>());
                d.velocity = Vector2.UnitX.RotateRandom(MathHelper.TwoPi) * Main.rand.NextFloat() * 4f;
                d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                d.fadeIn = 60;
            }
            return base.OnTileCollide(oldVelocity);
        }
    }
}
