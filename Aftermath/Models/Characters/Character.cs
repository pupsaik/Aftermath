using Aftermath.Models.Characters.States;
using Aftermath.Models.Occupation;
using AftermathModels.Buildings;
using AftermathModels.Characters;
using AftermathModels.Characters.States;
using AftermathModels.Loot;
using AftermathModels.Observer;
using AftermathModels.Occupation;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

public class Character
{
    public string Name { get; set; }
    public int MaxHealth { get; set; }
    public int MaxHunger { get; set; }
    public int MaxThirst { get; set; }
    public int MaxSanity { get; set; }

    private int _currentHealth;
    private int _currentHunger;
    private int _currentThirst;
    private int _currentSanity;

    public event Action OnDeath;
    public event Action OnKill;

    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Math.Clamp(value, 0, MaxHealth);
            if (_currentHealth <= 0)
            {
                Die();
                Notify(CharacterEventType.Death, null);
            }

            Notify(CharacterEventType.HealthChanged, _currentHunger);
        }
    }

    public int CurrentHunger
    {
        get => _currentHunger;
        set
        {
            if (_currentHunger == 0 && value > 0)
            {
                RemoveState(StateType.Hungry);
                RemoveStatus(Status.Hungry);
            }
            _currentHunger = Math.Clamp(value, 0, MaxHunger);
            if (_currentHunger <= 0)
            {
                if (!_states.Any(s => s.State == StateType.Hungry) && !_newStates.Any(s => s.State == StateType.Hungry))
                {
                    AddState(new HungryState());
                    Statuses.Add(Status.Hungry);
                }
            }
            Notify(CharacterEventType.HungerChanged, _currentHunger);
        }
    }

    public int CurrentThirst
    {
        get => _currentThirst;
        set
        {
            if (_currentThirst == 0 && value > 0)
            {
                RemoveState(StateType.Thirsty);
                RemoveStatus(Status.Thirsty);
            }

            _currentThirst = Math.Clamp(value, 0, MaxThirst);
            if (_currentThirst <= 0)
            {
                if (!_states.Any(s => s.State == StateType.Thirsty) && !_newStates.Any(s => s.State == StateType.Thirsty))
                {
                    AddState(new ThirstyState());
                    Statuses.Add(Status.Thirsty);
                }
            }

            Notify(CharacterEventType.ThirstChanged, _currentThirst);
        }
    }

    public int CurrentSanity
    {
        get => _currentSanity;
        set
        {
            if (_currentSanity <= 0 && value > 0)
            {
                RemoveState(StateType.Insane);
                RemoveStatus(Status.Insane);
            }

            _currentSanity = Math.Clamp(value, 0, MaxSanity);
            if (_currentSanity <= 0)
            {
                if (!_states.Any(s => s.State == StateType.Insane) && !_newStates.Any(s => s.State == StateType.Insane))
                {
                    AddState(new InsaneState());
                    Statuses.Add(Status.Insane);
                }
            }

            Notify(CharacterEventType.SanityChanged, _currentSanity);
        }
    }

    public readonly ObservableCollection<Status> Statuses = [];

    public void RemoveStatus(Status status)
    {
        Statuses.Remove(status);
    }

    private readonly List<IState> _states = [new AliveState()];
    private readonly List<IState> _newStates = [];
    private readonly List<IState> _statesToRemove = [];

    public Character(string name, int maxHealth, int maxHunger, int maxThirst, int maxSanity)
    {
        Name = name;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        MaxHunger = maxHunger;
        CurrentHunger = maxHunger;
        MaxThirst = maxThirst;
        CurrentThirst = maxThirst;
        MaxSanity = maxSanity;
        CurrentSanity = maxSanity;
    }

    public void SkipToNextDay()
    {
        foreach (var state in _states)
        {
            state.SkipToNextDay(this);
        }

        _states.AddRange(_newStates);

        foreach (var state in _statesToRemove)
        {
            _states.Remove(state);
        }

        _newStates.Clear();
        _statesToRemove.Clear();
    }

    public void Die()
    {
        OnDeath?.Invoke();
        CurrentOccupation = new NoOccupation();
        _states.Clear();
        Statuses.Clear();
        Notify(CharacterEventType.Death, null);
    }

    public void RemoveState(StateType stateType)
    {
        var stateToRemove = _states.FirstOrDefault(x => x.State == stateType);
        _statesToRemove.Add(stateToRemove);
    }

    public void AddState(IState state)
    {
        _newStates.Add(state);
    }

    private readonly List<IObserver> observers = new();
    public void Attach(IObserver observer) => observers.Add(observer);
    public void Detach(IObserver observer) => observers.Remove(observer);

    public void Notify(CharacterEventType type, object data)
    {
        foreach (var observer in observers)
            observer.Update(type, data);
    }

    public void InsaneActivity()
    {
        OnKill?.Invoke();
    }

    public ITool ToolInHand { get; set; } = null;

    private IOccupation _currentOccupation = new NoOccupation();
    public IOccupation CurrentOccupation
    {
        get => _currentOccupation;
        set
        {
            _currentOccupation = value;
        }
    }

    public List<Skill> Skills { get; set; } = new();
}