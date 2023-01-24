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

        public override void Initialize()
        {
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

        public bool ShieldActive => !ShieldDisabled && statShields > 0;
        public bool hasShield;
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
        }
        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if(ShieldActive)
            {
                damage -= (int)(damage * shield.damageResist);
                shield.HitShield(Player, damage);
            }
        }
        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
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
                if (!ShieldDisabled && hasShield)
                {
                    if (statShieldCooldown > 0)
                        statShieldCooldown = (uint)MathHelper.Lerp(statShieldCooldown, ShieldCD, .8f);
                    else
                        statShieldCooldown = ShieldCD;


                    if (ShieldActive)
                    {
                        //statShields -= damage;
                        statShields = (uint)MathHelper.Clamp((statShields - damage), 0, ShieldMax);
                        CombatText.NewText(Player.getRect(), shieldType.DamageColour, damage);
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
            if (OnyxiaSystem.TinkerKeybind.JustPressed)
            {
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
            ShieldDisabled = false;
            hasShield = true;
            shieldType = type;
            if (s > shield || Override || (!hasShield && !shEqual))
            {
                if (!shEqual)
                {
                    shield = s.Clone();
                    statShieldCooldown = (uint)(ShieldCooldown * ShieldCD);
                    if (statShieldCooldown < 0)
                        statShieldCooldown = 1;
                    statShields = (uint)(ShieldStrength * ShieldMax);
                }
                return;
            }
        }
    }

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

    //6 Damage Classes
    //Blast (Melee Damage)
        //Short Shotgun-Type attack
    //Melee-Tank-Hybrid
        //Charged explosion, centered on player hand
    //Mage-Damage-Hybrid
        //Laser, "burn" damage (constant dps + held projectile?)
    //Mage-Heal-Hybrid
        //Arcing-Type blast
    //Ranged-Hybrid
        //Firing array
    //Summon-Hybrid
        //Summons drones
}