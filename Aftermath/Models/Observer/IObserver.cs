using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AftermathModels.Observer
{
    public interface IObserver
    {
        void Update(CharacterEventType type, object data);
    }

    public enum CharacterEventType
    { 
        OccupationEnded,
        OccupationSet,
        HealthChanged,
        HungerChanged,
        ThirstChanged,
        SanityChanged,
        IconChanged,
        Death
    }
}
