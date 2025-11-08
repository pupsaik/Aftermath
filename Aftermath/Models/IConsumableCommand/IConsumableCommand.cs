using AftermathGameManaging;
using AftermathModels.Loot;

namespace AftermathModels.IConsumableCommand
{
    public interface IConsumableCommand
    {
        void Execute();
        void Undo();
    }

    public class ConsumeCommand : IConsumableCommand
    {
        private readonly InventoryManager _inventoryManager;
        public Character Character { get; }
        public IConsumable Consumable { get; }
        private IConsumptionEffect _effect;

        public ConsumeCommand(Character character, IConsumable consumable, InventoryManager inventoryManager)
        {
            Character = character;
            Consumable = consumable;
            _inventoryManager = inventoryManager;
            _effect = Consumable.Effect;
        }

        public void Execute()
        {
            Consumable.Effect.Apply(Character);
            _inventoryManager.RemoveItem(Consumable);
        }

        public void Undo()
        {
            Consumable.Effect.Cancel(Character);
            _inventoryManager.AddItem(Consumable);
        }
    }
}
