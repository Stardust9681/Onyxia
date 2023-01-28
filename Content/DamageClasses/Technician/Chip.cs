using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Onyxia.Content.DamageClasses.Technician
{
    //Apply to weapons
    //Change how the item is fired
    //Change what is fired from item
    //Change stats
    //Apply to accessories
    //Cause effects during active shielding
    //Change shield stats
    //Apply to armour
    //Modify player stats

    public abstract class Chip : ModItem
    {
        public override string Texture => "Terraria/Images/Item_0";
        public virtual void UpdateShieldStats(ref uint shieldMax, ref uint cooldownMax, ref float regen, ref bool? knockback) { }
        public virtual bool AppliesToItem(Item item) { return true; }
        public virtual void ModifyProj(Projectile p) { }
        public virtual bool SpawnProjs(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out Projectile[] newProjs)
        { newProjs = null; return false; }
    }

    [Autoload(false)]
    public class Chip_Default : Chip { }
}
