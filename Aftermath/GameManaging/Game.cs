using Aftermath.GameManaging;
using Aftermath.UI.Sound;
using AftermathModels.Loot;
using AftermathModels.Map;
using AftermathModels.Map.Factories;

namespace AftermathGameManaging
{
    public class Game
    {
        public CharacterManager CharacterManager { get; set; }
        public TimeManager TimeManager { get; set; }
        public InventoryManager Inventory { get; private set; } = InventoryManager.Instance;

        public Dictionary<ResourceType, Resource> BuildingResources { get; set; }
        public BuildingManager Buildings { get; set; }
        public MapManager MapManager { get; set; }
        public EventManager EventManager { get; set; }

        public Game()
        {
            EventManager = new();

            CharacterManager.Initialize(EventManager);
            CharacterManager = CharacterManager.Instance;

            TimeManager = new(CharacterManager, EventManager);

            Buildings = new();

            //Inventory.AddItem(new Axe());
            //Inventory.AddItem(new CannedMeat());
            //Inventory.AddItem(new Berries());
            //Inventory.AddItem(new Berries());
            //Inventory.AddItem(new Mushrooms());
            //Inventory.AddItem(new FreshWater());
            //Inventory.AddItem(new UnprocessedWater());
            //Inventory.AddItem(new Fish());
            //Inventory.AddItem(new Carrot());
            //Inventory.AddItem(new Bandage());
            //Inventory.AddItem(new Machete());
            //Inventory.AddItem(new Book());
            //Inventory.AddItem(new FirstAidManual());
            //Inventory.AddItem(new Flashlight());
            //Inventory.AddItem(new FishingRod());
            //Inventory.AddItem(new Sunscreen());

            BuildingResources = new()
            {
                { ResourceType.Wood, new Resource(ResourceType.Wood, 3) },
                { ResourceType.Stone, new Resource(ResourceType.Stone, 2) },
                { ResourceType.Metal, new Resource(ResourceType.Metal, 0) },
                { ResourceType.Rope, new Resource(ResourceType.Rope, 3) },
            };
        }

        public void InitializeMap(ITileFactory tileFactory)
        {
            MapManager.Initialize(tileFactory);
            MapManager = MapManager.Instance;
        }
    }
}
