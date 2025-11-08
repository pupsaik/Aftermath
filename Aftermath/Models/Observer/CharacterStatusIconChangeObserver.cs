using AftermathModels.Occupation;
using Aftermath.ViewModels;
using Aftermath.Models.Characters.States;
using System.Windows.Media;

namespace AftermathModels.Observer
{
    public class CharacterStatusIconChangeObserver : IObserver
    {
        private CharacterVM _characterVM;

        public CharacterStatusIconChangeObserver(CharacterVM character)
        {
            _characterVM = character;
        }

        public void Update(CharacterEventType eventType, object data)
        {
            if (eventType == CharacterEventType.IconChanged)
            { 
                _characterVM.Statuses.Add((Status)data);
            }
        }
    }
}
