using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Onyxia.Content.Items.Weapons.DamageClasses.Throwing
{
    //Unfinished, am lazy.
    [Autoload(false)]
    public class AstralyteSpear : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Throwing;
        }
    }
}
