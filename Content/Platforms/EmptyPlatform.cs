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

namespace Onyxia.Content.Platforms
{
    public class EmptyPlatform : EntityPlatform
    {
        public override void SetDefaults()
        {
            this.lifeTime = 900;
            this.width = 24;
            this.height = 256;
        }
        public override void Update()
        {
            if(TimeLeft%20 == 0)
                Dust.QuickBox(TopLeft, BottomRight, 6, Main.DiscoColor, new Action<Dust>((Dust x) => { }));
        }
    }
}
