using Aftermath.Models.Occupation;
using AftermathGameManaging;
using AftermathModels;
using AftermathModels.Loot;
using AftermathModels.Map;
using AftermathModels.Observer;
using AftermathModels.Occupation;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aftermath.ViewModels
{
    public class ExpeditionSelectNonOccupiedVM : BaseViewModel
    {
        private MapVM _mapVM;
        private CharacterManager _characterManager;
        private MapTileVM _tileVM;

        public CharacterSelectVM CharacterSelectVM { get; } = new();
        public ToolSelectVM ToolSelectVM { get; } = new();

        public List<CharacterListElementVM> CharacterList { get; }
        public List<ToolListElementVM> ToolList { get; }

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

        private bool _isCharacterSelectionModalOpen;
        public bool IsCharacterSelectionModalOpen
        {
            get => _isCharacterSelectionModalOpen;
            set
            {
                _isCharacterSelectionModalOpen = value;
                OnPropertyChanged(nameof(IsCharacterSelectionModalOpen));
            }
        }

        private bool _isToolSelectionModalOpen;
        public bool IsToolSelectionModalOpen
        {
            get => _isToolSelectionModalOpen;
            set
            {
                _isToolSelectionModalOpen = value;
                OnPropertyChanged(nameof(IsToolSelectionModalOpen));
            }
        }

        public ICommand OpenCharacterSelectionModalCommand { get; }
        public ICommand OpenToolSelectionModalCommand { get; }
        public ICommand OccupyTileCommand { get; }
        public ICommand CloseCommand { get; }

        public event Action CloseEvent;
        public event Action SubmitEvent;

        public ExpeditionSelectNonOccupiedVM(MapTileVM tile, CharacterManager characterManager, InventoryManager inventoryManager, MapVM mapVM)
        {
            _characterManager = characterManager;
            _mapVM = mapVM;

            _tileVM = tile;
            _isOccupied = tile.Tile.Occupation.OccupiedCharacter != null ? true : false;

            CharacterList = _characterManager.GetCharacters().Where(ch => ch.CurrentHealth > 0 && ch.CurrentOccupation is NoOccupation).Select(x => new CharacterListElementVM(x)).ToList();
            var debugAxe = inventoryManager.GetItems().FirstOrDefault(x => x is Axe);
            ToolList = inventoryManager.GetItems()
                .Where(x => x is ITool tool
                    && (tool.TileType & tile.Tile.Occupation.OccupationType) != 0
                    && !tool.IsOccupied)
                .Select(x => new ToolListElementVM(x as ITool))
                .ToList();
            OpenCharacterSelectionModalCommand = new RelayCommand(() => IsCharacterSelectionModalOpen = !IsCharacterSelectionModalOpen);
            OpenToolSelectionModalCommand = new RelayCommand(() => IsToolSelectionModalOpen = !IsToolSelectionModalOpen);
            OccupyTileCommand = new RelayCommand(OccupyTileMethod);

            CloseCommand = new RelayCommand(() => CloseEvent?.Invoke());

            foreach (var character in CharacterList)
            {
                character.CharacterSelectedEvent += (c) =>
                {
                    CharacterSelectVM.SetImage(c);
                    IsCharacterSelectionModalOpen = false;
                };
            }


            foreach (var tool in ToolList)
            {
                tool.ToolSelectedEvent += (t) =>
                {
                    ToolSelectVM.SetImage(t);
                    IsToolSelectionModalOpen = false;
                };
            }
        }

        private void OccupyTileMethod()
        {
            try
            {
                OccupationFacade occupationFacade = new();
                occupationFacade.OccupyTile(_tileVM.Tile, CharacterSelectVM.Character, ToolSelectVM.Tool);
                _tileVM.OccupiedCharacterIcon = new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/{CharacterSelectVM.Character.Name}.png"));

                CharacterSelectVM.HasError = false;

                _tileVM.Tile.Occupation.Attach(new ExplorationEndedObserver(_tileVM, _mapVM));
                SubmitEvent?.Invoke();
            }
            catch (Exception ex)
            {
                CharacterSelectVM.HasError = true;
            }
        }
    }
}
