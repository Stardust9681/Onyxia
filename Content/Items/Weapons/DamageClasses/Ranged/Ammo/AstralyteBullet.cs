using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Onyxia.Content.Items.Weapons.DamageClasses.Ranged.Ammo
{
    public class AstralyteBullet : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 6;
            Item.width = 10;
            Item.height = 18;
            Item.ammo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<Projectiles.DamageClasses.Ranged.AstralyteBullet>();
            Item.shootSpeed = 8.4f;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.rare = 3;
        }
        public override void AddRecipes()
        {
            CreateRecipe(40).
                AddIngredient(ItemID.SilverBullet, 40).
                AddIngredient(ModContent.ItemType<Materials.AstralyteShard>(), 3).
                AddTile(TileID.Anvils).
                Register();
            CreateRecipe(40).
                AddIngredient(ItemID.TungstenBullet, 40).
                AddIngredient(ModContent.ItemType<Materials.AstralyteShard>(), 3).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
