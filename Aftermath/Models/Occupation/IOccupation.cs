using Aftermath.Models.Characters.States;
using AftermathModels.Map;
using AftermathModels.Observer;

namespace AftermathModels.Occupation
{
    public interface IOccupation
    {
        string Name { get; }
        int Duration { get; }
        int TimeLeft { get; set; }
        Character OccupiedCharacter { get; set; }
        TileLootTable TileLootTable { get; }
        OccupationType OccupationType { get; }
        List<DangerousEvent> DangerousEvents { get; }
        Status CharacterStatus { get; }

        public void Attach(IObserver observer);
        public void Detach(IObserver observer);
        public void Notify(CharacterEventType type, object data);
    }

    public class Exploration : IOccupation
    {
        public string Name { get; }
        public int Duration { get; }
        public List<DangerousEvent> DangerousEvents { get; }
        public Status CharacterStatus { get; }

        private int _timeLeft;
        public int TimeLeft
        {
            get => _timeLeft;
            set
            {
                _timeLeft = value;
                if (_timeLeft <= 0)
                {
                    Notify(CharacterEventType.OccupationEnded, null);
                    ClearObservers();
                }
            }
        }
        public Character OccupiedCharacter { get; set; }

        public TileLootTable TileLootTable { get; }
        public OccupationType OccupationType { get; }

        public Exploration(string name, int duration, TileLootTable tileLootTable,
            OccupationType occupationType, Status status, List<DangerousEvent> dangerousEvents)
        {
            Name = name;
            Duration = duration;
            TimeLeft = duration;
            OccupiedCharacter = null;
            TileLootTable = tileLootTable;
            OccupationType = occupationType;
            DangerousEvents = dangerousEvents;
            CharacterStatus = status;
        }

        private List<IObserver> observers = [];

        public void Attach(IObserver observer) => observers.Add(observer);
        public void Detach(IObserver observer) => observers.Remove(observer);
        public void ClearObservers() => observers.Clear();

        public void Notify(CharacterEventType type, object data)
        {
            foreach (var observer in observers)
                observer.Update(type, data);
        }
    }

    [Flags]
    public enum OccupationType
    {
        None = 0,
        Forest = 1 << 0,
        CampingSite = 1 << 1,
        HuntersHut = 1 << 2,
        Rest = 1 << 3,
        FishingDock = 1 << 4,
    }
}
