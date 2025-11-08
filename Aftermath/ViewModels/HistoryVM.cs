using AftermathModels.IConsumableCommand;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aftermath.ViewModels
{
    public class HistoryVM : BaseViewModel
    {
        private InventoryVM _inventoryVM;

        private ConsumableCommandInvoker _commandInvoker;

        public ObservableCollection<HistoryItemVM> History { get; set; }

        public ICommand CloseCommand { get; }

        public HistoryVM(ConsumableCommandInvoker commandInvoker, InventoryVM inventoryVM)
        {
            _inventoryVM = inventoryVM;

            _commandInvoker = commandInvoker;

            History = new(commandInvoker.History.Select(c => new HistoryItemVM(c as ConsumeCommand)));

            _commandInvoker.OnHistoryChanged += () =>
            {
                History = new(_commandInvoker.History.Select(c => new HistoryItemVM(c as ConsumeCommand)));
                OnPropertyChanged(nameof(History));

                foreach (var historyItem in History)
                {
                    historyItem.OnUndo += () =>
                    {
                        _commandInvoker.UndoCommand(historyItem.Command);
                    };
                }
            };

            CloseCommand = new RelayCommand(() => _inventoryVM.IsHistoryOpened = false);
        }

    }

    public class HistoryItemVM
    {
        public ConsumeCommand Command { get; set; }

        public ImageSource CharacterIcon => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/{Command.Character.Name}.png"));

        public ImageSource ConsumableIcon => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/Items/{Command.Consumable.Name}.png"));

        public ICommand UndoCommand { get; }

        public Action OnUndo;

        public HistoryItemVM(ConsumeCommand command)
        {
            Command = command;
            UndoCommand = new RelayCommand(() =>
            {
                OnUndo?.Invoke();
            });
        }
    }
}
