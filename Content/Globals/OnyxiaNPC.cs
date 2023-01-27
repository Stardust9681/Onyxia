using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;
using Onyxia.Core.Utils;
using Terraria.ModLoader.IO;
using System.IO;

namespace Onyxia.Content.Globals
{
    public class OnyxiaNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public float spdMult = 1f;
        public override bool PreAI(NPC npc)
        {
            ResetSpeed(npc, spdMult);
            return base.PreAI(npc);
        }
        public override void PostAI(NPC npc)
        {
            SetSpeed(npc, spdMult);
            spdMult = MathHelper.Lerp(spdMult, 1, .003f);
        }

        /// <summary>
        /// Call this in <seealso cref="PostAI(NPC)"/>
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="mult"></param>
        /// <param name="ignoreGrav"></param>
        private void SetSpeed(NPC npc, float mult, bool ignoreGrav = false)
        {
            npc.velocity.X *= mult;
            if (npc.noGravity || ignoreGrav)
                npc.velocity.Y *= mult;
        }

        /// <summary>
        /// Call this in <seealso cref="PreAI(NPC)"/>
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="mult"></param>
        /// <param name="ignoreGrav"></param>
        private void ResetSpeed(NPC npc, float mult, bool ignoreGrav = false)
        {
            npc.velocity.X /= mult;
            if (npc.noGravity || ignoreGrav)
                npc.velocity.Y /= mult;
        }

        /// <summary>
        /// Used to modify the speed of NPCs
        /// </summary>
        /// <param name="aimMult"></param>
        public void AdjustSpeed(float aimMult)
        {
            spdMult = MathHelper.Lerp(spdMult, aimMult, .5f);
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            int[] validNPCs = new int[] {NPCID.AngryBones, NPCID.AngryBonesBig, NPCID.AngryBonesBigHelmet, NPCID.AngryBonesBigMuscle, NPCID.DarkCaster,
            NPCID.HellArmoredBones, NPCID.HellArmoredBonesMace, NPCID.HellArmoredBonesSpikeShield, NPCID.HellArmoredBonesSword, NPCID.DiabolistRed, NPCID.DiabolistWhite,
            NPCID.RustyArmoredBonesAxe, NPCID.RustyArmoredBonesFlail, NPCID.RustyArmoredBonesSword, NPCID.RustyArmoredBonesSwordNoArmor, NPCID.RaggedCaster, NPCID.RaggedCasterOpenCoat,
            NPCID.BlueArmoredBones, NPCID.BlueArmoredBonesMace, NPCID.BlueArmoredBonesNoPants, NPCID.BlueArmoredBonesSword, NPCID.Necromancer, NPCID.NecromancerArmored,
            NPCID.Paladin, NPCID.BoneLee, NPCID.SkeletonSniper, NPCID.TacticalSkeleton, NPCID.CursedSkull, NPCID.GiantCursedSkull, NPCID.SkeletonCommando, NPCID.DungeonGuardian, NPCID.SkeletronHead, NPCID.SkeletronHand};
            if(validNPCs.Contains(npc.type))
                npcLoot.Add(ItemDropRuleNormal.Common<Items.Materials.NecromanticEssence>(.5f, 1, 2).SetCondition(new ArbitraryCondition((DropAttemptInfo info) => info.player.ZoneDungeon && Main.hardMode, "In the Dungeon, in Hardmode")));
            validNPCs = new int[] { NPCID.Shark, NPCID.Crab, NPCID.Squid, NPCID.SeaSnail };
            if (validNPCs.Contains(npc.type))
                npcLoot.Add(ItemDropRuleNormal.Common<Items.Materials.MoteOfWater>(1, 1, 2).SetCondition(new ArbitraryCondition((DropAttemptInfo info) => info.player.ZoneBeach, "In Ocean")));
        }
    }
}
