using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aftermath.ViewModels
{
    public class CloseGameVM : BaseViewModel
    {
        private BaseVM _baseVM;
        public Action OnCloseGame;

        public ICommand CloseGameCommand { get; }
        public ICommand CloseCommand { get; }

        public CloseGameVM(BaseVM baseVM)
        {
            _baseVM = baseVM;
            CloseGameCommand = new RelayCommand(() => OnCloseGame?.Invoke());
            CloseCommand = new RelayCommand(() => _baseVM.IsCloseGameVisible = false);
        }
    }
}
