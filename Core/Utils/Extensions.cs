using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria;

namespace Onyxia.Core.Utils
{
    public static class Extensions
    {
        public static float DifficultyStrength
        {
            get
            {
                if (Main.GameModeInfo.IsJourneyMode)
                {
                    return CreativePowerManager.Instance.GetPower<CreativePowers.DifficultySliderPower>().StrengthMultiplierToGiveNPCs;
                }
                else
                {
                    if (Main.masterMode)
                        return 3f;
                    else if (Main.expertMode)
                        return 2f;
                    else
                        return 1f;
                }
            }
        }
        public static float Difficulty
        {
            get
            {
                if(Main.GameModeInfo.IsJourneyMode)
                    return Microsoft.Xna.Framework.MathHelper.Lerp(.25f, 1f, (DifficultyStrength - .5f) * .4f);
                if (Main.masterMode)
                    return 1f; //1 def = -1 dmg
                else if (Main.expertMode)
                    return .5f; //1 def = -0.5 dmg
                else
                    return .25f; //1 def = -.25 dmg
            }
        }
        public static double GetRelativeDamage(double dmg, int def)
        {
            double value = dmg - (def * Difficulty);
            return value < 1 ? 1 : value;
        }
        public static bool IsLocallyOwned(this Projectile p) => p.owner == Main.myPlayer;
        public static Recipe AddCondition(this Recipe r, Func<Recipe, bool> predicate, string desc = null)
        {
            return r.AddCondition(new Recipe.Condition(Terraria.Localization.NetworkText.FromFormattable(desc ?? ""), new Predicate<Recipe>(predicate)));
        }
    }
}
