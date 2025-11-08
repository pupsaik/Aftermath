using AftermathModels.Map;
using AftermathModels.Occupation;
using Aftermath.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AftermathModels.Observer
{
    public class ExplorationEndedObserver : IObserver
    {
        private MapTileVM _mapTileVM;
        private MapVM _mapVM;

        public ExplorationEndedObserver(MapTileVM mapTileVM, MapVM mapVM)
        {
            _mapTileVM = mapTileVM;
            _mapVM = mapVM;
        }

        public void Update(CharacterEventType type, object data)
        {
            if (type == CharacterEventType.OccupationEnded)
            {
                _mapTileVM.OccupiedCharacterIcon = new BitmapImage(new Uri($"pack://application:,,,/Aftermath;component/Resources/Icons/None.png"));
                MapManager.Instance.ExploreTile(_mapTileVM.Tile);
                UpdateTileImage(_mapTileVM);

                foreach (var tile in MapManager.Instance.GetAdjacentTiles(_mapTileVM.Tile))
                    UpdateTileImage(_mapVM.TileVMs.Where(t => t.Tile == tile).First());
            }
        }

        private void UpdateTileImage(MapTileVM mapTileVM)
        {
            if (mapTileVM.Tile.Occupation.OccupationType is OccupationType.None)
            {
                mapTileVM.TileImage = new BitmapImage(new Uri($"pack://application:,,,/Aftermath;component/Resources/Icons/Base.png"));
            }
            else
            {
                mapTileVM.TileImage = new BitmapImage(new Uri(mapTileVM.Tile.IsVisible
                    ? $"pack://application:,,,/Aftermath;component/Resources/Icons/{mapTileVM.Tile.Occupation.OccupationType}{(mapTileVM.Tile.IsExplored ? ".png" : "_foggy.png")}"
                    : "pack://application:,,,/Aftermath;component/Resources/Icons/FoggyTile.png"));
            }
        }
    }
}
