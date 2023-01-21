using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Onyxia.Content.Globals;

namespace Onyxia.Content.Items.Tools
{
    public class PlatformRod : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.SapphireStaff;
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.SapphireStaff);
            Item.mana = 0;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool? UseItem(Player player)
        {
            if(player.altFunctionUse == 2)
            {
                EntityPlatform plat = EntityPlatform.NewPlatformDirect<Platforms.LoosePlatform>(Main.MouseWorld);
                plat.position -= plat.Size * .5f;
                
            }
            else
            {
                EntityPlatform plat = EntityPlatform.NewPlatformDirect<Platforms.EmptyPlatform>(Main.MouseWorld);
                plat.position -= plat.Size * .5f;
            }
            return true;
        }
    }
}
