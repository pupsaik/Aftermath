using AftermathModels.Exceptions;
using AftermathModels.Observer;
using Aftermath.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AftermathModels.Buildings;

namespace Aftermath.GameManaging
{
    public class BuildingManager
    {
        public Trailer Trailer { get; set; } = new();

        public Tent Tent1 { get; set; } = new();

        public Tent Tent2 { get; set; } = null;

        public Tent Tent3 { get; set; } = null;

        public SleepingBag SleepingBag1 { get; set; } = new();

        public SleepingBag SleepingBag2 { get; set; } = new();

        public void OccupyBuilding(Building building, Character character)
        {
            building.OccupiedCharacter = character ?? throw new GameException("Character was not selected.");
            building.Attach(new OccupationEndObserver(building));
        }
    }
}
