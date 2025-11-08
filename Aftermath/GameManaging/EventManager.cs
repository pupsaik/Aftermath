using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aftermath.GameManaging
{
    public class EventManager
    {
        public List<Event> Events = [];

        public void Add(Event e)
        {
            Events.Add(e);
        }

        public void Remove(Event e)
        {
            Events.Remove(e);
        }

        public void ExecuteAll(IEnumerable<Character> characters)
        {
            foreach (var e in Events)
            {
                e.Execute(characters);
            }
        }
    }

    public abstract class Event
    {
        public Character Subject { get; }

        public abstract string Description { get; set; }

        protected Event(Character subject)
        {
            Subject = subject;
        }

        public abstract void Execute(IEnumerable<Character> characters);
    }

    public class Murder : Event
    {
        public Character Victim { get; set; }

        public Murder(Character subject) : base(subject) { }

        public override string Description { get; set; }

        public override void Execute(IEnumerable<Character> characters)
        {
            var potentialVictims = characters
                .Where(c => c.CurrentHealth > 0)
                .ToList();

            if (potentialVictims.Count == 0)
                return;

            var random = new Random();
            Victim = potentialVictims[random.Next(potentialVictims.Count)];

            Victim.CurrentHealth = 0;

            if (Victim.Name == Subject.Name)
                Description = $" went crazy and commited suicide!";
            else
                Description = $" went crazy and murdered ";
        }
    }
}
