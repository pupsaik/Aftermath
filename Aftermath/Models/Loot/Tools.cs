using AftermathModels.Occupation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AftermathModels.Loot
{
    public enum ToolType
    {
        ForExploration,
        ForBase
    }

    public interface ITool : IItem
    {
        OccupationType TileType { get; }
        List<ToolEffectEntry> Effects { get; }
        bool IsOccupied { get; set; }
    }

    public class Axe : ITool
    {
        public bool IsOccupied { get; set; }
        public string Name => "Axe";
        public string Description => "Reduces damage chance by 10%.\nUseful for cutting trees or defense.";
        public OccupationType TileType => OccupationType.Forest | OccupationType.CampingSite | OccupationType.HuntersHut;
        public List<ToolEffectEntry> Effects => [
            new ToolEffectEntry(ToolEffect.DamageChanceReduce, 10),
    ];
    }

    public class Machete : ITool
    {
        public bool IsOccupied { get; set; }
        public string Name => "Machete";
        public string Description => "Reduces damage chance by 20%.\nEfficient for clearing paths and protection.";
        public OccupationType TileType => OccupationType.Forest | OccupationType.CampingSite | OccupationType.HuntersHut;
        public List<ToolEffectEntry> Effects => [
            new ToolEffectEntry(ToolEffect.DamageChanceReduce, 20)
        ];
    }

    public class FirstAidManual : ITool
    {
        public bool IsOccupied { get; set; }
        public string Name => "First Aid Manual";
        public string Description => "Increaces healing after rest by 5.\nA basic guide to treat wounds.";
        public OccupationType TileType => OccupationType.Rest;
        public List<ToolEffectEntry> Effects => [
            new ToolEffectEntry(ToolEffect.BetterHealingSkills, 5)
        ];
    }

    public class Book : ITool
    {
        public bool IsOccupied { get; set; }
        public string Name => "Book";
        public string Description => "Increaces sanity restoration after rest by 10.\nA relaxing read during breaks.";
        public OccupationType TileType => OccupationType.Rest;
        public List<ToolEffectEntry> Effects => [
            new ToolEffectEntry(ToolEffect.BetterSleep, 10)
        ];
    }

    public class Flashlight : ITool
    {
        public bool IsOccupied { get; set; }
        public string Name => "Flashlight";
        public string Description => "Increaces chances to get food from exploration by 30%.\nHelps in locating supplies during night raids.";
        public OccupationType TileType => OccupationType.CampingSite;
        public List<ToolEffectEntry> Effects => [
            new ToolEffectEntry(ToolEffect.BonusFood, 30)
        ];
    }

    public class FishingRod : ITool
    {
        public bool IsOccupied { get; set; }
        public string Name => "Fishing Rod";
        public string Description => "Increaces chances to get fish from fishing dock by 50%.\nEssential for successful fishing expeditions.";
        public OccupationType TileType => OccupationType.FishingDock;
        public List<ToolEffectEntry> Effects => [
            new ToolEffectEntry(ToolEffect.BonusFish, 50)
        ];
    }

    public class Sunscreen : ITool
    {
        public bool IsOccupied { get; set; }
        public string Name => "Sunscreen";
        public string Description => "Reduces overheating chance by 10%.\nProtects from harmful sun exposure.";
        public OccupationType TileType => OccupationType.FishingDock;
        public List<ToolEffectEntry> Effects => [
            new ToolEffectEntry(ToolEffect.OverheatingChanceReduce, 10)
        ];
    }


    public class ToolEffectEntry
    {
        public ToolEffect ToolEffect { get; }
        public int EffectValue { get; }

        public ToolEffectEntry(ToolEffect toolEffect, int effectValue)
        {
            ToolEffect = toolEffect;
            EffectValue = effectValue;
        }
    }

    public enum ToolEffect
    {
        DamageChanceReduce,
        OverheatingChanceReduce,
        BonusWood,
        BonusFood,
        BonusFish,
        BetterHealingSkills,
        BetterSleep
    }
}
