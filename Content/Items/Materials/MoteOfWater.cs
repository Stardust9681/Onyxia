using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Onyxia.Content.Items.Materials
{
    public class MoteOfWater : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = 120*5;
            Item.rare = 1;
            Item.material = true;
            Item.maxStack = 999;
        }
    }
}