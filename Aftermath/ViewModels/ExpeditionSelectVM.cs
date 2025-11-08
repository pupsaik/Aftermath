using AftermathModels.Loot;
using AftermathModels.Map;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Collections.ObjectModel;
using AftermathGameManaging;
using AftermathModels.Buildings;
using AftermathModels.Occupation;
using System.Xml.Linq;

namespace Aftermath.ViewModels
{
    public class ExpeditionSelectVM : BaseViewModel
    {
        private MapVM _mapVM;

        private InventoryManager _inventoryManager;

        private CharacterManager _characterManager;

        private bool _isVisible = false;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        private string _type;
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        private int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

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

        private BaseViewModel _currentExpeditionSelect;
        public BaseViewModel CurrentExpeditionSelect
        {
            get => _currentExpeditionSelect;
            set
            {
                _currentExpeditionSelect = value;
                OnPropertyChanged(nameof(CurrentExpeditionSelect));
            }
        }
        
        public ObservableCollection<ExpeditionRewardVM> Rewards { get; } = [];
        public ObservableCollection<HazardVM> Hazards { get; } = [];

        public MapTileVM SelectedTile { get; set; } = null;

        public ICommand CloseCommand { get; }

        public event Action CloseEvent;

        public ExpeditionSelectVM(CharacterManager characterManager, InventoryManager inventoryManager, MapVM mapVM)
        {
            _characterManager = characterManager;
            _inventoryManager = inventoryManager;
            _mapVM = mapVM;
            CloseCommand = new RelayCommand(() => IsVisible = false);
        }

        public void ChangeTile(MapTileVM tile)
        {
            IsVisible = true;
            SelectedTile = tile;
            Type = tile.Tile.Occupation.Name;
            Duration = tile.Tile.Occupation.Duration;
            IsOccupied = tile.Tile.Occupation.OccupiedCharacter != null;

            Rewards.Clear();
            Hazards.Clear();

            if (IsOccupied)
            {
                CurrentExpeditionSelect = new ExpeditionSelectOccupiedVM(SelectedTile.Tile.Occupation.OccupiedCharacter, SelectedTile.Tile.Occupation.OccupiedCharacter.ToolInHand);
            }
            else
            {
                ExpeditionSelectNonOccupiedVM nonOccupiedVM = new ExpeditionSelectNonOccupiedVM(tile, _characterManager, _inventoryManager, _mapVM);
                nonOccupiedVM.CloseEvent += () => IsVisible = false;
                nonOccupiedVM.SubmitEvent += () =>
                {
                    IsVisible = false;
                    IsOccupied = true;
                };

                CurrentExpeditionSelect = nonOccupiedVM;
            }

            foreach (var drop in tile.Tile.Occupation.TileLootTable.Drops)
            {
                Rewards.Add(new ExpeditionRewardVM(drop));
            }

            foreach (var hazard in tile.Tile.Occupation.DangerousEvents)
            {
                Hazards.Add(new HazardVM(hazard));
            }
        }
    }

    public class ExpeditionRewardVM : BaseViewModel
    {
        public string Name { get; }

        public string Amount { get; set; }

        public ImageSource Image => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/Items/{Name.Replace(" ", "")}.png"));

        public string DropChance { get; set; }

        public ExpeditionRewardVM(LootDrop expeditionReward)
        {
            if (expeditionReward.Profit is IItem item)
            {
                Name = item.Name;
                Amount = expeditionReward.MinAmount == expeditionReward.MaxAmount ? expeditionReward.MinAmount.ToString() : $"{expeditionReward.MinAmount}-{expeditionReward.MaxAmount}";
                DropChance = $"{expeditionReward.DropChance}%";
            }
        }
    }

    public class HazardVM : BaseViewModel
    {
        private DangerousEvent _dangerousEvent;

        public int Probability { get; set; }

        public ImageSource Image => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/{_dangerousEvent.Type}.png"));

        public HazardVM(DangerousEvent dangerousEvent)
        {
            _dangerousEvent = dangerousEvent;
            Probability = _dangerousEvent.Probability;
        }
    }
}
