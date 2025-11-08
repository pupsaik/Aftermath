using AftermathModels.Loot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aftermath.ViewModels
{
    public interface IInventoryItemVM
    {
        ImageSource ItemIcon { get; }
        string Name { get; }
        string Description { get; }
        InventoryItemVMType Type { get; }
        List<IInventoryItemEffectVM> ItemEffectVMs { get; }
    }

    public class InventoryConsumableVM : IInventoryItemVM
    {
        public ImageSource ItemIcon => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/Items/{InventoryItem.Name.Replace(" ", "")}.png"));
        public string Name => InventoryItem.Name;
        public string Description => InventoryItem.Description;
        public InventoryItemVMType Type => InventoryItemVMType.ConsumableVM;

        public List<IInventoryItemEffectVM> ItemEffectVMs { get; } = [];

        public IConsumable InventoryItem { get; }

        public InventoryConsumableVM(IConsumable inventoryItem)
        {
            InventoryItem = inventoryItem;

            if (InventoryItem.Effect is CompositeEffect compositeEffect)
            {
                foreach (var effect in compositeEffect.Effects)
                {
                    ItemEffectVMs.Add(new InventoryConsumableItemEffectVM(effect as ConsumptionEffect));
                }
            }
            else if (InventoryItem.Effect is ConsumptionEffect consumptionEffect)
            {
                ItemEffectVMs.Add(new InventoryConsumableItemEffectVM(consumptionEffect));
            }
        }
    }

    public class InventoryToolVM : IInventoryItemVM
    {
        public ImageSource ItemIcon => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/Items/{_inventoryItem.Name.Replace(" ", "")}.png"));

        public string Name => _inventoryItem.Name;
        public string Description => _inventoryItem.Description;
        public InventoryItemVMType Type => InventoryItemVMType.ToolVM;

        public ICommand ItemClickedCommand { get; }

        public List<InventoryConsumableItemEffectVM> Effects { get; } = [];

        private ITool _inventoryItem { get; }

        public List<IInventoryItemEffectVM> ItemEffectVMs => throw new NotImplementedException();

        public InventoryToolVM(ITool inventoryItem)
        {
            _inventoryItem = inventoryItem;
        }
    }

    public interface IInventoryItemEffectVM
    {

    }

    public class InventoryConsumableItemEffectVM : IInventoryItemEffectVM
    {
        private ConsumptionEffect _consumptionEffect;

        public string Type => _consumptionEffect.Type.ToString();
        public int Amount { get; set; }
        public ImageSource Image => new BitmapImage(new Uri($"pack://application:,,,/Resources/Icons/Items/{_consumptionEffect.Type}.png"));

        public InventoryConsumableItemEffectVM(ConsumptionEffect consumptionEffect)
        {
            _consumptionEffect = consumptionEffect;
            Amount = _consumptionEffect.Amount;
        }
    }

    public enum InventoryItemVMType
    {
        ToolVM,
        ConsumableVM
    }
}
