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
    //What does this need to do?
    //Apply to weapons
    //Change how the item is fired
    //Change what is fired from item
    //Change stats
    //Apply to accessories
    //Cause effects during active shielding
    //Change shield stats
    //Apply to armour
    //Modify player stats

    #region Dead Code
    /*
    public abstract class Chip : ModItem
    {
        public TechDamageClass damageType;
        public sealed override void SetDefaults()
        {
            SafeSetDefaults();
            Item.useStyle = 0;
            Item.useAnimation = 0;
            Item.useTime = 0;
        }
        public virtual void SafeSetDefaults() { }
        public sealed override bool CanUseItem(Player player)
        {
            return false;
        }

        //Use shoot, shoot stats, modifyweapon, etc. for weapon application
        public virtual void Shoot(Player player, IEntitySource source, int damageLevel, Vector2 pos, Vector2 vel, int damage, float knockBack) { }
        public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => false;

        public virtual void ModifyShieldStats(ref int maxShields, ref int cooldown, ref float regen, ref float damageResist) { }
        //Use UpdateAccessory for accessory application

        //Use UpdateEquip for armour application
    }
    public class DebugChip : Chip
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.LifeCrystal;
        public override void SafeSetDefaults()
        {
            damageType = ModContent.GetInstance<Blast>();
        }
        public override void Shoot(Player player, IEntitySource source, int damageLevel, Vector2 pos, Vector2 vel, int damage, float knockBack)
        {
            if(damageLevel == 1)
            {
                for(int i = 0; i < 8; i++)
                {
                    Projectile p = Projectile.NewProjectileDirect(source, pos, vel.RotatedByRandom(.7f)*Main.rand.NextFloat(.6f, 1.05f), ModContent.ProjectileType<Projectiles.DamageClasses.Technician.BlastI>(), damage, 0, player.whoAmI);
                    p.DamageType = this.damageType;
                    p.timeLeft = 10;
                }
            }
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            damage += 3;
        }
    }
    */
    #endregion

    public abstract class Chip : ModItem
    {
        public override string Texture => "Terraria/Images/Item_0";
        public virtual void UpdateShieldStats(ref uint shieldMax, ref uint cooldownMax, ref float regen, ref bool? knockback) { }
        public virtual bool AppliesToItem(Item item) { return true; }
        public virtual void ModifyProj(Projectile p) { }
    }

    [Autoload(false)]
    public class Chip_Default : Chip { }

    //You totally won't remember this.
    //No fault of your own, here.
    //It's okay! You've got this.
    //Now, breathe, understand that this is a concept in the works, and that it may be changed
    //In fact, this is the second or third iteration, itself.
    //Plans for this are (currently) as follows:
    //Each individual item RETAINS its damage type
    //and can only have chips of the same damage type
    //This locks a couple of things out for each class, sadly
    //But will allow more creative takes on certain things
    //Eg, actual effects (not just stats) on chips
    //Compare to DD2's shard system
}
