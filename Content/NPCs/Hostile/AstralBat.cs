using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using Onyxia.Core.Utils;

namespace Onyxia.Content.NPCs.Hostile
{
    //StarryBat rewrite, custom AI and better data
    public class AstralBat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }
        public override void SetDefaults()
        {
            NPC.defense = 2;
            NPC.damage = 10;
            NPC.aiStyle = -1; //Custom
            NPC.width = 26;
            NPC.height = 18;
            NPC.lifeMax = 40;
            NPC.value = 40f;
            NPC.noGravity = true;

            //Buff immunities
            NPC.buffImmune[BuffID.CursedInferno] = true;
        }
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

            Vector2 targetDir = NPC.DirectionTo(target.position)*.25f;
            Vector2 targetVelocity = NPC.velocity + new Vector2(targetDir.X, targetDir.Y*.5f);
            if (MathF.Abs(targetVelocity.Y) > 1.5f)
                targetVelocity.Y = NPC.velocity.Y;
            if (MathF.Abs(targetVelocity.X) > 3f)
                targetVelocity.X = NPC.velocity.X;
            NPC.velocity = Vector2.Lerp(NPC.velocity, targetVelocity, .34f);

            if (NPC.collideX)
                NPC.velocity.X *= -1;
            if (NPC.collideY)
                NPC.velocity.Y *= -1;

            NPC.rotation = NPC.velocity.X * .1f;
            NPC.direction = NPC.velocity.X < 0 ? -1 : 1;
            NPC.spriteDirection = NPC.direction;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if(NPC.frameCounter > 20)
            {
                NPC.frame.Y = (NPC.frame.Y + frameHeight) % (Main.npcFrameCount[NPC.type] * frameHeight);
                NPC.frameCounter = 0;
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.position.Y > Main.worldSurface * .35f ? 0 : .15f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRuleNormal.Common<Items.Materials.AstralyteShard>(.8f, 1, 3));
        }
        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (MathF.Abs(knockback) > 5)
                knockback = MathF.Sign(knockback) * 5;
            return base.StrikeNPC(ref damage, defense, ref knockback, hitDirection, ref crit);
        }
    }
}
