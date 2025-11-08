using AftermathModels.Occupation;
using System.Windows;

namespace AftermathModels.Map.Tiles
{
    public class HuntersHutTile : Tile
    {
        public override Tile Clone(Point coordinates)
        {
            return new HuntersHutTile(coordinates, Occupation);
        }

        public HuntersHutTile(Point coordinates, IOccupation occupation) : base(coordinates, occupation)
        {
        }
    }
}
