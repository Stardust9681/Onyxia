using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Onyxia.Content.DamageClasses.Technician
{
    public class TechDamageClass : DamageClass
    {
        //public Color DamageColour { get; }
        public override void SetStaticDefaults()
        {
            ClassName.SetDefault("Technical damage.");
        }
    }
    /*
    public class Generic : TechDamageClass
    {
        public override Color DamageColour => Color.White;
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic) return StatInheritanceData.Full;
            return base.GetModifierInheritance(damageClass);
        }
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic) return true;
            return base.GetEffectInheritance(damageClass);
        }
    }
    public class Blast : TechDamageClass
    {
        public override Color DamageColour => Color.Red;
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic || damageClass == Melee) return StatInheritanceData.Full;
            return base.GetModifierInheritance(damageClass);
        }
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic || damageClass == Melee) return true;
            return base.GetEffectInheritance(damageClass);
        }
    }
    public class Infusion : TechDamageClass
    {
        public override Color DamageColour => Color.MediumVioletRed;
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic) return StatInheritanceData.Full;
            return base.GetModifierInheritance(damageClass);
        }
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic || damageClass == Magic) return true;
            return base.GetEffectInheritance(damageClass);
        }
    }
    public class Spark : TechDamageClass
    {
        public override Color DamageColour => new Color((Color.Orange.ToVector3() + Color.Yellow.ToVector3()) * .5f);
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic || damageClass == Ranged) return StatInheritanceData.Full;
            return base.GetModifierInheritance(damageClass);
        }
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic || damageClass == Ranged) return true;
            return base.GetEffectInheritance(damageClass);
        }
    }
    public class Construct : TechDamageClass
    {
        public override Color DamageColour => Color.LimeGreen;
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic || damageClass == Summon) return StatInheritanceData.Full;
            return base.GetModifierInheritance(damageClass);
        }
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic || damageClass == Summon) return true;
            return base.GetEffectInheritance(damageClass);
        }
    }
    public class Plated : TechDamageClass
    {
        public override Color DamageColour => Color.Blue;
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic || damageClass == Melee) return true;
            return base.GetEffectInheritance(damageClass);
        }
    }
    public class Energized : TechDamageClass
    {
        public override Color DamageColour => Color.Cyan;
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic || damageClass == Magic) return StatInheritanceData.Full;
            return base.GetModifierInheritance(damageClass);
        }
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic) return true;
            return base.GetEffectInheritance(damageClass);
        }
    }
    */
}
