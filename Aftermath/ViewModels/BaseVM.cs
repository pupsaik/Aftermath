using Aftermath.GameManaging;
using AftermathGameManaging;
using AftermathModels.Buildings;
using AftermathModels.Loot;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aftermath.ViewModels
{
    public class BaseVM : BaseViewModel
    {
        private TimeManager _timeManager;

        public MainVM MainVM { get; }
        public CharacterPanelVM CharacterPanelVM { get; }
        public ObservableCollection<BuildingDisplayVM> BuildingDisplayVMs { get; }
        public ObservableCollection<BuildingResourceVM> BuildingResourceVMs { get; }

        private bool _isDay = true;
        public bool IsDay
        {
            get => _isDay;
            set
            {
                _isDay = value;
                OnPropertyChanged(nameof(IsDay));
            }
        }

        public EndOfDayVM EndOfDayVM { get; }
        public DayResultsVM DayResultsVM { get; }
        public CloseGameVM CloseGameVM { get; }

        public BuildingDisplayVM SelectedBuilding { get; set; }

        private bool _isEndOfDayVisible = false;
        public bool IsEndOfDayVisible
        {
            get => _isEndOfDayVisible;
            set
            {
                _isEndOfDayVisible = value;
                OnPropertyChanged(nameof(IsEndOfDayVisible));
            }
        }

        private bool _isDayResultsVisible = false;
        public bool IsDayResultsVisible
        {
            get => _isDayResultsVisible;
            set
            {
                _isDayResultsVisible = value;
                OnPropertyChanged(nameof(IsDayResultsVisible));
            }
        }

        private bool _isCloseGameVisible = false;
        public bool IsCloseGameVisible
        {
            get => _isCloseGameVisible;
            set
            {
                _isCloseGameVisible = value;
                OnPropertyChanged(nameof(IsCloseGameVisible));
            }
        }

        public ICommand SkipToNextDayCommand { get; }
        public ICommand CloseGameCommand { get; }

        public int DayCounter { get; set; }

        public BaseVM(MainVM mainVM, TimeManager timeManager, BuildingManager buildings, CharacterManager characterManager, InventoryManager inventoryManager, EventManager eventManager)
        {
            _timeManager = timeManager;

            DayCounter = timeManager.DayCounter;
            EndOfDayVM = new(characterManager, this, timeManager);
            DayResultsVM = new(inventoryManager, eventManager, this);
            SelectedBuilding = new(this, new Trailer(), 0, 0, 0, 0, 0) { IsModalOpened = false };

            MainVM = mainVM;
            CloseGameVM = new(this);

            CharacterPanelVM = new CharacterPanelVM(mainVM, characterManager);
            BuildingDisplayVMs = [new(this, buildings.Trailer, 300, 110, 600, 450, 1),
                new(this, buildings.Tent1, 1050, 230, 400, 200, 1),
                new(this, buildings.SleepingBag1, 1115, 400, 150, 200, 1),
                new(this, buildings.SleepingBag2, 850, 400, 150, 200, 2)
            ];

            //BuildingResourceVMs = [
            //    new(Game.Instance.BuildingResources[ResourceType.Wood]),
            //    new(Game.Instance.BuildingResources[ResourceType.Stone]),
            //    new(Game.Instance.BuildingResources[ResourceType.Metal]),
            //    new(Game.Instance.BuildingResources[ResourceType.Rope])
            //];

            _timeManager.DayCounterChanged += () =>
            {
                DayCounter = _timeManager.DayCounter;
                OnPropertyChanged(nameof(DayCounter));
            };

            SkipToNextDayCommand = new RelayCommand(SkipToNextDayMethod);
            CloseGameCommand = new RelayCommand(() => IsCloseGameVisible = true);
        }

        public void ShowDayResults()
        {
            IsDayResultsVisible = true;
            DayResultsVM.Update();
        }

        private void SkipToNextDayMethod()
        {
            EndOfDayVM.Update();
            IsEndOfDayVisible = true;
        }

        public void OpenBuildingAction(BuildingDisplayVM building)
        {
            SelectedBuilding = building;
            MainVM.BuildingActionVM.ApplyBuilding(SelectedBuilding.Building);
            MainVM.BuildingActionVM.CloseEvent += building.CloseModal;
            building.IsModalOpened = building.IsModalOpened = true;

            OnPropertyChanged(nameof(SelectedBuilding));
        }

        public void SetToNight()
        {
            IsDay = false;
            foreach (var buildingVM in BuildingDisplayVMs)
            {
                buildingVM.IsNight = true;
            }
        }

        public void SetToDay()
        {
            IsDay = true;
            foreach (var buildingVM in BuildingDisplayVMs)
            {
                buildingVM.IsNight = false;
            }
        }
    }
}
