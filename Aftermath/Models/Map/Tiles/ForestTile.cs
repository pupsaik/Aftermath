using AftermathModels.Occupation;
using System.Windows;

namespace AftermathModels.Map.Tiles
{
    public class ForestTile : Tile
    {
        public override Tile Clone(Point coordinates)
        {
            return new ForestTile(coordinates, Occupation);
        }

        public ForestTile(Point coordinates, IOccupation occupation) : base(coordinates, occupation)
        {
        }
    }
}