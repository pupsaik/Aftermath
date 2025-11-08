using Aftermath.GameManaging;
using Aftermath.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AftermathGameManaging
{
    public class TimeManager
    {
        private CharacterManager _characterManager;
        private EventManager _eventManager;

        public int DayCounter { get; set; }
        public event Action? DayCounterChanged;

        public TimeManager(CharacterManager characterManager, EventManager eventManager)
        {
            DayCounter = 1;
            _characterManager = characterManager;
            _eventManager = eventManager;
        }

        public void SkipToNextDay()
        {
            DayCounter++;
            DayCounterChanged?.Invoke();

            foreach (var character in _characterManager.GetCharacters())
            {
                character.SkipToNextDay();
            }

            _eventManager.ExecuteAll(_characterManager.GetCharacters());
        }
    }
}
