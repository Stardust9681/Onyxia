using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace Onyxia.Content.NPCs.Boss.Gumo
{
    [Autoload(false)]
    public class Gumo : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.lifeMax = 20;
            NPC.damage = 3;
            NPC.defense = 1;
            NPC.width = 20;
            NPC.height = 20;
        }
        public override void AI()
        {
            for(int i = 0; i < Main.maxNPCs; i++)
            {
                if (i == NPC.whoAmI)
                    continue;
                NPC npc = Main.npc[i];
                if (!npc.active || npc.type != NPC.type)
                    continue;
                if (npc.getRect().Intersects(NPC.getRect()))
                {
                    if (NPC.ai[0] > npc.ai[0])
                    {
                        NPC.ai[0] += NPC.ai[0];
                        AdjustStats(npc);
                        npc.active = false;
                    }
                    else
                    {
                        npc.ai[0] += NPC.ai[0];
                        (npc.ModNPC as Gumo).AdjustStats(NPC);
                        NPC.active = false;
                    }
                }
            }
        }
        private void AdjustStats(NPC other)
        {
            NPC.lifeMax += (int)(other.lifeMax*2);
            if (NPC.lifeMax > 10000)
                NPC.lifeMax = 10000;
            NPC.life += (int)(other.life*2);
            if (NPC.life > NPC.lifeMax)
                NPC.life = NPC.lifeMax;
            NPC.defense += other.defense;
            if (NPC.defense > 20)
                NPC.defense = 20;
            NPC.damage += other.damage;
            if (NPC.damage > 100)
                NPC.damage = 100;
            if (NPC.ai[0] > 10)
            {
                NPC.boss = true;
            }
        }
    }
}
