using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.ItemDropRules;
using Terraria;

namespace Onyxia.Core.Utils
{
    public static class Extensions
    {
        public static bool IsLocallyOwned(this Projectile p) => p.owner == Main.myPlayer;
    }
}
