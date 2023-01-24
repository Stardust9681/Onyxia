using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Onyxia.Core.Utils.Extensions;

namespace Onyxia.Content.Projectiles.DamageClasses.Ranged
{
    public class AstralyteBullet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 18;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
                Projectile.ai[0] = Projectile.timeLeft;
            Projectile.velocity *= 1.0035f;
            if (Main.rand.NextFloat() < .75f)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.YellowStarDust);
                d.noGravity = true;
                d.velocity = Projectile.velocity * .67f;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.IsLocallyOwned())
            {
                if (crit)
                {
                    Vector2 spawnPos = Projectile.position - (Projectile.velocity * (Projectile.ai[0] - Projectile.timeLeft));
                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), spawnPos, Projectile.velocity * 2f, Projectile.type, Projectile.damage / 2, knockback, Projectile.owner);
                    p.DamageType = Projectile.DamageType;
                    p.CritChance += Projectile.CritChance - 1;
                }
            }
        }
    }
}
