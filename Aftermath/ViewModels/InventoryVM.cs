using AftermathGameManaging;
using AftermathModels.Loot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Aftermath.ViewModels
{
    public class InventoryVM : BaseViewModel
    {
        private MainVM _mainVM;

        private bool _isConsumeItemWindowOpened;
        public bool IsConsumeItemWindowOpened
        {
            get => _isConsumeItemWindowOpened;
            set
            {
                _isConsumeItemWindowOpened = value;
                OnPropertyChanged(nameof(IsConsumeItemWindowOpened));
            }
        }

        private bool _isHistoryOpened;
        public bool IsHistoryOpened
        {
            get => _isHistoryOpened;
            set
            {
                _isHistoryOpened = value;
                OnPropertyChanged(nameof(IsHistoryOpened));
            }
        }

        public ObservableCollection<IInventoryItemVM> InventoryItemVMs { get; } = [];

        private bool _isInventoryEmpty = true;
        public bool IsInventoryEmpty
        {
            get => _isInventoryEmpty;
            set
            {
                _isInventoryEmpty = value;
                OnPropertyChanged(nameof(IsInventoryEmpty));
            }
        }

        private IInventoryItemVM _hoveredItem;
        public IInventoryItemVM HoveredItem
        {
            get => _hoveredItem;
            set
            {
                _hoveredItem = value;
                OnPropertyChanged(nameof(HoveredItem));
            }
        }

        private IConsumable _selectedConsumable;
        public IConsumable SelectedConsumable
        {
            get => _selectedConsumable;
            set
            {
                _selectedConsumable = value;
                OnPropertyChanged(nameof(SelectedConsumable));
            }
        }

        private ConsumeItemVM _consumeItemVM;
        public ConsumeItemVM ConsumeItemVM
        {
            get => _consumeItemVM;
            set
            {
                _consumeItemVM = value;
                OnPropertyChanged(nameof(ConsumeItemVM));
            }
        }

        public ICommand BackCommand { get; }
        public ICommand OpenHistoryCommand { get; }
        public CharacterManager CharacterManager { get; set; }
        public InventoryManager InventoryManager { get; set; }

        public HistoryVM HistoryVM { get; set; }

        public InventoryVM(MainVM mainVM, CharacterManager characterManager, InventoryManager inventoryManager)
        {
            _mainVM = mainVM;
            CharacterManager = characterManager;
            InventoryManager = inventoryManager;
            InventoryManager.ItemsChanged += OnItemsChanged;

            HistoryVM = new(InventoryManager.ConsumableCommandInvoker, this);

            foreach (var item in inventoryManager.GetItems())
            {
                if (item is IConsumable consumable)
                {
                    InventoryConsumableVM inventoryConsumableVM = new InventoryConsumableVM(consumable);
                    InventoryItemVMs.Add(inventoryConsumableVM);
                }
                else if (item is ITool tool)
                    InventoryItemVMs.Add(new InventoryToolVM(tool));
            }

            BackCommand = new RelayCommand(() =>
            {
                IsConsumeItemWindowOpened = false;
                IsHistoryOpened = false;
                _mainVM.CurrentVM = _mainVM.BaseVM;
            });

            OpenHistoryCommand = new RelayCommand(() =>
            {
                IsHistoryOpened = true;
            });

            IsInventoryEmpty = InventoryItemVMs.Count == 0;
        }

        private void OnItemsChanged()
        {
            InventoryItemVMs.Clear();

            foreach (var item in InventoryManager.GetItems())
            {
                if (item is IConsumable consumable)
                    InventoryItemVMs.Add(new InventoryConsumableVM(consumable));
                else if (item is ITool tool)
                    InventoryItemVMs.Add(new InventoryToolVM(tool));
            }

            IsInventoryEmpty = InventoryItemVMs.Count == 0;
        }
    }
}
