using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Onyxia.Content.DamageClasses.Technician;
using Microsoft.Xna.Framework;

namespace Onyxia.Content.Items.Accessories
{
    public class CopperShield : TechItem
    {
        public override string Texture => "Terraria/Images/Item_1";
        protected override void SafeSetDefaults()
        {
            itemDamageType = ModContent.GetInstance<TechDamageClass>();
            Item.accessory = true;
            Item.height = 40;
            Item.width = 40;
            this.chipSlots = 3;
        }
        protected override Shield ItemShield => ShieldLoader.GetShield<AmethystShield>();
    }
    public class AmethystShield : Shield
    {
        public override void SetDefaults()
        {
            this.shieldsMax = 25;
            this.cooldownTime = 200;
        }
        public override Color ShieldColour => Color.MediumVioletRed;
    }

    public class TinShield : TechItem
    {
        public override string Texture => "Terraria/Images/Item_1";
        protected override void SafeSetDefaults()
        {
            itemDamageType = ModContent.GetInstance<TechDamageClass>();
            Item.accessory = true;
            Item.height = 40;
            Item.width = 40;
            this.chipSlots = 3;
        }
        protected override Shield ItemShield => ShieldLoader.GetShield<TopazShield>();
    }
    public class TopazShield : Shield
    {
        public override void SetDefaults()
        {
            this.shieldsMax = 35;
            this.cooldownTime = 240;
            this.damageResist = .01f;
        }
    }

    public class SilverShield : TechItem
    {
        public override string Texture => "Terraria/Images/Item_1";
        protected override void SafeSetDefaults()
        {
            itemDamageType = ModContent.GetInstance<TechDamageClass>();
            Item.accessory = true;
            Item.height = 40;
            Item.width = 40;
            this.chipSlots = 4;
        }
        protected override Shield ItemShield => ShieldLoader.GetShield<SapphireShield>();
    }
    public class SapphireShield : Shield
    {
        public override void SetDefaults()
        {
            this.shieldsMax = 60;
            this.cooldownTime = 200;
        }
        protected override void UpdateActive(Player player)
        {
            player.manaRegen++;
        }
    }

    public class TungstenShield : TechItem
    {
        public override string Texture => "Terraria/Images/Item_1";
        protected override void SafeSetDefaults()
        {
            itemDamageType = ModContent.GetInstance<TechDamageClass>();
            Item.accessory = true;
            Item.height = 40;
            Item.width = 40;
            this.chipSlots = 4;
        }
        protected override Shield ItemShield => ShieldLoader.GetShield<EmeraldShield>();
    }
    public class EmeraldShield : Shield
    {
        public override void SetDefaults()
        {
            this.shieldsMax = 80;
            this.cooldownTime = 280;
            this.regen = 2f;
        }
    }

    public class GoldShield : TechItem
    {
        public override string Texture => "Terraria/Images/Item_1";
        protected override void SafeSetDefaults()
        {
            itemDamageType = ModContent.GetInstance<TechDamageClass>();
            Item.accessory = true;
            Item.height = 40;
            Item.width = 40;
            this.chipSlots = 4;
        }
        protected override Shield ItemShield => ShieldLoader.GetShield<RubyShield>();
    }
    public class RubyShield : Shield
    {
        public override void SetDefaults()
        {
            this.shieldsMax = 85;
            this.cooldownTime = 270;
        }
        protected override void OnCooldown(Player player)
        {
            player.Heal(10);
        }
    }

    public class PlatinumShield : TechItem
    {
        public override string Texture => "Terraria/Images/Item_1";
        protected override void SafeSetDefaults()
        {
            itemDamageType = ModContent.GetInstance<TechDamageClass>();
            Item.accessory = true;
            Item.height = 40;
            Item.width = 40;
            this.chipSlots = 5;
        }
        protected override Shield ItemShield => ShieldLoader.GetShield<DiamondShield>();
    }
    public class DiamondShield : Shield
    {
        public override void SetDefaults()
        {
            this.shieldsMax = 100;
            this.cooldownTime = 300;
            this.damageResist = .05f;
            this.ignoreKb = true;
        }
    }
}
