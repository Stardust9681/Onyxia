using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace Onyxia.Content.Items.Materials
{
    public class PrimalWater : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = 400;
            Item.rare = 1;
            Item.material = true;
            Item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            Recipe r = CreateRecipe().
                AddIngredient(ModContent.ItemType<MoteOfWater>(), 3).
                AddCondition(Recipe.Condition.NearWater);
            r.AddCondition(new Recipe.Condition(NetworkText.FromFormattable("Requires Philosopher's Stone"), r => Main.LocalPlayer.HasItem(Terraria.ID.ItemID.PhilosophersStone))).
                Register();
        }
    }
}