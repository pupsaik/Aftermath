using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AftermathModels.Loot
{
    public interface IItem : ILoot, IOccupationProfit
    {
        string Description { get; }
    }

    public enum LootCategory
    {
        Resource,
        Consumable,
        Tool
    }
}
