using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;

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

        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            //Core.Utils.ItemDropRuleNormal iDropRule = Core.Utils.ItemDropRuleNormal.Common(ItemID.Bone, .5f, 1, 2) as Core.Utils.ItemDropRuleNormal;
            ///iDropRule.condition = new Core.Utils.ArbitraryCondition((DropAttemptInfo info) => info.player.ZoneDungeon && Main.hardMode, "In the Dungeon during Hardmode", true);
            //globalLoot.Add(iDropRule);
        }
    }
}
