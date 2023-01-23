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
using Terraria.GameInput;

namespace Onyxia.Content.DamageClasses.Technician
{
    public class Technician : ModPlayer
    {
        public delegate void ShieldStats(ref uint shieldMax, ref uint cooldownMax, ref float regen, ref bool? knockback);
        public ShieldStats ShieldChanges;

        /*#region DamageType
        public bool hasCoreBuff = false;
        private Core _coreItem;
        public Core CoreItem
        {
            get => _coreItem;
            set
            {
                if (_coreItem != null)
                {
                    Main.NewText(Player.QuickSpawnItemDirect(Player.GetSource_ItemUse(value.Item), _coreItem.Item.type, 1).type);
                }
                _coreItem = value;
            }
        }
        public TechDamageClass DamageType => CoreItem!=null ? CoreItem.DamageClass : ModContent.GetInstance<Generic>();
        #endregion*/

        /*public delegate void ShieldBreakEvent(Player player, int damage, int direction);
        public ShieldBreakEvent Event_OnShieldBreak;

        public delegate void ShieldDamageEvent(Player player, Entity ent, int damage);
        public ShieldDamageEvent Event_OnShieldDamage;

        public delegate void ShieldUpdateEvent(Player player);
        public ShieldUpdateEvent Event_OnShieldRefresh;
        public ShieldUpdateEvent Event_ShieldUpdate;*/

        public class ShieldLayer : PlayerDrawLayer
        {
            private ReLogic.Content.Asset<Texture2D> Projector_Texture;
            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                Technician modPlayer = drawInfo.drawPlayer.GetModPlayer<Technician>();
                if (modPlayer.ShieldDisabled || modPlayer.shield == null)
                    return;
                string newText = modPlayer.shield.Texture;
                if (Projector_Texture == null || Projector_Texture.Value.Name != newText)
                    Projector_Texture = ModContent.Request<Texture2D>(newText);
                Color drawColor = drawInfo.drawPlayer.GetModPlayer<Technician>().shieldType.DamageColour;
                Point pos = (drawInfo.drawPlayer.MountedCenter - Main.screenPosition).ToPoint() - new Point(32, (int)(32f+drawInfo.drawPlayer.gfxOffY));
                drawInfo.DrawDataCache.Add(new DrawData(Projector_Texture.Value, new Rectangle(pos.X, pos.Y, 64, 64), null, drawColor, 0f, Vector2.Zero, SpriteEffects.None, 0));
            }
            public override Position GetDefaultPosition()
            {
                return new AfterParent(PlayerDrawLayers.SolarShield);
            }
            public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
            {
                return drawInfo.drawPlayer.GetModPlayer<Technician>().ShieldStrength > 0;
            }
        }

        /*public int statShieldsMax;
        public int statShieldCooldown;
        public float shieldRegen;
        public float statShieldDamageResist;
        public bool noKnockback;

        public int statShields;
        private int shieldCooldown;*/

        public override void Initialize()
        {
            //_shieldStats = ShieldLoader.GetShield<Shield>();
            shield = null;
        }

        public bool ShieldDisabled
        {
            get;
            private set;
        } = true;
        public uint statShields = 1;
        public uint statShieldCooldown = 1;

        private Shield shield;

        private uint _shMaxInc = 0;
        private uint _cdTimeInc = 0;
        private float _shReg = 0;
        /// <summary>
        /// Shield Ignore Knockback
        /// </summary>
        private bool? _shIgnKB = null;

        public uint ShieldMax => shield.shieldsMax + _shMaxInc;
        public uint ShieldCD => shield.cooldownTime + _cdTimeInc;
        public float ShieldRegen => shield.regen + _shReg;
        public bool ShieldKB
        {
            get
            {
                if (_shIgnKB.HasValue)
                    return _shIgnKB.Value;
                return shield.ignoreKb || Player.noKnockback;
            }
        }

        public TechDamageClass shieldType = ModContent.GetInstance<Generic>();

        //public bool ShieldActive => statShieldsMax > 0 && statShields > 0;
        public bool ShieldActive => !ShieldDisabled && statShields > 0;
        public bool hasShield;
        //public float ShieldCooldown => 1f-((float)shieldCooldown / (float)statShieldCooldown);
        public float ShieldCooldown => ShieldActive ? MathHelper.Clamp(1f - ((float)statShieldCooldown / (float)(ShieldCD)), 0, 1) : 0;
        public float ShieldStrength => hasShield ? (float)statShields / (float)(ShieldMax) : 0;

        public override void ResetEffects()
        {
            ShieldDisabled = true;
            hasShield = false;
            _shMaxInc = 0;
            _cdTimeInc = 0;
            _shReg = 0;
            _shIgnKB = null;
        }
        public override void PostUpdateEquips()
        {
            if (!ShieldDisabled && hasShield)
            {
                if (ShieldChanges != null)
                {
                    ShieldChanges.Invoke(ref _shMaxInc, ref _cdTimeInc, ref _shReg, ref _shIgnKB);
                    ShieldChanges = null;
                }
            }
        }

        public override void PostUpdate()
        {
            //Main.NewText(shieldType.ClassName.GetDefault());
            //CoreItem.UpdateActive(Player);

            /*if (shieldCooldown > 0)
                shieldCooldown--;
            if(shieldCooldown == 0)
            {
                if (Event_OnShieldRefresh != null)
                    Event_OnShieldRefresh.Invoke(Player);
                shieldCooldown = -1;
                statShields = statShieldsMax;
            }
            if(ShieldActive)
            {
                if (Event_ShieldUpdate != null)
                    Event_ShieldUpdate.Invoke(Player);
            }*/

            if (!ShieldDisabled && hasShield)
            {
                if (statShields > ShieldMax)
                    statShields = ShieldMax;
                if (statShieldCooldown == 0 && statShields < ShieldMax)
                    statShieldCooldown = ShieldCD;
                if (statShieldCooldown > 1)
                    statShieldCooldown--;
                else if(statShieldCooldown > 0)
                {
                    shield.RefreshShield(this.Player);
                    statShields = ShieldMax;
                    statShieldCooldown = 0;
                }
                if (ShieldActive)
                {
                    shield.UpdateShieldActive(this.Player);
                }
            }
            //if (!hasCoreBuff && _coreItem != null)
            //    CoreItem = null;
        }
        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            /*if(ShieldActive)
            {
                damage -= (int)(damage * statShieldDamageResist);
                if (Event_OnShieldDamage != null)
                    Event_OnShieldDamage.Invoke(Player, npc, damage);
            }*/
            if(ShieldActive)
            {
                damage -= (int)(damage * shield.damageResist);
                shield.HitShield(Player, damage);
            }
        }
        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            /*if (ShieldActive)
            {
                damage -= (int)(damage * statShieldDamageResist);
                if (Event_OnShieldDamage != null)
                    Event_OnShieldDamage.Invoke(Player, proj, damage);
            }*/

            if(ShieldActive)
            {
                damage -= (int)(damage * shield.damageResist);
                shield.HitShield(Player, damage);
            }
        }
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            bool basePreHurt = base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
            if (Player.immune) return false;
            if (basePreHurt)
            {
                /*if (shieldCooldown > 0)
                    shieldCooldown = (int)MathHelper.Lerp(shieldCooldown, statShieldCooldown, .8f);
                else
                    shieldCooldown = statShieldCooldown;*/
                //uint cdTime = HasShields ? shield.cooldownTime : 600;
                if (!ShieldDisabled && hasShield)
                {
                    //Main.NewText(statShields + " , " + ShieldMax);
                    if (statShieldCooldown > 0)
                        statShieldCooldown = (uint)MathHelper.Lerp(statShieldCooldown, ShieldCD, .8f);
                    else
                        statShieldCooldown = ShieldCD;


                    if (ShieldActive)
                    {
                        //statShields -= damage;
                        statShields = (uint)MathHelper.Clamp((statShields - damage), 0, ShieldMax);
                        CombatText.NewText(Player.getRect(), shieldType.DamageColour, damage);
                        /*if (statShields <= 0)
                        {
                            if (Event_OnShieldBreak != null)
                                Event_OnShieldBreak.Invoke(Player, damage, hitDirection);
                            statShields = -1;
                        }
                        if (!noKnockback && !Player.noKnockback)
                        {
                            if(hitDirection != 0)
                                Player.velocity = new Vector2(hitDirection * 4.4f, -4.4f);
                        }*/
                        if (statShields <= 0)
                        {
                            shield.BreakShield(Player, damage);
                            statShields = 0;
                        }
                        if (!ShieldKB)
                        {
                            if (hitDirection != 0)
                                Player.velocity = new Vector2(hitDirection * 4.4f, -4.4f);
                        }
                        Player.immuneTime = 20;
                        Player.immune = true;
                        return false;
                    }
                }
            }
            return basePreHurt;
        }

        //Keybinds
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            
            //if(triggersSet.Inventory)
            if (OnyxiaSystem.TinkerKeybind.JustPressed)
            {
                //Main.NewText(OnyxiaSystem.TinkerStateActive);
                if (!OnyxiaSystem.TinkerStateActive)
                {
                    Item item = Main.HoverItem;
                    if (!item.IsAir && item.ModItem != null && item.ModItem.GetType().IsSubclassOf(typeof(TechItem)))
                    {
                        OnyxiaSystem.OpenTinkerUI(item.ModItem as TechItem);
                    }
                }
                else
                {
                    OnyxiaSystem.CloseTinkerUI();
                }
            }
            
        }

        public static Technician GetPlayer(Player player) => player.GetModPlayer<Technician>();
        public void AddShield(Shield s, TechDamageClass type, bool Override = false)
        {
            bool CheckShieldTypes()
            {
                if (shield == null) return false;
                return shield.GetType().Equals(s.GetType());
            }
            bool shEqual = CheckShieldTypes();
            //Main.NewText(shieldType.ClassName.GetDefault() + " : " + type.ClassName.GetDefault());
            /*if(ShieldDisabled && s == shield)
            {
                ShieldDisabled = true;
                return;
            }*/
            //ShieldDisabled = false;
            ShieldDisabled = false;
            hasShield = true;
            shieldType = type;
            if (s > shield || Override || (!hasShield && !shEqual))
            {
                if (!shEqual)
                {
                    //Main.NewText("Shield");
                    shield = s.Clone();
                    statShieldCooldown = (uint)(ShieldCooldown * ShieldCD);
                    if (statShieldCooldown < 0)
                        statShieldCooldown = 1;
                    statShields = (uint)(ShieldStrength * ShieldMax);
                }
                //shield = s.Clone();
                //shieldType = type;
                return;
            }
            //Main.NewText("None");
        }
    }

    /*public abstract class ForceProjector : TechItem
    {
        protected int shieldsMax;
        protected int shieldCooldown;
        protected float shieldRegen;
        protected float shieldDmgResist;
        protected bool disableKnockback;

        protected void ApplyShieldStats(Player player)
        {
            Technician modPlayer = player.GetModPlayer<Technician>();
            modPlayer.statShieldsMax = shieldsMax;
            modPlayer.statShieldCooldown = shieldCooldown;
            modPlayer.shieldRegen = shieldRegen;
            modPlayer.statShieldDamageResist = shieldDmgResist;
            modPlayer.noKnockback = disableKnockback;

            modPlayer.Event_OnShieldBreak = ShieldBreak;
            modPlayer.Event_OnShieldDamage = ShieldDamage;
            modPlayer.Event_OnShieldRefresh = ShieldRefresh;
            modPlayer.Event_ShieldUpdate = ShieldUpdate;
        }
        protected override void PostUpdateAccessory(Player player, bool hideVisual)
        {
            ApplyShieldStats(player);
            //UpdateInAccessory(player, hideVisual);
        }

        public virtual void ShieldBreak(Player player, int damage, int direction) { }

        public virtual void ShieldDamage(Player player, Entity ent, int damage) { }

        public virtual void ShieldRefresh(Player player) { }
        public virtual void ShieldUpdate(Player player) { }
    }*/

    public class ShieldState : UIState
    {
        public override void OnInitialize()
        {
            frame = ModContent.Request<Texture2D>("Onyxia/Assets/ShieldFrame");
        }

        float shieldPercent = 0;
        float cooldownPercent = 0;
        Color colour = Color.White;
        private ReLogic.Content.Asset<Texture2D> frame;
        public override void Update(GameTime gameTime)
        {
            Technician modPlayer = Main.LocalPlayer.GetModPlayer<Technician>();
            shieldPercent = modPlayer.ShieldActive ? modPlayer.ShieldStrength : 0;
            cooldownPercent = modPlayer.ShieldCooldown >= 1 ? 0 : modPlayer.ShieldCooldown;
            colour = modPlayer.shieldType.DamageColour;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Main.LocalPlayer.GetModPlayer<Technician>().hasShield)
                return;
            Rectangle drawRect = new Rectangle();
            drawRect.Location = new Point(Main.screenWidth - 91, 101 + (int)(24f * (1 - shieldPercent)));
            drawRect.Height = (int)(24f * shieldPercent);
            drawRect.Width = 22;
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, drawRect, colour);
            colour = new Color((colour.ToVector3() + Color.WhiteSmoke.ToVector3()) * .5f);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(drawRect.X + 1, 102 + (int)(20f * (1 - cooldownPercent)), 3, (int)(20f * cooldownPercent)), colour);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(drawRect.X + 18, 102 + (int)(20f * (1 - cooldownPercent)), 3, (int)(20f * cooldownPercent)), colour);
            if (frame == null)
            {
                frame = ModContent.Request<Texture2D>("Onyxia/Assets/ShieldFrame");
            }
            spriteBatch.Draw(frame?.Value, new Vector2(Main.screenWidth - 96f, 96), Color.White);
        }
    }
    
    /*public class DebugCore : Core
    {
        public override string Texture => "Terraria/Images/Item_2";
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("+5 Blast Damage");
        }
        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = 4;
            DamageClass = ModContent.GetInstance<Blast>();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(ModContent.GetInstance<Blast>()).Flat += 5;
        }
    }
    public class CoreBuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Oiled;
        public override string Name => "Core Stats:";
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            Player player = Main.LocalPlayer;
            Technician modPlayer = player.GetModPlayer<Technician>();
            tip = modPlayer.CoreItem.Tooltip.GetDefault() + "\nDisable this buff to return your core.";
        }
    }*/
    //6 Damage Classes
    //Blast (Melee Damage)
        //Short Shotgun-Type attack
    //Melee-Tank-Hybrid
        //Charged explosion, centered on player hand
    //Mage-Damage-Hybrid
        //Laser, "burn" damage (constant dps)
    //Mage-Heal-Hybrid
        //Arcing-Type blast
    //Ranged-Hybrid
        //Firing array
    //Summon-Hybrid
        //Summons drones to do work
}