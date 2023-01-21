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
    public class LoosePlatform : EntityPlatform
    {
        public override void SetDefaults()
        {
            this.lifeTime = 1200;
            this.width = 256;
            this.height = 24;
        }

        //Current problems:
            //Player cannot jump!!
                //I think this is because platform is moving upwards, and player doesn't get clearance..
                //Therefore, platform sets y-position, and kills velocity
                //Ending jump, and snapping player back into place.
            //Platform snaps down very quickly
                //Change lerp value probably
            //Platform adjusts when low hitDirection values and player standing on top
                //Change justHit to bool[2] instead of bool maybe
                //Check justHit[1] to see previous justHit value, if true, don't screw with hitDirection
                //justHit[0] acts how justHit works now.
        public override void Update()
        {
            if (TimeLeft % 20 == 0)
                Dust.QuickBox(TopLeft, BottomRight, 6, Color.BlueViolet, new Action<Dust>((Dust x) => { }));
            if (TimeLeft > lifeTime - 5)
                this.oldPosition = this.position;

            hitDirection *= .9f;
            if (hitDirection.LengthSquared() < .02)
                hitDirection = Vector2.Zero;

            this.position = Vector2.Lerp(this.position, this.oldPosition + (hitDirection * 64f), .08f);
        }
    }
}