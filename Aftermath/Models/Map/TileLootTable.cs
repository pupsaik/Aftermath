using Aftermath.ViewModels;
using AftermathGameManaging;
using AftermathModels.Loot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace AftermathModels.Map
{
    public class LootDrop
    {
        public IOccupationProfit Profit { get; }
        public int DropChance { get; set; }
        public int MinAmount { get; set; }
        public int MaxAmount { get; set; }

        public LootDrop(IOccupationProfit item, int dropChance, int minAmount, int maxAmount)
        {
            Profit = item;
            DropChance = dropChance;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
        }

        public void AcceptDrop(Character character, InventoryManager inventoryManager)
        {
            if (Profit is IConsumptionEffect consumptionEffect)
            {
                var betterSleep = character.ToolInHand?.Effects
                    .FirstOrDefault(e => e.ToolEffect == ToolEffect.BetterSleep);

                var effectToApply = consumptionEffect;

                if (betterSleep != null)
                {
                    effectToApply = EffectUtils.BoostSanity(consumptionEffect, betterSleep.EffectValue);
                }

                effectToApply.Apply(character);
            }
            else if (Profit is IItem item)
            {
                inventoryManager.LootOfDay.Add(item);
            }
        }

        public int GetEffectiveDropChance(ITool tool)
        {
            if (tool == null) return DropChance;

            ToolEffectEntry toolEffectEntry;

            if (Profit is Fish)
            {
                toolEffectEntry = tool.Effects.FirstOrDefault(e => e.ToolEffect == ToolEffect.BonusFish);

                if (toolEffectEntry != null)
                {
                    return DropChance + toolEffectEntry.EffectValue;
                }
            }

            if (Profit is Food)
            {
                toolEffectEntry = tool.Effects.FirstOrDefault(e => e.ToolEffect == ToolEffect.BonusFood);

                if (toolEffectEntry != null)
                {
                    return DropChance + toolEffectEntry.EffectValue;
                }
            }

            return DropChance;
        }
    }

    public class TileLootTable
    {
        public List<LootDrop> Drops { get; set; }

        public TileLootTable(List<LootDrop> drops)
        {
            Drops = drops;
        }
    }
}
