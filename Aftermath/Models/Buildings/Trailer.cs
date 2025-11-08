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
using System.Windows.Ink;

namespace AftermathModels.Buildings
{
    public class Trailer : Building
    {
        public override string Name => "Trailer";

        public override int Duration => 1;

        public override OccupationType OccupationType => OccupationType.Rest;
        
        public override TileLootTable TileLootTable => new TileLootTable(
            [
                new LootDrop(new SanityImpact(40), 100, 1, 1),
                new LootDrop(new HealthImpact(5), 100, 1, 1)
            ]   
        );
    }
}
