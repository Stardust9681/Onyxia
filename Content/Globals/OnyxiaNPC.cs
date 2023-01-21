using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

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
    }
}
