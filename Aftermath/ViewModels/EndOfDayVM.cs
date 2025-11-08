using Aftermath.Models.Occupation;
using AftermathGameManaging;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aftermath.ViewModels
{
    public class EndOfDayVM : BaseViewModel
    {
        private BaseVM _baseVM;
        private CharacterManager _characterManager;
        private TimeManager _timeManager;

        private ObservableCollection<UnoccupiedCharacterVM> _unoccupiedCharacters;
        public ObservableCollection<UnoccupiedCharacterVM> UnoccupiedCharacters
        {
            get => _unoccupiedCharacters;
            set
            {
                _unoccupiedCharacters = value;
                OnPropertyChanged(nameof(UnoccupiedCharacters));
            }
        }

        public int DesignHeight => IsAnyCharacterFree ? 175 : 150;
        public bool IsAnyCharacterFree { get; set; }
        public string EndDayButtonText => IsAnyCharacterFree ? "Proceed anyway" : "Proceed";

        public ICommand EndDayCommand { get; }
        public ICommand CancelCommand { get; }

        public EndOfDayVM(CharacterManager characterManager, BaseVM baseVM, TimeManager timeManager)
        {
            _characterManager = characterManager;
            _timeManager = timeManager;
            _baseVM = baseVM;
            EndDayCommand = new RelayCommand(EndDayMethod);
            CancelCommand = new RelayCommand(() => {
                _baseVM.IsEndOfDayVisible = false;
            });
        }

        public void Update()
        {
            UnoccupiedCharacters = new(_characterManager.GetCharacters().Where(ch => ch.CurrentHealth > 0 && ch.CurrentOccupation is NoOccupation).Select(x => new UnoccupiedCharacterVM(x)));
            IsAnyCharacterFree = UnoccupiedCharacters.Any();

            OnPropertyChanged(nameof(EndDayButtonText));
            OnPropertyChanged(nameof(IsAnyCharacterFree));
            OnPropertyChanged(nameof(DesignHeight));
        }

        private void EndDayMethod()
        {
            _timeManager.SkipToNextDay();
            _baseVM.IsEndOfDayVisible = false;
            _baseVM.ShowDayResults();
            _baseVM.SetToNight();
        }
    }

    public class UnoccupiedCharacterVM
    {
        private Character _character;

        public ImageSource IconPath => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/{_character.Name}.png"));

        public UnoccupiedCharacterVM(Character character)
        {
            _character = character;
        }
    }
}
