namespace AftermathModels.Map.Tiles
{
    public abstract class TileDecorator : Tile
    {
        protected Tile _inner;

        protected TileDecorator(Tile inner) : base(inner.Coordinates, inner.Occupation)
        {
            _inner = inner;
        }
    }
}
