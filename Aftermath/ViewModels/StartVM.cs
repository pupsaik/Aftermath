using AftermathModels.Map;
using AftermathModels.Map.Factories;
using System.Windows.Input;

namespace Aftermath.ViewModels
{
    public class StartVM : BaseViewModel
    {
        private MainVM _mainVM;

        public ICommand NormalCommand { get; }
        public ICommand HardCommand { get; }
        public ICommand CloseCommand { get; }

        public event Action<ITileFactory> OnChoseFactory; 
        
        public event Action OnClose;

        public StartVM(MainVM mainVM)
        {
            _mainVM = mainVM;

            CloseCommand = new RelayCommand(() => OnClose?.Invoke());

            NormalCommand = new RelayCommand(() =>
            {
                OnChoseFactory?.Invoke(new NormalTilesFactory());
            });

            HardCommand = new RelayCommand(() =>
            {
                OnChoseFactory?.Invoke(new HardTilesFactory());
            });

        }
    }
}
