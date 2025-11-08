using AftermathModels.Characters;
using AftermathModels.Exceptions;
using AftermathModels.Loot;
using AftermathModels.Map;
using AftermathModels.Observer;
using AftermathModels.Occupation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AftermathModels.Buildings
{
    public class SleepingBag : Building
    {
        public override string Name => "Sleeping Bag";

        public override OccupationType OccupationType => OccupationType.Rest;

        public override int Duration => 1;

        public override TileLootTable TileLootTable => new TileLootTable(
            [
                new LootDrop(new SanityImpact(20), 100, 1, 1)
            ]   
        );
    }
}
