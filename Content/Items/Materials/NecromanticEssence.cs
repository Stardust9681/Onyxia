using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Onyxia.Content.Items.Materials
{
    public class NecromanticEssence : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 8;
            Item.rare = 2;
            Item.maxStack = 999;
            Item.value = 250 * 5;
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            if (Main.rand.NextFloat() < .75f)
            {
                Vector2 spawnPos = new Vector2(Item.position.X + Main.rand.NextFloat(-16f, 16f), Item.position.Y + Item.height + Main.rand.NextFloat(-8f, 0f));
                Dust d = Dust.NewDustDirect(spawnPos, 1, 1, ModContent.DustType<Dusts.NecromanticDust>(), Alpha: 150, Scale: Main.rand.NextFloat(.85f, .9f));
            }
            base.Update(ref gravity, ref maxFallSpeed);
        }
    }
}
