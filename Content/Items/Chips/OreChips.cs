using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Onyxia.Content.DamageClasses.Technician;

namespace Onyxia.Content.Items.Chips
{
    public class CopperChip : Chip
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.CopperOre;
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage += .01f;
        }
    }
    public class TinChip : Chip
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.TinOre;
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage += .02f;
        }
    }
}
