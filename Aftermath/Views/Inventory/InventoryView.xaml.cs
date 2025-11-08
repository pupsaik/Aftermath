using Aftermath.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aftermath.Views.Inventory
{
    /// <summary>
    /// Interaction logic for InventoryView.xaml
    /// </summary>
    public partial class InventoryView : UserControl
    {
        public InventoryView()
        {
            InitializeComponent();
        }

        private void RootGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is FrameworkElement element && DataContext is InventoryVM vm && element.DataContext is IInventoryItemVM item)
            {
                vm.HoveredItem = item;
            }
        }

        private void RootGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (DataContext is InventoryVM vm)
            {
                vm.HoveredItem = null;
            }
        }

        private void RootGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && DataContext is InventoryVM vm && element.DataContext is IInventoryItemVM item)
            {
                if (item is InventoryConsumableVM consumableVM)
                {
                    vm.SelectedConsumable = consumableVM.InventoryItem;
                    vm.IsConsumeItemWindowOpened = true;
                    vm.ConsumeItemVM = new(vm.SelectedConsumable, vm.CharacterManager, vm.InventoryManager);
                    vm.ConsumeItemVM.CloseEvent += () => vm.IsConsumeItemWindowOpened = false;

                }
            }
        }

    }
}
