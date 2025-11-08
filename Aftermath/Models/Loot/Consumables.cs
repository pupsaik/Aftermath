using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace AftermathModels.Loot
{
    public enum ConsumableType
    {
        Food,
        Medical
    }

    public interface IConsumable : IItem
    {
        ConsumableType Type { get; }
        IConsumptionEffect Effect { get; set; }
    }

    public abstract class Food : IConsumable
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public ConsumableType Type => ConsumableType.Food;
        public IConsumptionEffect Effect { get; set; }
    }

    public abstract class Medical : IConsumable
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public ConsumableType Type => ConsumableType.Medical;
        public IConsumptionEffect Effect { get; set; }
    }

    // ==== Food Items ====

    public class Berries : Food
    {
        public override string Name => "Berries";
        public override string Description => "потім придумаю ;)";

        public Berries()
        {
            Effect = new Random().NextDouble() * 100 > 10
                ? new CompositeEffect(new HungerImpact(10), new SanityImpact(10))
                : new CompositeEffect(new HealthImpact(-10), new HungerImpact(10), new SanityImpact(-10));
        }
    }

    public class Mushrooms : Food
    {
        public override string Name => "Mushrooms";
        public override string Description => "потім придумаю ;)";

        public Mushrooms()
        {
            Effect = new Random().NextDouble() * 100 > 20
                ? new HungerImpact(15)
                : new CompositeEffect(new HealthImpact(-15), new HungerImpact(15), new SanityImpact(-15));
        }
    }

    public class CannedMeat : Food
    {
        public override string Name => "Canned Meat";
        public override string Description => "потім придумаю ;)";

        public CannedMeat() => Effect = new HungerImpact(20);
    }

    public class FreshWater : Food, IOccupationProfit
    {
        public override string Name => "Fresh Water";
        public override string Description => "потім придумаю ;)";

        public FreshWater() => Effect = new ThirstImpact(25);
    }

    public class UnprocessedWater : Food
    {
        public override string Name => "Stale Water";
        public override string Description => "потім придумаю ;)";

        public UnprocessedWater() => Effect = new CompositeEffect(new ThirstImpact(15), new SanityImpact(-5));
    }

    public class Fish : Food
    {
        public override string Name => "Fish";
        public override string Description => "потім придумаю ;)";

        public Fish() => Effect = new HungerImpact(10);
    }

    public class Carrot : Food, IOccupationProfit
    {
        public override string Name => "Carrot";
        public override string Description => "потім придумаю ;)";

        public Carrot() => Effect = new HungerImpact(15);
    }

    // ==== Medical Item ====

    public class Bandage : Medical
    {
        public override string Name => "Bandage";
        public override string Description => "потім придумаю ;)";

        public Bandage() => Effect = new CompositeEffect(new HealthImpact(25), new SanityImpact(5));
    }

    // ==== Consumption Effects ====

    public interface IConsumptionEffect : IOccupationProfit
    {
        ConsumptionEffectType Type { get; }
        void Apply(Character character);
        void Cancel(Character character);
        IConsumptionEffect Clone();
    }

    public abstract class ConsumptionEffect : IOccupationProfit, IConsumptionEffect
    {
        public int Amount { get; set; }

        public abstract ConsumptionEffectType Type { get; }

        protected ConsumptionEffect(int amount)
        {
            Amount = amount;
        }

        public abstract void Apply(Character character);
        public abstract void Cancel(Character character);
        public abstract IConsumptionEffect Clone(); // Абстрактна реалізація
    }

    public class HealthImpact : ConsumptionEffect
    {
        public HealthImpact(int amount) : base(amount) { }

        public override ConsumptionEffectType Type => ConsumptionEffectType.Health;

        public override void Apply(Character character) => character.CurrentHealth += Amount;
        public override void Cancel(Character character) => character.CurrentHealth -= Amount;
        public override IConsumptionEffect Clone() => new HealthImpact(Amount);
    }

    public class HungerImpact : ConsumptionEffect
    {
        public HungerImpact(int amount) : base(amount) { }

        public override ConsumptionEffectType Type => ConsumptionEffectType.Hunger;

        public override void Apply(Character character) => character.CurrentHunger += Amount;
        public override void Cancel(Character character) => character.CurrentHunger -= Amount;
        public override IConsumptionEffect Clone() => new HungerImpact(Amount);
    }

    public class ThirstImpact : ConsumptionEffect
    {
        public ThirstImpact(int amount) : base(amount) { }

        public override ConsumptionEffectType Type => ConsumptionEffectType.Thirst;

        public override void Apply(Character character) => character.CurrentThirst += Amount;
        public override void Cancel(Character character) => character.CurrentThirst -= Amount;
        public override IConsumptionEffect Clone() => new ThirstImpact(Amount);
    }

    public class SanityImpact : ConsumptionEffect
    {
        public SanityImpact(int amount) : base(amount) { }

        public override ConsumptionEffectType Type => ConsumptionEffectType.Sanity;

        public override void Apply(Character character) => character.CurrentSanity += Amount;
        public override void Cancel(Character character) => character.CurrentSanity -= Amount;
        public override IConsumptionEffect Clone() => new SanityImpact(Amount);
    }

    public class CompositeEffect : IConsumptionEffect
    {
        public readonly List<ConsumptionEffect> Effects = new();

        public CompositeEffect(params ConsumptionEffect[] effects)
        {
            Effects = new List<ConsumptionEffect>(effects);
        }

        public ConsumptionEffectType Type => ConsumptionEffectType.Composite;

        public void AddEffect(ConsumptionEffect effect) => Effects.Add(effect);

        public void Apply(Character character)
        {
            foreach (var effect in Effects)
                effect.Apply(character);
        }

        public void Cancel(Character character)
        {
            foreach (var effect in Effects)
                effect.Cancel(character);
        }

        public IConsumptionEffect Clone()
        {
            var clonedEffects = Effects.Select(e => (ConsumptionEffect)e.Clone()).ToArray();
            return new CompositeEffect(clonedEffects);
        }
    }

    public enum ConsumptionEffectType
    {
        Health,
        Hunger,
        Thirst,
        Sanity,
        Composite
    }

    public static class EffectUtils
    {
        public static IConsumptionEffect BoostSanity(IConsumptionEffect effect, int bonus)
        {
            var clone = effect.Clone();

            if (clone is SanityImpact sanityImpact)
            {
                sanityImpact.Amount += bonus;
            }
            else if (clone is CompositeEffect composite)
            {
                foreach (var subEffect in composite.Effects)
                {
                    if (subEffect is SanityImpact sanity)
                    {
                        sanity.Amount += bonus;
                    }
                }
            }

            return clone;
        }
    }
}
