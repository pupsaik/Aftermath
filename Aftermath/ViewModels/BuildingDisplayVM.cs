using AftermathModels.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aftermath.ViewModels
{
    public class BuildingDisplayVM : BaseViewModel
    {
        public Building Building { get; }

        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int Variant { get; set; }
        public ImageSource ImagePath => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/{Building.Name.Replace(" ", "")}{Variant}{(IsNight ? "Night" : "")}.png"));
        public ImageSource ImagePathMouseOver => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/{Building.Name.Replace(" ", "")}{Variant}MouseOver.png"));

        private bool _isNight;
        public bool IsNight
        {
            get => _isNight;
            set
            {
                _isNight = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }

        private bool _isModalOpened = false;
        public bool IsModalOpened
        {
            get => _isModalOpened;
            set
            {
                _isModalOpened = value;
                OnPropertyChanged(nameof(IsModalOpened));
            }
        }

        public ICommand BuildinActionCommand { get; }

        public BuildingDisplayVM(BaseVM parent, Building building, double x, double y, double width, double height, int variant)
        {
            OnPropertyChanged(nameof(IsModalOpened));
            Building = building;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Variant = variant;

            BuildinActionCommand = new RelayCommand(() => parent.OpenBuildingAction(this));
        }

        public void CloseModal()
        {
            IsModalOpened = false;
        }
    }
}
