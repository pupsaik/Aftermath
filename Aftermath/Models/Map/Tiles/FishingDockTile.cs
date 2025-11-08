using AftermathModels.Occupation;
using System.Windows;

namespace AftermathModels.Map.Tiles
{
    public class FishingDockTile : Tile
    {
        public override Tile Clone(Point coordinates)
        {
            return new FishingDockTile(coordinates, Occupation);
        }

        public FishingDockTile(Point coordinates, IOccupation occupation) : base(coordinates, occupation)
        {

        }
    }
}
