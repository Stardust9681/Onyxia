using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using Onyxia.Core.Utils;
using Terraria.DataStructures;

namespace Onyxia.Content.NPCs.Hostile
{
    public class AstralCrawler : ModNPC
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.SpiderEgg;
        public override void SetDefaults()
        {
            NPC.friendly = false;
            NPC.damage = 12;
            NPC.width = 40;
            NPC.height = 40;
            NPC.lifeMax = 80;
            NPC.value = 240f;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.defense = 1;
            NPC.knockBackResist = 0.9f;

            NPC.buffImmune[BuffID.CursedInferno] = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.ai[2] = 1;
            base.OnSpawn(source);
        }

        //Attack forms/types
        private const int Jump = 1;
        private const int Crawl = 2;
        public float AttackType
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public float Timer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        private const float tilesDetectRadius = 18;
        private const float detectRadius = tilesDetectRadius * 16;
        public override void AI()
        {
            if (NPC.target < 0 || NPC.target > Main.maxPlayers)
                NPC.TargetClosest(false);
            Player target = Main.player[NPC.target];
            if (target.dead || !target.active)
            {
                NPC.TargetClosest(false);
                target = Main.player[NPC.target];
            }

            Vector2 targetDir = NPC.DirectionTo(target.position);
            int xDir = targetDir.X < 0 ? -1 : 1;
            int yDir = targetDir.Y < 0 ? -1 : 1;

            

            if (NPC.DistanceSQ(target.Center) < (detectRadius*detectRadius) && Collision.CanHitLine(NPC.Center, NPC.width-4, NPC.height-4, target.Center, target.width-4, target.height-4))
                AttackType = Crawl;
            else
                AttackType = Jump;


            if (AttackType == Jump)
            {
                Timer++;
                if (Timer > 240 && MathF.Abs(NPC.velocity.Y) < 1)
                {
                    NPC.velocity.Y = -((NPC.ai[2] * 2f) + 4f);
                    NPC.ai[2] = ((int)(NPC.ai[2]) % 3) + 1;
                    NPC.velocity.X = xDir * 2.4f;
                    Timer = 0;
                }
                if (NPC.collideX)
                    NPC.velocity.X *= -1;
                if (NPC.collideY && NPC.oldVelocity.Y > 0 && NPC.velocity.Y >= 0)
                    NPC.velocity.X = 0;

                NPC.velocity.Y += .18f;
            }
            else if(AttackType == Crawl)
            {
                if (NPC.velocity.LengthSquared() < 16)
                    NPC.velocity += targetDir * .1f;
                else
                    NPC.velocity *= .98f;
            }
        }
        public override void DrawEffects(ref Color drawColor)
        {
            if (AttackType == Crawl)
            {
                float check = Main.rand.NextFloat();
                if (check < .1f)
                {
                    Dust d = Dust.NewDustDirect(Vector2.Lerp(NPC.Center, Main.player[NPC.target].Center, check * 10f), 1, 1, ModContent.DustType<Dusts.SparkleDust>());
                    d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                    d.fadeIn = 20;
                    d.velocity = Vector2.Zero;
                }
            }

            if (Main.rand.NextFloat() < .75f)
            {
                Dust d = Dust.NewDustDirect(NPC.Center + (Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * detectRadius), 1, 1, DustID.YellowStarDust);
                d.noGravity = true;
                d.scale *= 1.4f;
                d.rotation = d.velocity.X;
                d.velocity = Vector2.Zero;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if(AttackType == Crawl)
                Utils.DrawLine(spriteBatch, NPC.Center, Main.player[NPC.target].Center, Color.White * .3f, Color.White * .7f, 2);
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRuleNormal.Common<Items.Materials.AstralyteShard>(.8f, 1, 3));
        }
    }
}
