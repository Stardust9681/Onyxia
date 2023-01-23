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
using Terraria.ModLoader.IO;

namespace Onyxia.Content.DamageClasses.Technician
{
    public abstract class TechItem : ModItem
    {
        /*[14:42:59.613] [.NET ThreadPool Worker/WARN] [tML]: Onyxia.Content.DamageClasses.Technician.TechItem has reference fields (_itemShield, <ItemShield>k__BackingField, modules) that may not be safe to share between clones.
For deep-cloning, add a custom Clone override and make proper copies of these fields. If shallow (memberwise) cloning is acceptable, mark the fields with [CloneByReference] or properties with [field: CloneByReference]
        */
        private Shield _itemShield = null;
        public TechDamageClass itemDamageType;
        public Shield Shield
        {
            get
            {
                if (_itemShield != null)
                    return _itemShield;
                _itemShield = ItemShield;
                IsShield = (_itemShield!=null);
                if (IsShield)
                    return _itemShield;
                return null;
                //return (IsShield = ((_itemShield = ItemShield) != null)) ? _itemShield : null;
            }
        }
        protected virtual Shield ItemShield
        {
            get;
        }
        private bool _isShield = true;
        public bool IsShield
        {
            get => _isShield;
            private set => _isShield = value;
        }

        protected int chipSlots = 0;
        private Item[] modules;
        public int ModSlots => modules.Length;
        /*public int NumMods
        {
            get
            {
                //return 0;
                if (modules == null || modules.Length < 1) return 0;
                int cap = 0;// = modules.ToList().FindIndex(x => x!=null && x.Item.IsAir);
                for (int i = 0; i < NumMods; i++)
                {
                    Item it = modules[i];
                    if (it == null || it.IsAir || it.ModItem == null) continue;
                    cap++;
                }
                return cap;
            }
        }*/

        //Inherited SetDefaults
        protected virtual void SafeSetDefaults() { }
        protected virtual void VolatileSetDefaults() { }
        public sealed override void SetDefaults()
        {
            SafeSetDefaults();
            modules = new Item[chipSlots];
            for(int i = 0; i < chipSlots; i++)
            {
                modules[i] = new Item();
            }
            if(itemDamageType == null)
            {
                itemDamageType = ModContent.GetInstance<Generic>();
            }
            Item.DamageType = itemDamageType;
            VolatileSetDefaults();
            Logging.PublicLogger.Info("Loading " + GetType().Name + " shield: " + Shield?.GetType().Name);

        }

        //Inherited ModifyWeaponDamage
        protected virtual void PostModifyWeaponDamage(Player player, ref StatModifier damage) { }
        public sealed override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            for (int i = 0; i < ModSlots; i++)
            {
                Item? it = modules[i];
                if (it == null || it.IsAir || it.ModItem == null) continue;
                it.ModItem.ModifyWeaponDamage(player, ref damage);
            }
            PostModifyWeaponDamage(player, ref damage);
        }

        //Inherited UpdateEquip
        protected virtual void PostUpdateEquip(Player player) { }
        public sealed override void UpdateEquip(Player player)
        {
            Technician modPlayer = player.GetModPlayer<Technician>();
            //if (IsShield && player.GetModPlayer<Technician>().shield < Shield)
            //    player.GetModPlayer<Technician>().shield = Shield;
            for (int i = 0; i < ModSlots; i++)
            {
                Item? it = modules[i];
                if (it == null || it.IsAir || it.ModItem == null) continue;
                it.ModItem.UpdateEquip(player);
                if (IsShield && Item.wornArmor)
                {
                    modPlayer.ShieldChanges += (it.ModItem as Chip).UpdateShieldStats;
                }
            }
            if (IsShield && Item.wornArmor)
            {
                modPlayer.AddShield(Shield, itemDamageType);
            }
            PostUpdateEquip(player);
        }

        //Inherited UpdateAccessory
        protected virtual void PostUpdateAccessory(Player player, bool hideVisual) { }
        public sealed override void UpdateAccessory(Player player, bool hideVisual)
        {
            Technician modPlayer = player.GetModPlayer<Technician>();
            for (int i = 0; i < ModSlots; i++)
            {
                Item? it = modules[i];
                if (it == null || it.IsAir || it.ModItem == null) continue;
                it.ModItem.UpdateAccessory(player, hideVisual);
                if(IsShield)
                {
                    modPlayer.ShieldChanges += (it.ModItem as Chip).UpdateShieldStats;
                }
            }
            if(IsShield)
            {
                modPlayer.AddShield(Shield, itemDamageType);
            }
            PostUpdateAccessory(player, hideVisual);
        }

        public bool SwapModule(int index, ref Item other)
        {
            /*
            if (other.Item.DamageType != Item.DamageType)
                return false;
            Terraria.Utils.Swap<Chip>(ref modules[index].ModItem as Chip, ref other);
            return true;*/
            ref Item item = ref modules[index];
            if ((other == null || other.IsAir) && (item == null || item.IsAir))
                return false;
            if (other.type == item.type)
                return false;

            if (other.IsAir)
            {
                other = item.Clone();
                item.TurnToAir();
                //itemTex = null;
            }
            else if (item.IsAir)
            {
                //Utils.Swap(ref item, ref Main.mouseItem);
                item = other.Clone();
                other.TurnToAir();
                //itemTex = null;
            }
            else
            {
                Item inter = item.Clone();
                item = other.Clone();
                item = inter.Clone();
                //itemTex = null;
            }
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            for (int i = 0; i < ModSlots; i++)
            {
                Item? it = modules[i];
                if (it == null || it.IsAir || it.ModItem == null) continue;
                (it.ModItem as Chip)?.ModifyProj(p);
            }
            return false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            for (int i = 0; i < ModSlots; i++)
            {
                Item? it = modules[i];
                if (it == null || it.IsAir || it.ModItem == null) continue;
                it.ModItem?.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
            }
        }

        public ref Item GetChip(int index)
        {
            return ref modules[index%ModSlots];
        }

        public virtual bool AllowChip(Item item, int index) { return true; }

        public override void SaveData(TagCompound tag)
        {
            for(int i = 0; i < ModSlots; i++)
            {
                tag.Add("chip::" + i, GetChip(i));
            }
        }
        public override void LoadData(TagCompound tag)
        {
            for (int i = 0; i < ModSlots; i++)
            {
                if (!tag.ContainsKey("chip::" + i))
                {
                    modules[i] = new Item();
                }
                modules[i] = tag.Get<Item>("chip::" + i);
            }
        }
    }

    [Autoload(false)]
    public class TechItem_Default : TechItem
    {
        protected override void SafeSetDefaults()
        {
            itemDamageType = ModContent.GetInstance<Generic>();
        }
        //public override TechDamageClass ItemDamageType => ModContent.GetInstance<Generic>();
    }

    public class Shield : ILoadable
    {
        public virtual string Texture => "Onyxia/Assets/ForceProjector";

        public uint shieldsMax;
        public uint cooldownTime;
        public float regen;
        public bool ignoreKb;
        public float damageResist;
        public float priority = .5f;

        public void Init()
        {
            shieldsMax = 1;
            cooldownTime = 1;
            regen = 0;
            ignoreKb = false;
            damageResist = 0;
            priority = -1;
            SetDefaults();
        }
        public virtual void SetDefaults() { }
        public Shield() : base()
        {
            Init();
        }
        public Shield Clone()
        {
            Shield s = (Shield)MemberwiseClone();
            return s;
        }

        protected virtual void OnHit(Player player, int damage) { }
        protected virtual void OnBreak(Player player, int damage) { }
        protected virtual void UpdateActive(Player player) { }
        protected virtual void OnCooldown(Player player) { }
        public void HitShield(Player player, int damage) { OnHit(player, damage); }
        public void BreakShield(Player player, int damage) { OnBreak(player, damage); }
        public void UpdateShieldActive(Player player) { UpdateActive(player); }
        public void RefreshShield(Player player) { OnCooldown(player); }

        public static bool operator >(Shield a, Shield b)
        {
            if (a == null)
                return false;
            if (b == null)
                return true;
            float priorityCheck = (a.priority - b.priority);
            float shieldCapCheck = (2f * (a.shieldsMax - b.shieldsMax)) / (float)(a.shieldsMax + b.shieldsMax);
            float cdCheck = (2f * (b.cooldownTime - a.cooldownTime)) / (float)(b.cooldownTime + a.cooldownTime);
            return (priorityCheck + shieldCapCheck + cdCheck) > 0;
        }
        public static bool operator <(Shield a, Shield b)
        {
            if (b == null)
                return false;
            if (a == null)
                return true;
            float priorityCheck = (a.priority - b.priority);
            float shieldCapCheck = (2f*(a.shieldsMax - b.shieldsMax)) / (float)(a.shieldsMax + b.shieldsMax);
            float cdCheck = (2f*(b.cooldownTime - a.cooldownTime)) / (float)(b.cooldownTime + a.cooldownTime);
            return (priorityCheck + shieldCapCheck + cdCheck) < 0;
        }
        /*public static bool operator ==(Shield a, Shield b)
        {
            //if (a == null || b == null) return false;
            return ((a.priority == b.priority) && (a.shieldsMax == b.shieldsMax) && (a.cooldownTime == b.cooldownTime) && (a.damageResist == b.damageResist) && (a.ignoreKb == b.ignoreKb) && (a.regen == b.regen));
        }
        public static bool operator !=(Shield a, Shield b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            if (!obj.GetType().IsSubclassOf(GetType())) return false;
            return (Shield)(obj) == (Shield)this;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
            //return (int)((this.cooldownTime ^ this.shieldsMax) * ((int)(10 * this.regen) ^ (int)(this.damageResist * 100)) * (this.ignoreKb ? -1 : 1));
        }*/
        public void Load(Mod mod)
        {
            //Init();
        }
        public void Unload()
        {
            shieldsMax = 0;
            cooldownTime = 0;
            regen = 0;
            ignoreKb = false;
            damageResist = 0;
            priority = -1;
        }
    }

    public class ShieldLoader : ILoader
    {
        public static Shield GetShield<T>() where T : Shield
        {
            return (Shield)ModContent.GetInstance<T>();
        }
    }
}
