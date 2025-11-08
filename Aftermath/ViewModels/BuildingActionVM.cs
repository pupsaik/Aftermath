using AftermathModels.Buildings;
using AftermathModels.Loot;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Collections.ObjectModel;
using AftermathGameManaging;

namespace Aftermath.ViewModels
{
    public class BuildingActionVM : BaseViewModel
    {
        private InventoryManager _inventoryManager;
        private CharacterManager _characterManager;
        private Building _building;

        public string Name => _building?.Name ?? "";
        public int Duration { get; private set; }

        public ObservableCollection<BuildingActionRewardVM> Rewards { get; private set; } = new();

        private BaseViewModel _currentBuildingActionVM;
        public BaseViewModel CurrentBuildingActionVM
        {
            get => _currentBuildingActionVM;
            set
            {
                _currentBuildingActionVM = value;
                OnPropertyChanged(nameof(CurrentBuildingActionVM));
            }
        }

        private BuildingActionNonOccupiedVM _buildingActionNonOccupiedVM;

        public event Action CloseEvent;

        private bool _isOccupied;
        public bool IsOccupied
        {
            get => _isOccupied;
            set
            {
                _isOccupied = value;
                OnPropertyChanged(nameof(IsOccupied));
            }
        }

        public ICommand CloseCommand { get; }

        public BuildingActionVM(CharacterManager characterManager, InventoryManager inventoryManager)
        {
            _characterManager = characterManager;
            _inventoryManager = inventoryManager;
            CloseCommand = new RelayCommand(() => CloseEvent?.Invoke());
        }

        public void ApplyBuilding(Building building)
        {
            _building = building;

            IsOccupied = _building.OccupiedCharacter != null;

            Rewards.Clear();

            if (IsOccupied)
            {
                CurrentBuildingActionVM = new BuildingActionOccupiedVM(_building.OccupiedCharacter, _building.OccupiedCharacter.ToolInHand);
            }
            else
            {
                _buildingActionNonOccupiedVM = new BuildingActionNonOccupiedVM(_building, _characterManager, _inventoryManager);
                _buildingActionNonOccupiedVM.CloseEvent += () => CloseEvent?.Invoke();
                _buildingActionNonOccupiedVM.SubmitEvent += () =>
                {
                    IsOccupied = true;
                    CloseEvent?.Invoke();
                };

                CurrentBuildingActionVM = _buildingActionNonOccupiedVM;

                OnPropertyChanged(nameof(_buildingActionNonOccupiedVM));
            }

            Duration = _building.Duration;

            foreach (var buildingProfit in building.TileLootTable.Drops)
            {
                if (buildingProfit.Profit is CompositeEffect compositeEffect)
                {
                    foreach (IConsumptionEffect consumptionEffect in compositeEffect.Effects)
                    {
                        Rewards.Add(new BuildingActionRewardVM(consumptionEffect));
                    }
                }
                else
                {
                    Rewards.Add(new BuildingActionRewardVM(buildingProfit.Profit));
                }
            }

            OnPropertyChanged(nameof(Rewards));
            OnPropertyChanged(nameof(Duration));
            OnPropertyChanged(nameof(Name));
        }
    }

    public class BuildingActionRewardVM : BaseViewModel
    {
        public string Type { get; }
        public int Amount { get; set; }
        public ImageSource Image => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/Items/{Type}.png"));

        public BuildingActionRewardVM(IOccupationProfit buildingProfit)
        {
            if (buildingProfit is ConsumptionEffect ce)
            {
                Type = ce.Type.ToString();
                Amount = ce.Amount;
            }
            else if (buildingProfit is IItem ip)
            {
                Type = ip.Name;
                Amount = 1;
            }
        }
    }
}
