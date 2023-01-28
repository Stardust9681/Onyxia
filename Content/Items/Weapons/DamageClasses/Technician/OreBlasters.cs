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
using Onyxia.Content.DamageClasses.Technician;

namespace Onyxia.Content.Items.Weapons.DamageClasses.Technician
{
    public class CopperBlaster : TechItem
    {
        public override string Texture => "Terraria/Images/Projectile_0";
        protected override void SafeSetDefaults()
        {
            Item.damage = 2;
            Item.DamageType = ModContent.GetInstance<TechDamageClass>();
            this.chipSlots = 1;
            Item.shoot = ModContent.ProjectileType<Projectiles.DamageClasses.Technician.NondescriptLaser>();
            Item.shootSpeed = 9f;
            Item.useStyle = 5;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.autoReuse = true;
        }
    }
    public class TinBlaster : TechItem
    {
        public override string Texture => "Terraria/Images/Projectile_0";
        protected override void SafeSetDefaults()
        {
            Item.damage = 3;
            Item.DamageType = ModContent.GetInstance<TechDamageClass>();
            this.chipSlots = 1;
            Item.shoot = ModContent.ProjectileType<Projectiles.DamageClasses.Technician.NondescriptLaser>();
            Item.shootSpeed = 9f;
            Item.useStyle = 5;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.autoReuse = true;
        }
    }
    public class SilverBlaster : TechItem
    {
        public override string Texture => "Terraria/Images/Projectile_0";
        protected override void SafeSetDefaults()
        {
            Item.damage = 1;
            Item.DamageType = ModContent.GetInstance<TechDamageClass>();
            this.chipSlots = 2;
            Item.shoot = ModContent.ProjectileType<Projectiles.DamageClasses.Technician.NondescriptLaser>();
            Item.shootSpeed = 9f;
            Item.useStyle = 5;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.autoReuse = true;
        }
    }
    public class TungstenBlaster : TechItem
    {
        public override string Texture => "Terraria/Images/Projectile_0";
        protected override void SafeSetDefaults()
        {
            Item.damage = 2;
            Item.DamageType = ModContent.GetInstance<TechDamageClass>();
            this.chipSlots = 2;
            Item.shoot = ModContent.ProjectileType<Projectiles.DamageClasses.Technician.NondescriptLaser>();
            Item.shootSpeed = 9f;
            Item.useStyle = 5;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.autoReuse = true;
        }
    }
    public class GoldBlaster : TechItem
    {
        public override string Texture => "Terraria/Images/Projectile_0";
        protected override void SafeSetDefaults()
        {
            Item.damage = 5;
            Item.DamageType = ModContent.GetInstance<TechDamageClass>();
            this.chipSlots = 3;
            Item.shoot = ModContent.ProjectileType<Projectiles.DamageClasses.Technician.NondescriptLaser>();
            Item.shootSpeed = 9f;
            Item.useStyle = 5;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.autoReuse = true;
        }
    }
    public class PlatinumBlaster : TechItem
    {
        public override string Texture => "Terraria/Images/Projectile_0";
        protected override void SafeSetDefaults()
        {
            Item.damage = 6;
            Item.DamageType = ModContent.GetInstance<TechDamageClass>();
            this.chipSlots = 3;
            Item.shoot = ModContent.ProjectileType<Projectiles.DamageClasses.Technician.NondescriptLaser>();
            Item.shootSpeed = 9f;
            Item.useStyle = 5;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.autoReuse = true;
        }
    }
}
