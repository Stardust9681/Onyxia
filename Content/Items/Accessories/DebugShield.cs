using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Onyxia.Content.DamageClasses.Technician;

namespace Onyxia.Content.Items.Accessories
{
    public class DebugShield : TechItem
    {
        public override string Texture => "Terraria/Images/Item_1";
        //public override TechDamageClass ItemDamageType => ModContent.GetInstance<Blast>();
        protected override void SafeSetDefaults()
        {
            //this.shieldsMax = 100;
            //this.shieldCooldown = 180;
            itemDamageType = ModContent.GetInstance<TechDamageClass>();
            Item.accessory = true;
            Item.height = 40;
            Item.width = 40;
            this.chipSlots = 4;
        }
        protected override Shield ItemShield => ShieldLoader.GetShield<DebugShield2>();
    }

    public class DebugShield3 : TechItem
    {
        public override string Texture => "Terraria/Images/Item_2";
        //public override TechDamageClass ItemDamageType => ModContent.GetInstance<Blast>();
        protected override void SafeSetDefaults()
        {
            //this.shieldsMax = 100;
            //this.shieldCooldown = 180;
            itemDamageType = ModContent.GetInstance<TechDamageClass>();
            Item.accessory = true;
            Item.height = 40;
            Item.width = 40;
            this.chipSlots = 4;
        }
        protected override Shield ItemShield => ShieldLoader.GetShield<DebugShield2>();
    }

    public class DebugShield2 : Shield
    {
        public override void SetDefaults()
        {
            this.shieldsMax = 150;
            this.cooldownTime = 180;
            this.priority = 1;
        }
    }

    public class DebugChip : Chip
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.MoonlordBullet;
        public override void UpdateShieldStats(ref uint shieldMax, ref uint cooldownMax, ref float regen, ref bool? knockback)
        {
            shieldMax += 3000;
        }
    }
}
