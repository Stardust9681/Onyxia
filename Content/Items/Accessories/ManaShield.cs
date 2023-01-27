using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Onyxia.Content.Items.Accessories
{
    public class ManaShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Splits damage taken between Health and Mana.");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            Item.rare = 1;
            Item.value = 20000 * 5;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<OnyxiaPlayer>().manaShield = .2f;
        }
    }
}
