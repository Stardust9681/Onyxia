using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Onyxia.Content.Items.Weapons.DamageClasses.Magic
{
    public class Starfall : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.width = 24;
            Item.height = 14;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = 5;
            Item.mana = 15;
            Item.damage = 34;
            Item.knockBack = .1f;
            Item.crit = 8;
            Item.value = 12000 * 5;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<Projectiles.DamageClasses.Magic.StarSurge>();
            Item.autoReuse = true;
            Item.noMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                position.X = Main.MouseWorld.X;
                position.Y = Main.MouseWorld.Y - 240f;
                velocity = new Vector2(0, 1) * velocity.Length();
                Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI).netUpdate=true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).
                AddIngredient(ModContent.ItemType<Materials.AstralyteShard>(), 8).
                AddIngredient(ItemID.Book, 5).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
