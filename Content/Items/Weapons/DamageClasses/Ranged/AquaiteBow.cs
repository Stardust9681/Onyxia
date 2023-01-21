using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Onyxia.Content.Items.Weapons.DamageClasses.Ranged
{
    public class AquaiteBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 44;
            Item.DamageType = DamageClass.Ranged;
            Item.useAmmo = AmmoID.Arrow;
            Item.damage = 11;
            Item.shoot = 10;
            Item.shootSpeed = 12f;
            Item.useStyle = 5;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.autoReuse = true;
        }
    }
}
