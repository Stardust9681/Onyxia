using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Onyxia.Content.Items.Weapons.DamageClasses.Melee
{
    public class AstralyteBlade : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 16;
            Item.width = 40;
            Item.height = 40;
            Item.value = 2500 * 5;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.rare = 3;
            Item.useTurn = true;
            Item.knockBack = 2;
            Item.autoReuse = true;
            Item.useStyle = Globals.OnyxiaItem.CustomUsestyleID.Swipe;
        }
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (target.position.Y < 1280)
            {
                damage = (int)(damage * 1.1f);
                for (int i = 0; i < 2 + Main.rand.Next(3); i++)
                {

                    Dust d = Dust.NewDustDirect(target.Center, 1, 1, ModContent.DustType<Dusts.SparkleDust>());
                    d.velocity = Vector2.Zero;
                    d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                    d.fadeIn = 40;
                }
            }
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if(Main.rand.NextFloat() < .1f)
            {
                Dust d = Dust.NewDustDirect(hitbox.Location.ToVector2(), hitbox.Width, hitbox.Height, DustID.YellowStarDust);
                d.velocity = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * 2.8f;
                d.scale *= 2f;
                d.noGravity = true;
            }
            base.MeleeEffects(player, hitbox);
        }
    }
}
