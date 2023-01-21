using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Onyxia.Content.Items.Accessories
{
    public class BloodLotus : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.JungleRose;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("-25% damage.\nDealing damage increases your maximum life, up to +90");
        }
        //Master-exclusive from BoC maybe?
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.value = 2000 * 5;
            Item.rare = ItemRarityID.Expert;
            Item.expert = true;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //player.GetModPlayer<OnyxiaPlayer>().bloodLotus = true;
        }
    }
}
