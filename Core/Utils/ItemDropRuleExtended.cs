using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.ItemDropRules;
using Terraria;

namespace Onyxia.Core.Utils
{
    public class ItemDropRuleNormal : IItemDropRule
    {
        float chance;
        int minimum;
        int maximum;
        int type;
        public IItemDropRuleCondition condition;
        public ItemDropRuleNormal(int type, float chance = 1, int min = 1, int max = 1)
        {
            this.type = type;
            this.chance = chance;
            minimum = min;
            maximum = max;
        }
        public bool CanDrop(DropAttemptInfo info) => condition.CanDrop(info);
        public void ReportDroprates(List<DropRateInfo> info, DropRateInfoChainFeed feed)
        {
            info.Add(new DropRateInfo(type, minimum, maximum, chance));
        }
        public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
        {
            if(!condition.CanDrop(info))
                return new ItemDropAttemptResult() { State = ItemDropAttemptResultState.DoesntFillConditions };
            if (info.rng.NextFloat() > chance)
                return new ItemDropAttemptResult() { State = ItemDropAttemptResultState.FailedRandomRoll };
            CommonCode.DropItem(info, type, info.rng.Next(minimum, maximum+1));
            return new ItemDropAttemptResult() { State = ItemDropAttemptResultState.Success };
        }
        public List<IItemDropRuleChainAttempt> ChainedRules
        {
            get;
            private set;
        }
        public static IItemDropRule Common(int type, float chance = 1, int min = 1, int max = 1)
        {
            return new ItemDropRuleNormal(type, chance, min, max);
        }
        public static IItemDropRule Common<T>(float chance, int min = 1, int max = 1) where T : Terraria.ModLoader.ModItem
        {
            return Common(Terraria.ModLoader.ModContent.ItemType<T>(), chance, min, max);
        }
    }
    public class ArbitraryCondition : IItemDropRuleCondition
    {
        public string desc;
        Func<DropAttemptInfo, bool> canDrop;
        bool showDrop;
        public ArbitraryCondition(Func<DropAttemptInfo, bool> predicate, string description = null, bool global = false)
        {
            canDrop = predicate;
            desc = description;
            showDrop = !global;
        }
        public string GetConditionDescription() => desc;
        public bool CanDrop(DropAttemptInfo info) => canDrop.Invoke(info);
        public bool CanShowItemDropInUI() => showDrop;
    }
}
