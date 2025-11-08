using Aftermath.Models.Characters.States;
using AftermathModels.Characters;
using AftermathModels.Observer;
using AftermathModels.Occupation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Aftermath.ViewModels
{
    public class CharacterVM : BaseViewModel
    {
        private Character _character;

        public ObservableCollection<Status> Statuses { get; set; } = [];

        public int CurrentHealth
        {
            get => _character.CurrentHealth;
            set
            {
                _character.CurrentHealth = value;
                OnPropertyChanged(nameof(CurrentHealth));
                OnPropertyChanged(nameof(HealthBarIcon));
            }
        }

        public int CurrentHunger
        {
            get => _character.CurrentHunger;
            set
            {
                _character.CurrentHunger = value;
                OnPropertyChanged(nameof(CurrentHunger));
            }
        }

        public int CurrentThirst
        {
            get => _character.CurrentThirst;
            set
            {
                _character.CurrentThirst = value;
                OnPropertyChanged(nameof(CurrentThirst));
            }
        }

        public string Name
        {
            get => _character.Name;
            set
            {
                _character.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int CurrentSanity
        {
            get => _character.CurrentSanity;
            set
            {
                _character.CurrentSanity = value;
                OnPropertyChanged(nameof(CurrentSanity));
            }
        }

        private bool _isDead = false;
        public bool IsDead
        {
            get => _isDead;
            set
            {
                _isDead = value;
                OnPropertyChanged(nameof(IsDead));
            }
        }

        public ImageSource HealthBarIcon
        {
            get
            {
                int healthPercent = (int)((double)CurrentHealth / _character.MaxHealth * 100);
                string iconName;

                if (healthPercent >= 100)
                    iconName = "HealthBar100";
                else if (healthPercent >= 75)
                    iconName = "HealthBar75";
                else if (healthPercent >= 50)
                    iconName = "HealthBar50";
                else if (healthPercent >= 25)
                    iconName = "HealthBar25";
                else
                    iconName = "HealthBar0";

                return new ImageSourceConverter().ConvertFromString(
                    $"pack://application:,,,/Resources/Icons/{iconName}.png") as ImageSource;
            }
        }

        public ImageSource HungerBarIcon
        {
            get
            {
                int percent = (int)((double)CurrentHunger / _character.MaxHunger * 100);
                string iconName;

                if (percent >= 100)
                    iconName = "HungerBar100";
                else if (percent >= 75)
                    iconName = "HungerBar75";
                else if (percent >= 50)
                    iconName = "HungerBar50";
                else if (percent >= 25)
                    iconName = "HungerBar25";
                else
                    iconName = "HungerBar0";

                return new ImageSourceConverter().ConvertFromString(
                    $"pack://application:,,,/Resources/Icons/{iconName}.png") as ImageSource;
            }
        }

        public ImageSource ThirstBarIcon
        {
            get
            {
                int percent = (int)((double)CurrentThirst / _character.MaxThirst * 100);
                string iconName;

                if (percent >= 100)
                    iconName = "ThirstBar100";
                else if (percent >= 75)
                    iconName = "ThirstBar75";
                else if (percent >= 50)
                    iconName = "ThirstBar50";
                else if (percent >= 25)
                    iconName = "ThirstBar25";
                else
                    iconName = "ThirstBar0";

                return new ImageSourceConverter().ConvertFromString(
                    $"pack://application:,,,/Resources/Icons/{iconName}.png") as ImageSource;
            }
        }

        public ImageSource SanityBarIcon
        {
            get
            {
                int percent = (int)((double)CurrentSanity / _character.MaxSanity * 100);
                string iconName;

                if (percent >= 100)
                    iconName = "SanityBar100";
                else if (percent >= 75)
                    iconName = "SanityBar75";
                else if (percent >= 50)
                    iconName = "SanityBar50";
                else if (percent >= 25)
                    iconName = "SanityBar25";
                else
                    iconName = "SanityBar0";

                return new ImageSourceConverter().ConvertFromString(
                    $"pack://application:,,,/Resources/Icons/{iconName}.png") as ImageSource;
            }
        }


        public CharacterVM(Character character)
        {
            _character = character;
            _character.Attach(new CharacterStatusIconChangeObserver(this));
            _character.Attach(new CharacterNeedChangeObserver(this));
            _character.Attach(new CharacterDeathObserver(this));

            _character.Statuses.CollectionChanged += test;
        }

        private void test(object sender, NotifyCollectionChangedEventArgs o)
        {
            Statuses = _character.Statuses;
            OnPropertyChanged(nameof(Statuses));
        }
    }
}
