using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Onyxia.Content.Globals;

namespace Onyxia.Content.Items.Weapons.DamageClasses.Melee
{
    public class AquaiteSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.autoReuse = true;
            Item.width = 44;
            Item.height = 44;
            Item.rare = 1;
            Item.value = 3000;
            Item.knockBack = 2f;
            Item.useStyle = OnyxiaItem.CustomUsestyleID.Swipe;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.reuseDelay = 3;
            Item.UseSound = Terraria.ID.SoundID.Item85;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextFloat() < .33f)
            {
                Dust d = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.AquaiteDust>());
                d.scale = Main.rand.NextFloat(.8f, .95f);
                d.noGravity = true;
                d.alpha = 50;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<Materials.Aquaite>(), 8).
                AddCondition(Recipe.Condition.NearWater).
                Register();
        }
    }
}
