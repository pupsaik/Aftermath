using AftermathGameManaging;
using AftermathModels.Loot;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using AftermathModels.Map;
using System.ComponentModel;
using Aftermath.GameManaging;
using System.DirectoryServices.ActiveDirectory;

namespace Aftermath.ViewModels
{
    public class DayResultsVM : BaseViewModel
    {
        private BaseVM _baseVM;
        private InventoryManager _inventoryManager;
        private EventManager _eventManager;

        public List<ResultRewardVM> NewLoot { get; set; }
        public List<DangerousEventVM> NewHazards { get; set; }
        public List<IEventVM> NewEvents { get; set; }

        private bool _isNewLootEmpty;
        private bool _isNewHazardsEmpty;
        private bool _isNewEventsEmpty;
        private bool _didNothingHappen;

        public bool IsNewLootEmpty
        {
            get => _isNewLootEmpty;
            set
            {
                _isNewLootEmpty = value;
                OnPropertyChanged(nameof(IsNewLootEmpty));
            }
        }

        public bool IsNewHazardsEmpty
        {
            get => _isNewHazardsEmpty;
            set
            {
                _isNewHazardsEmpty = value;
                OnPropertyChanged(nameof(IsNewHazardsEmpty));
            }
        }

        public bool IsNewEventsEmpty
        {
            get => _isNewEventsEmpty;
            set
            {
                _isNewEventsEmpty = value;
                OnPropertyChanged(nameof(IsNewEventsEmpty));
            }
        }

        public bool DidNothingHappen
        {
            get => _didNothingHappen;
            set
            {
                _didNothingHappen = value;
                OnPropertyChanged(nameof(DidNothingHappen));
            }
        }

        public ICommand CloseCommand { get; }

        public DayResultsVM(InventoryManager inventoryManager, EventManager eventManager, BaseVM baseVM)
        {
            _inventoryManager = inventoryManager;
            _baseVM = baseVM;
            _eventManager = eventManager;
            CloseCommand = new RelayCommand(() => {
                _baseVM.IsDayResultsVisible = false;
                _baseVM.SetToDay();
            }); 
        }

        public void Update()
        {
            if (_inventoryManager.LootOfDay != null)
            {
                NewLoot = new(_inventoryManager.LootOfDay.Select(x => new ResultRewardVM(x)));
                NewHazards = new(_inventoryManager.HazardsOfDay.Select(de => new DangerousEventVM(de.Character, de.DangerousEvent)));
                NewEvents = new(_eventManager.Events.Select(e => {
                    if (e is Murder murder)
                        return new MurderEventVM(murder);

                    return new MurderEventVM(e as Murder);
                }));
                OnPropertyChanged(nameof(NewLoot));
                OnPropertyChanged(nameof(NewHazards));
                OnPropertyChanged(nameof(NewEvents));

                IsNewLootEmpty = NewLoot.Count == 0;
                IsNewHazardsEmpty = NewHazards.Count == 0;
                IsNewEventsEmpty = NewEvents.Count == 0;

                DidNothingHappen = IsNewLootEmpty && IsNewHazardsEmpty && IsNewEventsEmpty;

                _inventoryManager.LootOfDay.Clear();
                _inventoryManager.HazardsOfDay.Clear();
                _eventManager.Events.Clear();
            }
            else
            {
                _baseVM.IsDayResultsVisible = false;
            }
        }
    }

    public class ResultRewardVM : BaseViewModel
    {
        public string Type { get; }
        public int Amount { get; set; }
        public ImageSource Image => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/Items/{Type.Replace(" ", "")}.png"));

        public ResultRewardVM(IItem item)
        {
            Type = item.Name;
            Amount = 1;
        }
    }

    public class DangerousEventVM
    {
        private Character _character;
        private DangerousEvent _dangerousEvent;
        private ConsumptionEffect _consumptionEffect;

        public ImageSource CharacterIcon => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/{_character.Name}.png"));
        public ImageSource DangerousEventIcon => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/{Type}.png"));
        public string Type { get; }
        public string Name { get; }
        public int ImpactAmount { get; }
        public ImageSource ImpactIcon => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/Items/{_consumptionEffect.Type}.png"));

        public DangerousEventVM(Character character, DangerousEvent dangerousEvent)
        {
            _character = character;
            _dangerousEvent = dangerousEvent;

            Type = _dangerousEvent.Type.ToString();
            Name = _dangerousEvent.Name;

            _consumptionEffect = _dangerousEvent.Effect as ConsumptionEffect;
            ImpactAmount = _consumptionEffect.Amount;
            
        }
    }

    public interface IEventVM
    {
        EventVMType Type { get; }
    }

    public class MurderEventVM : BaseViewModel, IEventVM
    {
        private Murder _murder;

        public EventVMType Type => EventVMType.MurderVM;

        public ImageSource KillerIcon => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/{_murder.Subject.Name}.png"));
        public ImageSource VictimIcon => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/{_murder.Victim.Name}.png"));
        public string Description => _murder.Description;

        public bool IsNotSuicide { get; set; }

        public MurderEventVM(bool isNotSuicide)
        {
            IsNotSuicide = isNotSuicide;
        }

        public MurderEventVM(Murder murder)
        {
            _murder = murder;

            IsNotSuicide = !(_murder.Subject.Name == _murder.Victim.Name);

            OnPropertyChanged(nameof(IsNotSuicide));
        }
    }

    public enum EventVMType
    {
        MurderVM
    }
}
