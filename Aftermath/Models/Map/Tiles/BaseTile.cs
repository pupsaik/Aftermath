using AftermathModels.Loot;
using AftermathModels.Occupation;
using System.Windows;

namespace AftermathModels.Map.Tiles
{
    public class BaseTile : Tile
    {
        public TileLootTable TileLootTable { get; }

        public List<DangerousEvent> DangerousEvents { get; }

        public override Tile Clone(Point coordinates)
        {
            return new BaseTile(coordinates, Occupation);
        }

        public BaseTile(Point coordinates, IOccupation occupation) : base(coordinates, occupation)
        {
            IsVisible = true;
            IsExplored = true;
        }
    }
}
