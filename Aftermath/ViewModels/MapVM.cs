using AftermathGameManaging;
using AftermathModels.Map;
using AftermathModels.Map.Tiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Aftermath.ViewModels
{
    public class MapVM : BaseViewModel
    {
        private MainVM _mainVM;

        public ObservableCollection<MapTileVM> TileVMs { get; } = [];
        public ExpeditionSelectVM ExpeditionSelectVM { get; }

        public double ControlWidth { get; set; }
        public double ControlHeight { get; set; }

        public ICommand BackCommand { get; set; }
        public ICommand TileClick { get; set; }

        public MapVM(MainVM mainVM, MapManager mapManager, InventoryManager inventoryManager, CharacterManager characterManager)
        {
            _mainVM = mainVM;
            LoadFromTiles(mapManager.Tiles, 125);
            BackCommand = new RelayCommand(() => _mainVM.CurrentVM = _mainVM.BaseVM);
            ExpeditionSelectVM = new(characterManager, inventoryManager, this);

            TileClick = new RelayCommandParametrized<MapTileVM>(tileVM => { 
                if(tileVM.Tile.IsVisible && tileVM.Tile is not BaseTile)
                    ExpeditionSelectVM.ChangeTile(tileVM); 
            });
        }

        public void LoadFromTiles(Dictionary<Point, Tile> tiles, double sideLength)
        {
            TileVMs.Clear();

            foreach (var kvp in tiles)
            {
                Point point = kvp.Key;
                Tile tile = kvp.Value;

                TileVMs.Add(new MapTileVM(tile)
                {
                    X = 800 + point.X * sideLength - sideLength / 2,
                    Y = 400 - point.Y * sideLength - sideLength / 2,
                    Size = sideLength,
                });
            }
        }
    }
}
