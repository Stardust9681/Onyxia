using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Onyxia.Content.Projectiles.DamageClasses.Technician
{
    public class NondescriptLaser : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_0";
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.extraUpdates = 30;
            Projectile.penetrate = -1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[0] = DustID.BlueCrystalShard;
        }
        public override void AI()
        {
            if(Main.rand.NextFloat() < .8f)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center, 1, 1, (int)Projectile.ai[0]);
                d.noGravity = true;
                int r = (int)Projectile.ai[1] % 100;
                int g = (int)(Projectile.ai[1] / 100) % 100;
                int b = (int)(Projectile.ai[1] / 10000) % 100;
                d.color = new Color(r, g, b);
                d.velocity = Vector2.Zero;
            }
        }
    }
}
