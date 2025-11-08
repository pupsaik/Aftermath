using AftermathModels.Map;
using System.Windows;

public class MapManager
{
    private static MapManager? _instance;
    private static readonly object _lock = new object();

    private readonly ITileFactory _hexFactory;

    public Dictionary<Point, Tile> Tiles { get; }

    public static void Initialize(ITileFactory tileFactory)
    {
        if (_instance != null)
            throw new InvalidOperationException("MapManager already initialized.");

        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = new MapManager(tileFactory);
            }
        }
    }

    public static MapManager Instance
    {
        get
        {
            if (_instance == null)
                throw new InvalidOperationException("MapManager is not initialized. Call Initialize(tileFactory) first.");
            return _instance;
        }
    }

    public MapManager(ITileFactory hexFactory)
    {
        _hexFactory = hexFactory;

        Tiles = new Dictionary<Point, Tile>
        {
            [new Point(0, 0)] = _hexFactory.CreateBaseTile(new Point(0, 0)),
            [new Point(1, 0)] = _hexFactory.CreateCampTile(new Point(1, 0)),
            [new Point(0, 1)] = _hexFactory.CreateFishingDockTile(new Point(0, 1)),
            [new Point(-1, 0)] = _hexFactory.CreateCampTile(new Point(-1, 0)),
            [new Point(0, -1)] = _hexFactory.CreateForestTile(new Point(0, -1)),
            [new Point(1, 1)] = _hexFactory.CreateFishingDockTile(new Point(1, 1)),
            [new Point(-1, -1)] = _hexFactory.CreateFishingDockTile(new Point(-1, -1)),
            [new Point(1, -1)] = _hexFactory.CreateFishingDockTile(new Point(1, -1)),
            [new Point(-1, 1)] = _hexFactory.CreateFishingDockTile(new Point(-1, 1))
        };

        ExploreTile(Tiles[new Point(0, 0)]);
    }

    public IEnumerable<Tile> GetTiles() => Tiles.Values;

    public void ExploreTile(Tile tile)
    {
        tile.IsExplored = true;
        Tiles.TryGetValue(new Point(tile.Coordinates.X - 1, tile.Coordinates.Y), out var left);
        Tiles.TryGetValue(new Point(tile.Coordinates.X + 1, tile.Coordinates.Y), out var right);
        Tiles.TryGetValue(new Point(tile.Coordinates.X, tile.Coordinates.Y + 1), out var up);
        Tiles.TryGetValue(new Point(tile.Coordinates.X, tile.Coordinates.Y - 1), out var down);

        if (left != null) left.IsVisible = true;
        if (right != null) right.IsVisible = true;
        if (up != null) up.IsVisible = true;
        if (down != null) down.IsVisible = true;
    }

    public IEnumerable<Tile> GetAdjacentTiles(Tile tile)
    {
        List<Tile> adjacentTiles = [];
        Tiles.TryGetValue(new Point(tile.Coordinates.X - 1, tile.Coordinates.Y), out var left);
        Tiles.TryGetValue(new Point(tile.Coordinates.X + 1, tile.Coordinates.Y), out var right);
        Tiles.TryGetValue(new Point(tile.Coordinates.X, tile.Coordinates.Y - 1), out var up);
        Tiles.TryGetValue(new Point(tile.Coordinates.X, tile.Coordinates.Y + 1), out var down);

        if (left != null) adjacentTiles.Add(left);
        if (right != null) adjacentTiles.Add(right);
        if (up != null) adjacentTiles.Add(up);
        if (down != null) adjacentTiles.Add(down);

        return adjacentTiles;
    }

    public void OccupyTile(Tile tile, Character character)
    {
        tile.IsOccupied = true;
        tile.Occupation.OccupiedCharacter = character;
    }

    public Tile? GetTile(Point coordinates)
    {
        return Tiles.TryGetValue(coordinates, out var tile) ? tile : null;
    }
}
