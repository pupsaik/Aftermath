using Aftermath.GameManaging;
using AftermathModels.Loot;
using AftermathModels.Map;
using AftermathModels.Occupation;

namespace AftermathGameManaging
{
    public class CharacterManager
    {
        private readonly List<Character> Characters;

        public EventManager EventManager { get; }

        private static CharacterManager _instance;

        public static CharacterManager Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("CharacterManager is not initialized. Call Initialize() first.");
                return _instance;
            }
        }

        public static void Initialize(EventManager eventManager)
        {
            if (_instance != null)
                throw new InvalidOperationException("CharacterManager already initialized.");

            if (_instance == null)
            {
                _instance = new CharacterManager(eventManager);
            }
        }


        private CharacterManager(EventManager eventManager)
        {
            EventManager = eventManager;

            Characters = [
                new Character("Rob", 90, 100, 100, 100),
                new Character("Beth", 100, 100, 110, 120),
                new Character("Adam", 120, 100, 120, 80),
                new Character("Stacy", 110, 100, 110, 90),
            ];

            foreach (Character character in Characters)
            {
                character.OnKill += () => EventManager.Add(new Murder(character));
            }
        }

        public void OccupyCharacter(Character character, IOccupation building)
        {
            if (character.CurrentOccupation != null)
                character.CurrentOccupation = building;
        }

        public void EquipCharacter(Character character, ITool tool)
        {
            if (character != null)
                character.ToolInHand = tool;
        }

        public IEnumerable<Character> GetCharacters() => new List<Character>(Characters);
    }
}
