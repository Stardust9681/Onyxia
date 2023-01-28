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
using ReLogic.Content;

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
                Color drawColor = drawInfo.drawPlayer.GetModPlayer<Technician>().coreColour;
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
        public Color coreColour;

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

        //public TechDamageClass shieldType = ModContent.GetInstance<Generic>();

        public bool ShieldActive => !ShieldDisabled && statShields > 0;
        public bool hasShield;
        public float ShieldCooldown => hasShield ? MathHelper.Clamp(1f - ((float)statShieldCooldown / (float)(ShieldCD)), 0, 1) : 0;
        public float ShieldStrength => hasShield ? (float)statShields / (float)(ShieldMax) : 0;

        public int statEnergy;
        public int statEnergyMax = 100;
        public float EnergyPercent => (float)statEnergy / (float)statEnergyMax;

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
                //Main.NewText(statShieldCooldown + " : " + (ShieldCooldown >= 1 ? 0 : ShieldCooldown));
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
                        CombatText.NewText(Player.getRect(), coreColour, damage);
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
        public void AddShield(Shield s, Color colour, bool Override = false)
        {
            bool CheckShieldTypes()
            {
                if (shield == null) return false;
                return shield.GetType().Equals(s.GetType());
            }
            bool shEqual = CheckShieldTypes();
            ShieldDisabled = false;
            hasShield = true;
            coreColour = colour;
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
        private Asset<Texture2D> frameLeft;
        private Asset<Texture2D> frameMid;
        private Asset<Texture2D> frameRight;
        private Asset<Texture2D> resourceBar;
        private Asset<Texture2D> cooldownBar;
        public override void OnInitialize()
        {
            frameLeft = ModContent.Request<Texture2D>("Onyxia/Assets/ShieldResourceFrameLeft");
            frameRight = ModContent.Request<Texture2D>("Onyxia/Assets/ShieldResourceFrameRight");
            frameMid = ModContent.Request<Texture2D>("Onyxia/Assets/ShieldResourceFrameMid");
            resourceBar = ModContent.Request<Texture2D>("Onyxia/Assets/ShieldResourceBar");
            cooldownBar = ModContent.Request<Texture2D>("Onyxia/Assets/ShieldResourceBar_Cooldown");
        }

        float shieldPercent = 0;
        float cooldownPercent = 0;
        bool hovering;
        public override bool ContainsPoint(Vector2 point)
        {
            float scale = Main.UIScale;
            return new Rectangle((int)(Main.screenWidth * MathHelper.Clamp((.75f / scale), 0f, .85f) - (((2 * 46) - 80) * scale)), 16, (int)(154 * scale), (int)(40 * scale)).Contains(point.ToPoint());
        }
        public override void Update(GameTime gameTime)
        {
            Technician modPlayer = Main.LocalPlayer.GetModPlayer<Technician>();
            if (!modPlayer.hasShield) return;
            shieldPercent = modPlayer.ShieldActive ? modPlayer.ShieldStrength : 0;
            cooldownPercent = modPlayer.ShieldCooldown >= 1 ? 0 : modPlayer.ShieldCooldown;
            if(ContainsPoint(Main.MouseScreen))
            {
                hovering = true;
                //Main.hoverItemName = modPlayer.statShields + "/" + modPlayer.ShieldMax;
                //Main.mouseText = true;
                if (Main.MouseScreen.Y > 16 + (13*Main.UIScale))
                    Main.instance.MouseText(modPlayer.statShields + "/" + modPlayer.ShieldMax);
                else
                    Main.instance.MouseText((int)(modPlayer.ShieldCooldown * 100) + "%");
            }
            else
            {
                hovering = false;
                //Main.mouseText = false;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw nothing if the player does not currently have a shield (duh)
            if (!Main.LocalPlayer.GetModPlayer<Technician>().hasShield)
                return;
            //Make sure no null assets are being grabbed or used.
            if(frameLeft == null || frameLeft.Value == null)
                frameLeft = ModContent.Request<Texture2D>("Onyxia/Assets/ShieldResourceFrameLeft");
            if(frameRight == null || frameRight.Value == null)
                frameRight = ModContent.Request<Texture2D>("Onyxia/Assets/ShieldResourceFrameRight");
            if(frameMid == null || frameMid.Value == null)
                frameMid = ModContent.Request<Texture2D>("Onyxia/Assets/ShieldResourceFrameMid");
            if(resourceBar == null || resourceBar.Value == null)
                resourceBar = ModContent.Request<Texture2D>("Onyxia/Assets/ShieldResourceBar");
            if(cooldownBar == null || cooldownBar.Value == null)
                cooldownBar = ModContent.Request<Texture2D>("Onyxia/Assets/ShieldResourceBar_Cooldown");

            //Cache UIScale (idk what goes into it, might be an expensive calculation)
            float scale = Main.UIScale;
            //Number of segments, leave at 2, because bar textures have not been made to be scaled yet)
            int s = 2;
            //Right side of where this resource is drawn, anchor point more or less
            float right = Main.screenWidth * MathHelper.Clamp((.75f/scale), 0f, .85f);
            //Calculate position from anchor point
            float left = right - (((s * 46) - 80)*scale);
            //More legible and able to be modified than constant 16
            float top = 16;
            //Draw frame left
            Vector2 drawPos = new Vector2(left, top);
            spriteBatch.Draw(frameLeft.Value, drawPos, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            //Adjust position to start frame mid drawing (multiple segments)
            drawPos.X += (34f * scale);
            while(s > 0)
            {
                spriteBatch.Draw(frameMid.Value, drawPos, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                drawPos.X += (46f * scale);
                s--;
            }
            //Draw frame right
            spriteBatch.Draw(frameRight.Value, drawPos, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

            #region Shield Stat
            //Don't draw anything if there's nothing to be drawn.
            if (shieldPercent > 0)
            {
                //Width of texture to be drawn
                float rssWidth = (shieldPercent * 118f);
                //The leftmost side of where this bar is being drawn from
                //Draws from right side as anchor point, more maths being performed
                float rssLeft = left + ((118f - rssWidth)+26)*scale;
                //Destination (left, top, right-left (width), bottom-top (height))
                Rectangle destRect = new Rectangle((int)rssLeft, (int)(top + (16 * scale)), (int)(rssWidth*scale), (int)(14 * scale));
                //Source (not drawing full texture at scale)
                Rectangle sourceRect = new Rectangle((int)(118-rssWidth), 0, (int)(rssWidth), 14);
                spriteBatch.Draw(resourceBar.Value, destRect, sourceRect, Color.White);
            }
            #endregion
            #region Shield Cooldown
            //See comments from above region.
            //Same principle, different stat, slightly different location.
            if (cooldownPercent > 0 && cooldownPercent < 1)
            {
                float rssWidth = (cooldownPercent * 102f);
                float rssLeft = left + ((102 - rssWidth) + 28) * scale;
                Rectangle destRect = new Rectangle((int)rssLeft, (int)(top + (6 * scale)), (int)(rssWidth * scale), (int)(6 * scale));
                Rectangle sourceRect = new Rectangle((int)(102-rssWidth), 0, (int)(rssWidth), 6);
                spriteBatch.Draw(resourceBar.Value, destRect, sourceRect, Color.White);
            }
            #endregion
        }
    }

    public class EnergyState : UIState
    {
        Asset<Texture2D> frame;
        Asset<Texture2D> bar;
        float energyPercent = -1;
        float energyTarget;
        bool hovering;
        public override void OnInitialize()
        {
            frame = ModContent.Request<Texture2D>("Onyxia/Assets/EnergyResourceFrame");
            bar = ModContent.Request<Texture2D>("Onyxia/Assets/EnergyResourceBar");
        }
        public override void Update(GameTime gameTime)
        {
            if (!Main.LocalPlayer.HeldItem.ModItem.GetType().IsSubclassOf(typeof(TechItem)))
                return;
            Technician modPlayer = Main.LocalPlayer.GetModPlayer<Technician>();
            float currentValue = modPlayer.EnergyPercent;
            if (energyPercent == -1)
                energyPercent = currentValue;
            energyTarget = currentValue;
            if(MathF.Abs(energyTarget - energyPercent) < .01f)
            {
                energyPercent = currentValue;
                //modPlayer.statEnergy = Main.rand.Next(100);
            }
            else
            {
                energyPercent = MathHelper.Lerp(energyPercent, energyTarget, .03f);
            }
            if (ContainsPoint(Main.MouseScreen))
            {
                hovering = true;
                //Main.hoverItemName = modPlayer.statEnergy + "/" + modPlayer.statEnergyMax;
                //Main.mouseText = true;
                Main.instance.MouseText(modPlayer.statEnergy + "/" + modPlayer.statEnergyMax);
            }
            else
            {
                hovering = false;
                //Main.mouseText = false;
            }
        }
        public override bool ContainsPoint(Vector2 point)
        {
            float scale = Main.UIScale;
            return new Rectangle(Main.screenWidth / 2 - (int)(64f * scale), Main.screenHeight / 2 - (int)(80f * scale), (int)(128f * scale), (int)(34f * scale)).Contains(point.ToPoint());
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Main.LocalPlayer.HeldItem.ModItem.GetType().IsSubclassOf(typeof(TechItem)))
                return;

            if (frame == null || frame.Value == null)
                frame = ModContent.Request<Texture2D>("Onyxia/Assets/EnergyResourceFrame");
            if (bar == null || bar.Value == null)
                bar = ModContent.Request<Texture2D>("Onyxia/Assets/EnergyResourceBar");

            float scale = Main.UIScale;
            Point drawPos = ((Main.ScreenSize.ToVector2() * .5f) + new Vector2(-64f * scale, -80f * scale)).ToPoint();
            float transparency = hovering ? .25f : 1;
            spriteBatch.Draw(frame.Value, drawPos.ToVector2(), null, Color.White * transparency, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);

            if(energyPercent > 0)
            {
                Rectangle dest;
                Rectangle source;
                drawPos = (drawPos.ToVector2() + (new Vector2(28.5f, 12.5f)) * scale).ToPoint();
                if (energyTarget > energyPercent)
                {
                    dest = new Rectangle((int)drawPos.X, (int)drawPos.Y, (int)(energyTarget * 90f * scale), (int)(12 * scale));
                    source = new Rectangle(0, 0, (int)(energyTarget * 90), 12);
                    spriteBatch.Draw(bar.Value, dest, source, Color.Teal * .4f * transparency);
                }
                dest = new Rectangle((int)drawPos.X, (int)drawPos.Y, (int)(energyPercent * 90f * scale), (int)(12 * scale));
                source = new Rectangle(0, 0, (int)(energyPercent * 90), 12);
                spriteBatch.Draw(bar.Value, dest, source, Color.White * transparency);
                if(energyTarget < energyPercent)
                {
                    dest = new Rectangle((int)(drawPos.X + (energyTarget*90*scale)), (int)drawPos.Y, (int)((energyPercent - energyTarget) * 90f * scale), (int)(12 * scale));
                    source = new Rectangle((int)(energyTarget*90), 0, (int)((energyPercent - energyTarget)*90f), 12);
                    spriteBatch.Draw(bar.Value, dest, source, Color.OrangeRed * .4f * transparency);
                }
            }
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