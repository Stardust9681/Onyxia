using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Onyxia.Content.NPCs
{
    //Writing for another mod, this WILL be removed later
    [Autoload(false)]
    public class FungulatedBug : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //What are you lot calling this thing?
            DisplayName.SetDefault("Fungulated Insectoid");
        }
        public override void SetDefaults()
        {
            NPC.width = 100;
            NPC.height = 100;
            NPC.aiStyle = -1;
        }
        const int Num_Attacks = 1;
        float AITimer
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        float AttackType
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        public override void AI()
        {
            int duration = 0;
            if(AttackType == 0)
            {
                duration = 180;
                NPC.velocity.X = 0;
                NPC.velocity.Y += .014f;
                if(AITimer%30-1<=0)
                {
                    Vector2 velocity = (Vector2.UnitY * Main.rand.Next(new int[] { -1, 1 })).RotatedByRandom(MathHelper.ToRadians(15f)) * 8f;
                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, velocity, 0, 20, 1f, Main.myPlayer).hostile = true;
                    AITimer += 2;
                }
            }
            else if(AttackType == 1)
            {
                duration = 60;
                Player target = Main.player[NPC.target];
                Vector2 targetPos = target.position;
                if(Math.Abs(NPC.position.X - targetPos.X) > 64 && AITimer < duration*2/3)
                {
                    NPC.velocity.X += .018f * (targetPos.X > NPC.position.X ? -1 : 1);
                    NPC.velocity.Y += .014f;
                    NPC.noTileCollide = true;
                }
                else
                {
                    NPC.velocity.X = 0;
                    NPC.velocity.Y += .14f;
                    NPC.noTileCollide = false;
                    if(NPC.collideY)
                    {
                        AITimer = duration;
                    }
                }
            }
            // . . .
            if (AITimer == 0)
            {
                OnStartAttack((int)AttackType, duration);
            }
            AITimer++;
            if (AITimer > duration)
            {
                OnEndAttack((int)AttackType, duration);
                AITimer = 0;
            }
        }
        private void OnStartAttack(int attackType, int duration)
        {
            if(attackType == 1)
            {
                NPC.velocity.Y -= 12f;
            }
        }
        private void OnEndAttack(int attackType, int duration)
        {
            if(attackType == 1)
            {
                //Spawn shockwave projectile
            }

            AttackType = Main.rand.Next(Num_Attacks);
        }
    }
}
