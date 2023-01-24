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
    //Discussion: ModProjectile Instead?
    //Felt like making these Entities, but Projectiles do already have sync methods and such.
    //Let me know I guess.
    public abstract class EntityPlatform : Entity
    {
        public int TimeLeft
        {
            get;
            private set;
        }
        public int lifeTime;
        public bool jumpThrough;
        public bool fallThrough;
        public bool[] justHit = new bool[2];
        public Vector2 hitDirection;
        public bool affectPlayer;
        public bool affectProj;
        public bool affectNPC;
        public void setup()
        {
            SetDefaults();
            this.active = true;
            TimeLeft = lifeTime;
        }
        public virtual void SetDefaults()
        {
            lifeTime = 0;
            affectPlayer = true;
            affectProj = true;
            affectNPC = true;
            hitDirection = Vector2.Zero;
            jumpThrough = false;
            fallThrough = false;
        }
        public void update()
        {
            if (!active)
                return;
            TimeLeft--;
            if (TimeLeft < 0)
                Kill();
            this.wet = Collision.WetCollision(position, width, height);
            this.wetCount = this.wet ? (byte)MathHelper.Clamp(this.wetCount+1, 0, 255) : (byte)0;
            this.lavaWet = Collision.LavaCollision(position, width, height);
            this.position += this.velocity;
            justHit[1] = justHit[0];
            justHit[0] = false;

            float YSlope1(float inputX)
            {
                return MathF.Abs(inputX - ((this.Right.X + this.Left.X) * .5f)) + this.Bottom.Y - ((this.Right.X - this.Left.X) * .5f);
            }
            float YSlope2(float inputX)
            {
                return -MathF.Abs(inputX - ((this.Right.X + this.Left.X) * .5f)) + this.Top.Y + ((this.Right.X - this.Left.X) * .5f);
            }
            if(affectPlayer)
                for(int i = 0; i < Main.maxPlayers; i++)
            {
                Player entity = Main.player[i];
                if (!entity.active || entity.dead) continue;
                Vector2 centre = entity.MountedCenter;
                float expXTop = YSlope2(centre.X);
                float expXBot = YSlope1(centre.X);
                bool top = centre.Y < expXTop && centre.Y < this.Center.Y;
                bool right = (centre.Y > expXTop && centre.Y < expXBot && entity.position.X > this.Center.X);
                bool left = (centre.Y > expXTop && centre.Y < expXBot && entity.position.X < this.Center.X);
                bool bot = centre.Y > expXBot && centre.Y > this.Center.Y;
                if (!fallThrough && !jumpThrough && left && (entity.Top.Y < this.Bottom.Y && entity.Bottom.Y > this.Top.Y))
                {
                    if (entity.Right.X > this.Left.X && entity.Left.X < this.Right.X && entity.velocity.X > 0)
                    {
                        justHit[0] = true;
                        hitDirection.X = MathHelper.Lerp(hitDirection.X, entity.velocity.X * .267f, .34f);
                        entity.velocity.X = 0;
                        entity.position.X = this.Left.X - entity.width + .1f;
                        if (entity.velocity.Y > 0)
                        {
                            if (entity.spikedBoots == 1)
                            {
                                entity.velocity.Y = MathHelper.Clamp(entity.velocity.Y, -1, 1f);
                                entity.justJumped = false;
                                entity.sliding = true;
                            }
                            else if (entity.spikedBoots > 1)
                            {
                                entity.velocity.Y = 0;
                                entity.justJumped = false;
                                entity.sliding = true;
                            }
                        }
                    }
                }
                if (!fallThrough && !jumpThrough && right && (entity.Top.Y < this.Bottom.Y && entity.Bottom.Y > this.Top.Y))
                {
                    if (entity.Left.X < this.Right.X && entity.Right.X > this.Left.X && entity.velocity.X < 0)
                    {
                        justHit[0] = true;
                        hitDirection.X = MathHelper.Lerp(hitDirection.X, entity.velocity.X * .267f, .34f);
                        entity.velocity.X = 0;
                        entity.position.X = this.Right.X - .1f;
                        if (entity.velocity.Y > 0)
                        {
                            if (entity.spikedBoots == 1)
                            {
                                entity.velocity.Y = MathHelper.Clamp(entity.velocity.Y, -1, 2f);
                                entity.justJumped = false;
                                entity.sliding = true;
                            }
                            else if (entity.spikedBoots > 1)
                            {
                                entity.velocity.Y = 0;
                                entity.justJumped = false;
                                entity.sliding = true;
                            }
                        }
                    }
                }
                if((!fallThrough || (jumpThrough && !entity.controlDown)) && top && (entity.Right.X > this.Left.X && entity.Left.X < this.Right.X))
                {
                    if(entity.Bottom.Y > this.Top.Y && entity.velocity.Y >= 0)
                    {
                        justHit[0] = true;
                        hitDirection.Y = MathHelper.Lerp(hitDirection.Y, entity.velocity.Y * .267f, .34f);
                        entity.velocity.Y = 0;
                        entity.justJumped = false;
                        entity.position.Y = this.Top.Y - entity.height + .1f;
                    }
                }
                if((!jumpThrough || (fallThrough && !entity.controlUp)) && bot && (entity.Right.X > this.Left.X && entity.Left.X < this.Right.X))
                {
                    if (entity.Top.Y < this.Bottom.Y && entity.velocity.Y < 0)
                    {
                        justHit[0] = true;
                        hitDirection.Y = MathHelper.Lerp(hitDirection.Y, entity.velocity.Y * .267f, .34f);
                        entity.velocity.Y = 0;
                        entity.position.Y = this.Bottom.Y + .1f;
                    }
                }
            }
            if(affectNPC)
                for(int i = 0; i < Main.maxNPCs; i++)
            {
                NPC entity = Main.npc[i];
                if (!entity.active) continue;
                Vector2 centre = entity.Center;
                float expXTop = YSlope2(centre.X);
                float expXBot = YSlope1(centre.X);
                bool top = centre.Y < expXTop && centre.Y < this.Center.Y;
                bool right = (centre.Y > expXTop && centre.Y < expXBot && entity.position.X > this.Center.X);
                bool left = (centre.Y > expXTop && centre.Y < expXBot && entity.position.X < this.Center.X);
                bool bot = centre.Y > expXBot && centre.Y > this.Center.Y;
                if (!fallThrough && !jumpThrough && left && (entity.Top.Y < this.Bottom.Y && entity.Bottom.Y > this.Top.Y))
                {
                    if (entity.Right.X > this.Left.X && entity.Left.X < this.Right.X && entity.velocity.X > 0)
                    {
                        justHit[0] = true;
                        hitDirection.X = MathHelper.Lerp(hitDirection.X, entity.velocity.X * .267f, .34f);
                        entity.velocity.X = 0;
                        entity.position.X = this.Left.X - entity.width + .1f;
                    }
                }
                if (!fallThrough && !jumpThrough && right && (entity.Top.Y < this.Bottom.Y && entity.Bottom.Y > this.Top.Y))
                {
                    if (entity.Left.X < this.Right.X && entity.Right.X > this.Left.X && entity.velocity.X < 0)
                    {
                        justHit[0] = true;
                        hitDirection.X = MathHelper.Lerp(hitDirection.X, entity.velocity.X * .267f, .34f);
                        entity.velocity.X = 0;
                        entity.position.X = this.Right.X - .1f;
                    }
                }
                if ((!fallThrough || (jumpThrough && !entity.stairFall)) && top && (entity.Right.X > this.Left.X && entity.Left.X < this.Right.X))
                {
                    if (entity.Bottom.Y > this.Top.Y && entity.velocity.Y >= 0)
                    {
                        justHit[0] = true;
                        hitDirection.Y = MathHelper.Lerp(hitDirection.Y, entity.velocity.Y * .267f, .34f);
                        entity.velocity.Y = 0;
                        entity.position.Y = this.Top.Y - entity.height + .1f;
                    }
                }
                if (!jumpThrough && bot && (entity.Right.X > this.Left.X && entity.Left.X < this.Right.X))
                {
                    if (entity.Top.Y < this.Bottom.Y && entity.velocity.Y < 0)
                    {
                        justHit[0] = true;
                        hitDirection.Y = MathHelper.Lerp(hitDirection.Y, entity.velocity.Y * .267f, .34f);
                        entity.velocity.Y = 0;
                        entity.position.Y = this.Bottom.Y + .1f;
                    }
                }
            }
            if(affectProj)
                for(int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile entity = Main.projectile[i];
                if (!entity.active || !entity.tileCollide) continue;
                Vector2 centre = entity.Center;
                float expXTop = YSlope2(centre.X);
                float expXBot = YSlope1(centre.X);
                bool top = centre.Y < expXTop && centre.Y < this.Center.Y;
                bool right = (centre.Y > expXTop && centre.Y < expXBot && entity.position.X > this.Center.X);
                bool left = (centre.Y > expXTop && centre.Y < expXBot && entity.position.X < this.Center.X);
                bool bot = centre.Y > expXBot && centre.Y > this.Center.Y;
                if (!fallThrough && !jumpThrough && left && (entity.Top.Y < this.Bottom.Y && entity.Bottom.Y > this.Top.Y))
                {
                    if (entity.Right.X > this.Left.X && entity.Left.X < this.Right.X && entity.velocity.X > 0)
                    {
                        justHit[0] = true;
                        hitDirection.X = MathHelper.Lerp(hitDirection.X, entity.velocity.X * .267f, .34f);
                        entity.velocity.X = 0;
                        entity.position.X = this.Left.X - entity.width + .1f;
                    }
                }
                if (!fallThrough && !jumpThrough && right && (entity.Top.Y < this.Bottom.Y && entity.Bottom.Y > this.Top.Y))
                {
                    if (entity.Left.X < this.Right.X && entity.Right.X > this.Left.X && entity.velocity.X < 0)
                    {
                        justHit[0] = true;
                        hitDirection.X = MathHelper.Lerp(hitDirection.X, entity.velocity.X * .267f, .34f);
                        entity.velocity.X = 0;
                        entity.position.X = this.Right.X - .1f;
                    }
                }
                if (!fallThrough && top && (entity.Right.X > this.Left.X && entity.Left.X < this.Right.X))
                {
                    if (entity.Bottom.Y > this.Top.Y && entity.velocity.Y >= 0)
                    {
                        justHit[0] = true;
                        hitDirection.Y = MathHelper.Lerp(hitDirection.Y, entity.velocity.Y * .267f, .34f);
                        entity.velocity.Y = 0;
                        entity.position.Y = this.Top.Y - entity.height + .1f;
                    }
                }
                if (!jumpThrough && bot && (entity.Right.X > this.Left.X && entity.Left.X < this.Right.X))
                {
                    if (entity.Top.Y < this.Bottom.Y && entity.velocity.Y < 0)
                    {
                        justHit[0] = true;
                        hitDirection.Y = MathHelper.Lerp(hitDirection.Y, entity.velocity.Y * .267f, .34f);
                        entity.velocity.Y = 0;
                        entity.position.Y = this.Bottom.Y + .1f;
                    }
                }
            }
            Update();
        }
        public virtual void Update()
        {

        }
        public virtual void Draw()
        {
            
        }
        public void Kill()
        {
            if(Kill(TimeLeft))
            {
                this.active = false;
            }
        }
        public virtual bool Kill(int timeLeft)
        {
            return true;
        }

        public Rectangle PlatformRect
        {
            get
            {
                return new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
            }
            set
            {
                this.position = value.Location.ToVector2();
                this.Size = value.Size();
            }
        }

        public static int NewPlatform<T>(Vector2 position, bool? fallOverride = null, bool? jumpOverride = null) where T : EntityPlatform, new()
        {
            int whoAmI = -1;
            int lowTime = int.MaxValue;
            for(int i = 0; i < OnyxiaSystem.MaxPlatforms; i++)
            {
                EntityPlatform platform = OnyxiaSystem.Platform[i];
                if(platform == null || !platform.active)
                {
                    whoAmI = i;
                    break;
                }
                if(platform.TimeLeft < lowTime)
                {
                    whoAmI = i;
                    lowTime = platform.TimeLeft;
                }
            }            
            if (OnyxiaSystem.Platform[whoAmI] != null && OnyxiaSystem.Platform[whoAmI].active)
                OnyxiaSystem.Platform[whoAmI].Kill();

            OnyxiaSystem.Platform[whoAmI] = new T();
            OnyxiaSystem.Platform[whoAmI].setup();
            OnyxiaSystem.Platform[whoAmI].position = position;
            if (fallOverride.HasValue) OnyxiaSystem.Platform[whoAmI].fallThrough = fallOverride.Value;
            if (jumpOverride.HasValue) OnyxiaSystem.Platform[whoAmI].jumpThrough = jumpOverride.Value;
            OnyxiaSystem.Platform[whoAmI].whoAmI = whoAmI;
            return whoAmI;
        }
        public static EntityPlatform NewPlatformDirect<T>(Vector2 position, bool? fallOverride = null, bool? jumpOverride = null) where T : EntityPlatform, new()
        {
            return OnyxiaSystem.Platform[NewPlatform<T>(position, fallOverride, jumpOverride)];
        }
    }
}
