using Aftermath.Models.Characters.States;
using AftermathModels.Characters.States;
using AftermathModels.Loot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AftermathModels.Map
{
    public abstract class DangerousEvent
    {
        public abstract HazardType Type { get; }

        public abstract string Name { get; }

        public int Probability { get; }

        public abstract IConsumptionEffect Effect { get; }

        public DangerousEvent(int probability)
        {
            Probability = probability;
        }

        public abstract int GetEffectiveProbability(ITool tool);

        public abstract void Apply(Character character);
    }

    public class SpiderAttack : DangerousEvent
    {
        public SpiderAttack(int probability) : base(probability)
        {
        }

        public override HazardType Type => HazardType.SpiderAttack;

        public override string Name => "Spider attack";

        public override IConsumptionEffect Effect => new HealthImpact(-20);

        public override void Apply(Character character)
        {
            Effect.Apply(character);
            character.AddState(new PoisonedState());
            character.Statuses.Add(Status.Poisoned);
        }

        public override int GetEffectiveProbability(ITool tool)
        {
            if (tool == null) return Probability;

            var toolEffect = tool.Effects.FirstOrDefault(e => e.ToolEffect == ToolEffect.DamageChanceReduce);

            if (toolEffect != null)
            {
                return Probability - toolEffect.EffectValue;
            }

            return Probability;
        }

    }

    public class BearAttack : DangerousEvent
    {
        public BearAttack(int probability) : base(probability)
        {
        }

        public override string Name => "Bear attack";


        public override HazardType Type => HazardType.BearAttack;

        public override IConsumptionEffect Effect => new HealthImpact(-20);

        public override void Apply(Character character)
        {
            Effect.Apply(character);
        }

        public override int GetEffectiveProbability(ITool tool)
        {
            if (tool == null) return Probability;

            var toolEffect = tool.Effects.FirstOrDefault(e => e.ToolEffect == ToolEffect.DamageChanceReduce);

            if (toolEffect != null)
            {
                return Probability - toolEffect.EffectValue;
            }

            return Probability;
        }
    }

    public class MutantAttack : DangerousEvent
    {
        public MutantAttack(int probability) : base(probability)
        {
        }

        public override string Name => "Mutant attack";


        public override HazardType Type => HazardType.MutantAttack;

        public override IConsumptionEffect Effect => new HealthImpact(-20);

        public override void Apply(Character character)
        {
            Effect.Apply(character);
        }

        public override int GetEffectiveProbability(ITool tool)
        {
            if (tool == null) return Probability;

            var toolEffect = tool.Effects.FirstOrDefault(e => e.ToolEffect == ToolEffect.DamageChanceReduce);

            if (toolEffect != null)
            {
                return Probability - toolEffect.EffectValue;
            }

            return Probability;
        }
    }

    public class Overheating : DangerousEvent
    {
        public Overheating(int probability) : base(probability)
        {
        }

        public override string Name => "Overheating";


        public override HazardType Type => HazardType.Overheating;

        public override IConsumptionEffect Effect => new SanityImpact(-10);

        public override void Apply(Character character)
        {
            Effect.Apply(character);
        }

        public override int GetEffectiveProbability(ITool tool)
        {
            if (tool == null) return Probability;

            var toolEffect = tool.Effects.FirstOrDefault(e => e.ToolEffect == ToolEffect.OverheatingChanceReduce);

            if (toolEffect != null)
            {
                return Probability - toolEffect.EffectValue;
            }

            return Probability;
        }
    }

    public class Infection : DangerousEvent
    {
        public Infection(int probability) : base(probability)
        {
        }

        public override string Name => "Infection";


        public override HazardType Type => HazardType.Infection;

        public override IConsumptionEffect Effect => new HealthImpact(20);

        public override void Apply(Character character)
        {
            Effect.Apply(character);
            character.AddState(new InfectedState());
        }

        public override int GetEffectiveProbability(ITool tool)
        {
            return Probability;
        }
    }

    public enum HazardType
    {
        Infection,
        BearAttack,
        MutantAttack,
        SpiderAttack,
        Radiation,
        Overheating
    }
}
