using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Onyxia.Content.Items.Weapons.DamageClasses.Technician
{
    public class BlastCanister : Content.DamageClasses.Technician.TechItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.CopperShortsword;
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }
        protected override void SafeSetDefaults()
        {
            Item.shoot = ProjectileID.HeatRay;
            Item.shootSpeed = 4;
            Item.damage = 50;
            Item.useStyle = 5;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noMelee = true;
            this.chipSlots = 5;
        }
    }
    public class BlastChip : Content.DamageClasses.Technician.Chip
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.CopperBroadsword;
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage += 5.05f;
        }
    }
}
