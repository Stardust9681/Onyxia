using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Onyxia.Content.Items.Weapons.DamageClasses.Melee
{
    public class DivineSword : ModItem //Starcrystal -> Divine
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 36;
            Item.knockBack = 3.6f;
            Item.value = 3600 * 5;
            Item.useStyle = 16;
            Item.height = 40;
            Item.width = 30;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.reuseDelay = 9;
            Item.autoReuse = true;
        }

        //AddRecipes
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.GetGlobalNPC<Content.Globals.OnyxiaNPC>().AdjustSpeed(.3f);
        }
    }
}
