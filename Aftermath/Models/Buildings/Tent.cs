using AftermathModels.Characters;
using AftermathModels.Exceptions;
using AftermathModels.Loot;
using AftermathModels.Map;
using AftermathModels.Occupation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AftermathModels.Buildings
{
    public class Tent : Building
    {
        public override string Name => "Tent";

        public override int Duration => 1;

        public override OccupationType OccupationType => OccupationType.Rest;

        public override TileLootTable TileLootTable => new TileLootTable(
            [
                new LootDrop(new SanityImpact(25), 100, 1, 1)
            ]   
        );
    }
}
