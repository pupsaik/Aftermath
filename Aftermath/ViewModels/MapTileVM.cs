using Accessibility;
using AftermathModels.Map;
using AftermathModels.Observer;
using AftermathModels.Occupation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aftermath.ViewModels
{
    public class MapTileVM : BaseViewModel
    {
        public Tile Tile { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Size { get; set; }

        private ImageSource _occupiedCharacterIcon;
        public ImageSource OccupiedCharacterIcon
        {
            get => _occupiedCharacterIcon;
            set
            {
                _occupiedCharacterIcon = value;
                OnPropertyChanged(nameof(OccupiedCharacterIcon));
            }
        }

        private ImageSource _tileImage;
        public ImageSource TileImage
        {
            get => _tileImage;
            set
            {
                _tileImage = value;
                OnPropertyChanged(nameof(TileImage));
            }
        }

        public MapTileVM(Tile tile)
        {
            Tile = tile;
            if (Tile.Occupation.OccupationType is OccupationType.None)
            {
                TileImage = new BitmapImage(new Uri($"pack://application:,,,/Aftermath;component/Resources/Icons/Base.png"));
            }
            else
            {
                TileImage = new BitmapImage(new Uri(Tile.IsVisible
                    ? $"pack://application:,,,/Aftermath;component/Resources/Icons/{Tile.Occupation.OccupationType}{(Tile.IsExplored ? ".png" : "_foggy.png")}"
                    : "pack://application:,,,/Aftermath;component/Resources/Icons/FoggyTile.png"));
            }
        }
    }
}
