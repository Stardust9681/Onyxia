using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Onyxia.Content.Projectiles.DamageClasses.Technician
{
    public class BlastI : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_0";
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.extraUpdates = 10;
            Projectile.timeLeft = 15;
        }
        public override void AI()
        {
            if (Projectile.ai[1] == 0 && Projectile.timeLeft > 2)
            {
                Projectile.ai[1] = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            if (Projectile.timeLeft < 2 && Projectile.ai[0] < 60)
            {
                Projectile.timeLeft = 2;
                Projectile.ai[0]++;
                Projectile.velocity = Vector2.Zero;
            }
        }
        /*public override void PostDraw(Color lightColor)
        {
            if (Projectile.timeLeft < 3 || Projectile.ai[0] > 0)
            {
                Assets.AssetLoader.GetTexture("Burst", out ReLogic.Content.Asset<Texture2D> aTexture);
                Texture2D texture = aTexture.Value;
                if (aTexture.Value != null)
                {
                    Vector2 drawOrigin = new Vector2(texture.Width * 0.5f * .0013f, texture.Height * 0.5f * .0013f);
                    Vector2 pos = (Projectile.position - Main.screenPosition) + drawOrigin + new Vector2(0, Projectile.gfxOffY);
                    Color c = Color.Red * ((-Projectile.ai[0] + 60f) * .0167f);
                    //Main.EntitySpriteDraw(texture, pos, null, c, Projectile.velocity.ToRotation(), Vector2.Zero, .032f, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(texture, pos, null, c, Projectile.ai[1], drawOrigin, .032f, SpriteEffects.None, 0f);
                }
            }
        }*/
    }
}
