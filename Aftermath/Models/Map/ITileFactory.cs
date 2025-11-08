using System.Windows;

namespace AftermathModels.Map
{
    public interface ITileFactory
    {
        Tile CreateBaseTile(Point coordinates);
        Tile CreateCampTile(Point coordinates);
        Tile CreateFishingDockTile(Point coordinates);
        Tile CreateForestTile(Point coordinates);
        Tile CreateHuntersHutTile(Point coordinates);
    }
}
