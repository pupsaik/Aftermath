using Aftermath.Models.Occupation;
using AftermathGameManaging;
using AftermathModels;
using AftermathModels.Buildings;
using AftermathModels.Loot;
using AftermathModels.Occupation;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aftermath.ViewModels
{
    public class BuildingActionNonOccupiedVM : BaseViewModel
    {
        private CharacterManager _characterManager;
        private Building _building;

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
        public ICommand OccupyBuildingCommand { get; }
        public ICommand CloseCommand { get; }

        public event Action CloseEvent;
        public event Action SubmitEvent;

        public BuildingActionNonOccupiedVM(Building building, CharacterManager characterManager, InventoryManager inventoryManager)
        {
            _characterManager = characterManager;

            _building = building;
            _isOccupied = _building.OccupiedCharacter != null ? true : false;

            CharacterList = _characterManager.GetCharacters().Where(ch => ch.CurrentHealth > 0 && ch.CurrentOccupation is NoOccupation).Select(x => new CharacterListElementVM(x)).ToList();
            ToolList = inventoryManager.GetItems().Where(x => x is ITool tool && tool.TileType == OccupationType.Rest && !tool.IsOccupied).Select(x => new ToolListElementVM(x as ITool)).ToList();

            OpenCharacterSelectionModalCommand = new RelayCommand(() => IsCharacterSelectionModalOpen = !IsCharacterSelectionModalOpen);
            OpenToolSelectionModalCommand = new RelayCommand(() => IsToolSelectionModalOpen = !IsToolSelectionModalOpen);
            OccupyBuildingCommand = new RelayCommand(OccupyBuildingMethod);

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

        private void OccupyBuildingMethod()
        {
            try
            {
                OccupationFacade occupationFacade = new();
                occupationFacade.OccupyBuilding(_building, CharacterSelectVM.Character, ToolSelectVM.Tool);
                CharacterSelectVM.HasError = false;
                SubmitEvent?.Invoke();
            }
            catch (Exception ex)
            {
                CharacterSelectVM.HasError = true;
            }
        }
    }

    public class ToolSelectVM : BaseViewModel
    {
        public ITool Tool { get; set; }

        private ImageSource _image;
        public ImageSource Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        public ToolSelectVM()
        {
            SetImage(null);
        }

        public void SetImage(ITool tool)
        {
            Tool = tool;

            if (Tool == null)
                Image = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/ItemSelectPlus.png"));
            else
                Image = new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/Items/{tool.Name.Replace(" ", "")}.png"));
        }
    }

    public class CharacterSelectVM : BaseViewModel
    {
        public Character Character { get; set; }

        private bool _hasError = false;
        public bool HasError
        {
            get => _hasError;
            set
            {
                _hasError = value;
                OnPropertyChanged(nameof(HasError));
            }
        }

        private ImageSource _image;
        public ImageSource Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        public CharacterSelectVM()
        {
            SetImage(null);
        }

        public void SetImage(Character character)
        {
            Character = character;

            if (Character == null)
                Image = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/CharacterPlaceholder.png"));
            else
                Image = new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/{Character.Name}.png"));
        }
    }

    public class ToolListElementVM : BaseViewModel
    {
        private ITool _tool;
        public ImageSource Image => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/Items/{_tool.Name.Replace(" ", "")}.png"));

        public event Action<ITool> ToolSelectedEvent;

        public ICommand ToolSelectedCommand { get; }

        public ToolListElementVM(ITool tool)
        {
            _tool = tool;
            ToolSelectedCommand = new RelayCommand(() => ToolSelectedEvent?.Invoke(_tool));
        }
    }

    public class CharacterListElementVM : BaseViewModel
    {
        private Character _character;
        public ImageSource Image => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/{_character.Name}.png"));

        public event Action<Character> CharacterSelectedEvent;

        public ICommand CharacterSelectedCommand { get; }

        public CharacterListElementVM(Character character)
        {
            _character = character;
            CharacterSelectedCommand = new RelayCommand(() => CharacterSelectedEvent?.Invoke(_character));
        }
    }
}
