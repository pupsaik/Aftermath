using Aftermath.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AftermathModels.Observer
{
    public class CharacterDeathObserver : IObserver
    {
        private CharacterVM _characterVM;

        public CharacterDeathObserver(CharacterVM сharacterVM)
        {
            _characterVM = сharacterVM;
        }

        public void Update(CharacterEventType type, object data)
        {
            if(type == CharacterEventType.Death)
            {
                _characterVM.IsDead = true;
            }
        }
    }
}
