using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.CodeAnalysis.Diagnostics;
using Onyxia.Content.Globals;
using IL.Terraria.DataStructures;

namespace Onyxia.Content.Items.Weapons.DamageClasses.Melee
{
    public class NecromanticSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.width = 44;
            Item.height = 44;
            Item.rare = 2;
            Item.useStyle = OnyxiaItem.CustomUsestyleID.Swipe;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 29;
            Item.knockBack = 1f;
            Item.autoReuse = true;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextFloat() < .33f)
            {
                Dust d = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.NecromanticDust>());
                d.scale = Main.rand.NextFloat(.8f, .95f);
                d.noGravity = true;
                d.alpha = 50;
            }
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (crit)
            {
                player.statLife += damage / 5;
                player.HealEffect(damage / 5);
            }
        }
    }
}
