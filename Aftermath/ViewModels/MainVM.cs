using AftermathGameManaging;
using Aftermath.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AftermathModels.Map;

namespace Aftermath.ViewModels
{
    public class MainVM : BaseViewModel
    {
        private BaseViewModel _currentVM;
        public BaseViewModel CurrentVM
        {
            get => _currentVM;
            set
            {
                _currentVM = value;
                OnPropertyChanged(nameof(CurrentVM));
            }
        }

        public StartVM StartVM { get; private set; }

        public BaseVM BaseVM { get; private set; }
        public MapVM MapVM { get; private set; }
        public InventoryVM InventoryVM { get; private set; }
        public BuildingActionVM BuildingActionVM { get; private set; }

        public Game Game { get; }

        public MainVM()
        {
            StartVM = new(this);
            CurrentVM = StartVM; 

            Game = new Game();

            BaseVM = new(this, Game.TimeManager, Game.Buildings, Game.CharacterManager, Game.Inventory, Game.EventManager);
            InventoryVM = new(this, Game.CharacterManager, Game.Inventory);
            BuildingActionVM = new(Game.CharacterManager, Game.Inventory);
        }

        public void DifficultySelected(ITileFactory tileFactory)
        {
            Game.InitializeMap(tileFactory);
            MapVM = new(this, Game.MapManager, Game.Inventory, Game.CharacterManager);

            CurrentVM = BaseVM;
        }
    }
}
