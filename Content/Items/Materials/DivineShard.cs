using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Onyxia.Content.Items.Materials
{
    public class DivineShard : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.material = true; //Auto-set, later
        }
        public override void AddRecipes()
        {
            CreateRecipe(5).
                AddIngredient(ItemID.CrystalShard, 3).
                AddIngredient(ModContent.ItemType<AstrallyteShard>(), 2).
                AddIngredient(ItemID.SoulofLight).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            maxFallSpeed = 1f;
        }
    }
}
