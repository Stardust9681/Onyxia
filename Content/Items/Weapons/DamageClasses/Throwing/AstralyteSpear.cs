using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Onyxia.Content.Items.Weapons.DamageClasses.Throwing
{
    public class AstralyteSpear : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Throwing;
            Item.width = 10;
            Item.height = 22;
            Item.damage = 17;
            Item.knockBack = 1f;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.DamageClasses.Thrown.AstralyteSpear>();
            Item.consumable = true;
            Item.rare = 3;
            Item.shootSpeed = 5.3f;
            Item.maxStack = 999;
            Item.noMelee = true;
            Item.rare = 3;
        }
        public override void AddRecipes()
        {
            CreateRecipe(50).
                AddIngredient(ModContent.ItemType<Materials.AstralyteShard>(), 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
