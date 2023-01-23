using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.Audio;

namespace Onyxia.Content
{
    public class OnyxiaPlayer : ModPlayer
    {
        //public bool bloodLotus;
        //public int bloodLotusCounter;
        //public int bloodLotusHealth;

        public override void ResetEffects()
        {
            dashData = DashData.Zero;

            if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[(int)Direction.DOWN] < 15)
                dashDirection = Direction.DOWN;
            else if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[(int)Direction.UP] < 15)
                dashDirection = Direction.UP;
            else if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[(int)Direction.RIGHT] < 15)
                dashDirection = Direction.RIGHT;
            else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[(int)Direction.LEFT] < 15)
                dashDirection = Direction.LEFT;
            else
                dashDirection = Direction.NONE;

            //bloodLotus = false;
        }
        public void OnHitAnything(Entity target, int damage)
        {
            /*if (bloodLotus)
            {
                bloodLotusCounter += (int)MathHelper.Clamp(damage / 30, 1, 30);
                if (bloodLotusCounter > 30)
                {
                    bloodLotusHealth = (int)MathHelper.Clamp(bloodLotusHealth + bloodLotusCounter, 0, 90);
                    bloodLotusCounter = 0;
                }
            }*/
        }
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            OnHitAnything(target, damage);
            base.OnHitNPC(item, target, damage, knockback, crit);
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            OnHitAnything(target, damage);
            base.OnHitNPCWithProj(proj, target, damage, knockback, crit);
        }
        public override void OnHitPvp(Item item, Player target, int damage, bool crit)
        {
            OnHitAnything(target, damage);
            base.OnHitPvp(item, target, damage, crit);
        }
        public override void OnHitPvpWithProj(Projectile proj, Player target, int damage, bool crit)
        {
            OnHitAnything(target, damage);
            base.OnHitPvpWithProj(proj, target, damage, crit);
        }

        public int useStyleData;
        public object data;
        public bool isItemDelay;

        public override void PostItemCheck()
        {
            if (Player.ItemAnimationEndingOrEnded)
            {
                if (isItemDelay)
                {
                    isItemDelay = false;
                }
                else
                {
                    isItemDelay = true;
                }
            }
            if (Player.itemAnimation <= 0 || Player.HeldItem.reuseDelay == 0)
                isItemDelay = false;
        }


        private DashData dashData;

        private enum Direction : int
        {
            NONE = -1,
            DOWN = 0,
            UP = 1,
            RIGHT = 2,
            LEFT = 3,
        }

        private Direction dashDirection;
        private int dashTimer;
        private int dashCDTimer;
        public override void PreUpdateMovement()
        {
            if((dashData.speed > 0) && (Player.dashType == 0) && (!Player.mount.Active) && (dashCDTimer <= 0) && (dashDirection != Direction.NONE))// && !Player.CCed)
            {
                bool dashStarted = true;
                Vector2 vel = Player.velocity;
                switch(dashDirection)
                {
                    case Direction.LEFT when Player.velocity.X > -dashData.speed:
                    case Direction.RIGHT when Player.velocity.X < dashData.speed:
                        vel.X = dashData.speed * ((dashDirection == Direction.RIGHT) ? 1 : -1);
                        break;
                    case Direction.DOWN when Player.velocity.Y < dashData.speed && dashData.anyDirection:
                    case Direction.UP when Player.velocity.Y > -dashData.speed && dashData.anyDirection:
                        vel.Y = dashData.speed * ((dashDirection == Direction.UP) ? -1.23f : 1f);
                        break;
                    default:
                        dashStarted = false;
                        break;
                }
                
                if (dashStarted)
                {
                    dashTimer = dashData.time;
                    dashCDTimer = dashData.time + dashData.cooldown;
                    Player.velocity = vel;
                }
            }
            if (dashTimer > 0)
            {
                if (dashData.damage > 0)
                {
                    Rectangle rect = Player.getRect();
                    for(int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if(Player.CanHit(npc) && npc.getRect().Intersects(rect))
                        {
                            if(npc.friendly)
                            dashTimer = 0;
                            npc.StrikeNPC(dashData.damage, dashData.speed, Player.velocity.X > 0 ? 1 : -1);
                            if(!Player.noKnockback)
                            {
                                Player.velocity.Y -= dashData.speed * .4f;
                                Player.velocity.X = Player.direction * dashData.speed * -.5f;
                            }
                            break;
                        }
                    }
                }
                dashTimer--;
            }
            if (dashCDTimer > 0)
                dashCDTimer--;
        }

        public bool SetDashStats(float speed, int effectDuration, float priority = 1, int damage = 0, bool omniDirectional = false)
        {
            dashData = new DashData(speed, effectDuration, priority, damage, omniDirectional);
            return true;
        }
    }
    struct DashData
    {
        public int time, cooldown, damage;
        public float speed, priority;
        public bool anyDirection;
        public DashData(float speed, int duration, float priority, int damage = 0, bool anyDirection = false)
        {
            this.speed = speed;
            this.time = duration;
            this.priority = priority;
            this.cooldown = (int)(duration * .4f);
            this.anyDirection = anyDirection;
            this.damage = damage;
        }
        public static DashData Zero
        {
            get
            {
                DashData data = new DashData();
                data.time = 0;
                data.cooldown = 0;
                data.damage = 0;
                data.speed = 0;
                data.priority = 0;
                data.anyDirection = false;
                return data;
            }
        }
    }
}