using Aftermath.GameManaging;
using Aftermath.Models.Characters.States;
using AftermathModels.Buildings;
using AftermathModels.Loot;
using AftermathModels.Map;
using AftermathModels.Observer;
using AftermathModels.Occupation;
using System.Windows.Data;

namespace AftermathGameManaging
{
    public class OccupationFacade
    {
        private readonly CharacterManager _characterManager;
        private readonly BuildingManager _buildingManager;
        private readonly InventoryManager _inventoryManager;
        private readonly MapManager _mapManager;

        public OccupationFacade()
        {
            _characterManager = CharacterManager.Instance;
            _buildingManager = new();
            _inventoryManager = InventoryManager.Instance;
            _mapManager = MapManager.Instance;
        }

        public void OccupyBuilding(Building building, Character character, ITool tool)
        {
            _characterManager.EquipCharacter(character, tool);
            _inventoryManager.OccupyTool(tool);
            _characterManager.OccupyCharacter(character, building);
            _buildingManager.OccupyBuilding(building, character);

            character.Statuses.Add(building.CharacterStatus);
        }

        public void OccupyTile(Tile tile, Character character, ITool tool)
        {
            _characterManager.EquipCharacter(character, tool);
            _inventoryManager.OccupyTool(tool);
            _characterManager.OccupyCharacter(character, tile.Occupation);
            _mapManager.OccupyTile(tile, character);

            character.Statuses.Add(tile.Occupation.CharacterStatus);

            tile.Occupation.Attach(new OccupationEndObserver(tile.Occupation));
        }

        public void GetProfitFromOccupation(IOccupation occupation, Character character)
        {
            foreach (var profit in occupation.TileLootTable.Drops)
            {
                Random random = new();
                int addedCount = 0;
                for (int i = 0; i < profit.MaxAmount; i++)
                {
                    int randomValue = random.Next(0, 100);
                    if (randomValue <= profit.GetEffectiveDropChance(character.ToolInHand))
                    {
                        profit.AcceptDrop(character, _inventoryManager);
                        addedCount++;
                    }
                }

                if (addedCount < profit.MinAmount)
                {
                    for (int i = 0; i < profit.MinAmount; i++)
                        profit.AcceptDrop(character, _inventoryManager);
                }
            }


            foreach (var hazard in occupation.DangerousEvents)
            {
                Random rand = new();
                double randomValue = rand.Next(0, 100);

                if (randomValue <= hazard.GetEffectiveProbability(character.ToolInHand))
                {
                    _inventoryManager.HazardsOfDay.Add((hazard, occupation.OccupiedCharacter));
                    hazard.Apply(occupation.OccupiedCharacter);
                    break;
                }
            }

            foreach (IItem loot in _inventoryManager.LootOfDay)
                _inventoryManager.AddItem(loot);

            character.RemoveStatus(occupation.CharacterStatus);
        }
    }
}
