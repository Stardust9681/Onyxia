using System;
using Terraria;
using Terraria.ModLoader;

namespace Onyxia.Content.Items.Materials
{
    class AquaiteBar : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = 400;
            Item.rare = 1;
            Item.material = true;
            Item.maxStack = 99;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<Aquaite>(), 3).
                AddCondition(Recipe.Condition.NearWater).
                Register();
        }
    }
}