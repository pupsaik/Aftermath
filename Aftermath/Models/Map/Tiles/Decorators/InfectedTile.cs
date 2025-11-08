using System.Windows;

namespace AftermathModels.Map.Tiles.Decorators
{
    public class InfectedTile : TileDecorator
    {
        private readonly List<DangerousEvent> _modifiedEvents;

        public InfectedTile(Tile inner) : base(inner)
        {
            _modifiedEvents = new List<DangerousEvent>()
            {
                new Infection(40)
            };
        }

        public List<DangerousEvent> DangerousEvents => _modifiedEvents;

        public override Tile Clone(Point coordinates)
        {
            Tile clonnedInner = _inner.Clone(coordinates);
            return new InfectedTile(clonnedInner);
        }
    }
}
