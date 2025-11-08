using AftermathModels.Occupation;
using System.Windows;

namespace AftermathModels.Map
{
    public abstract class Tile
    {
        public Point Coordinates { get; set; }
        public bool IsVisible { get; set; } = false;
        public bool IsExplored { get; set; } = false;
        public bool IsOccupied { get; set; } = false;
        public IOccupation Occupation { get; set; }

        public abstract Tile Clone(Point coordinates);

        protected Tile(Point coordinates, IOccupation occupation)
        {
            Coordinates = coordinates;
            Occupation = occupation;
        }
    }
}
