using Aftermath.Models.Characters.States;
using AftermathModels.Characters;
using AftermathModels.Loot;
using AftermathModels.Map;
using AftermathModels.Observer;
using AftermathModels.Occupation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aftermath.Models.Occupation
{
    public class NoOccupation : IOccupation
    {
        public string Name => "Base";

        public int Duration => 0;

        public Status CharacterStatus => Status.None;

        public int TimeLeft { get; set; }

        public Character OccupiedCharacter { get; set; }

        public TileLootTable TileLootTable { get; init; } = new([]);

        public List<DangerousEvent> DangerousEvents { get; }

        public OccupationType OccupationType => OccupationType.None;

        public void Attach(IObserver observer)
        {
            throw new NotImplementedException();
        }

        public void Detach(IObserver observer)
        {
            throw new NotImplementedException();
        }

        public void Notify(CharacterEventType type, object data)
        {
            throw new NotImplementedException();
        }

        public void Occupy(Character character, ITool tool)
        {
            throw new NotImplementedException();
        }
    }
}
