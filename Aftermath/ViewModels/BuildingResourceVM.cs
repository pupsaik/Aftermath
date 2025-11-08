using AftermathModels.Loot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aftermath.ViewModels
{
    public class BuildingResourceVM : BaseViewModel
    {
        private Resource _resource;
        public ResourceType ResourceType => _resource.Type;
        public ImageSource ImageSource => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/{ResourceType}.png"));

        public int Quantity
        {
            get => _resource.Quantity;
            set
            {
                _resource.Quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public BuildingResourceVM(Resource resource)
        {
            _resource = resource;
        }
    }
}
