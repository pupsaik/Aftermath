using Aftermath.Models.Characters.States;
using Aftermath.Models.Occupation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AftermathModels.Characters.States
{
    public interface IState
    {
        void SkipToNextDay(Character character);
        StateType State { get; }
    }

    public class AliveState : IState
    {
        public StateType State => StateType.Alive;

        public void SkipToNextDay(Character character)
        {
            if (character.CurrentOccupation is not NoOccupation)
                character.CurrentOccupation.TimeLeft--;

            character.CurrentHunger -= 20;
            character.CurrentThirst -= 20;
            character.CurrentSanity -= 20;
        }
    }

    public class PoisonedState : IState
    {
        public StateType State => StateType.Poisoned;

        public int DaysLeft { get; set; } = 3;

        public void SkipToNextDay(Character character)
        {
            character.CurrentHealth -= 10;
            DaysLeft--;
            if (DaysLeft <= 0)
            {
                character.RemoveStatus(Status.Poisoned);
                character.RemoveState(StateType.Poisoned);
            }
        }
    }

    public class InfectedState : IState
    {
        public StateType State => StateType.Poisoned;

        public int DaysLeft { get; set; }

        public void SkipToNextDay(Character character)
        {
            character.CurrentHealth -= 10;
            DaysLeft--;
        }
    }

    public class HungryState : IState
    {
        public StateType State => StateType.Hungry;

        public void SkipToNextDay(Character character)
        {
            character.CurrentHealth -= 20;
        }
    }

    public class ThirstyState : IState
    {
        public StateType State => StateType.Thirsty;

        public void SkipToNextDay(Character character)
        {
            character.CurrentHealth -= 15;
        }
    }

    public class InsaneState : IState
    {
        public StateType State => StateType.Insane;

        public void SkipToNextDay(Character character)
        {
            character.InsaneActivity();
        }
    }

    public enum StateType
    {
        Alive,
        Hungry,
        Thirsty,
        Poisoned,
        Insane,
        Dead
    }
}
