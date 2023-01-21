using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Onyxia.Content.Items.Materials
{
    public class AstrallyteShard : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.rare = 2;
            Item.maxStack = 99;
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            if (Item.position.Y < Main.maxTilesY * .063f)
                maxFallSpeed = 0.1f;
            else
                maxFallSpeed = 1f;
        }
    }
}
