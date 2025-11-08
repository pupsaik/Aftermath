using AftermathGameManaging;
using Aftermath.ViewModels;
using System.Windows;

namespace Aftermath
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainVM mainVM = new MainVM();
            mainVM.StartVM.OnChoseFactory += mainVM.DifficultySelected;

            mainVM.BaseVM.CloseGameVM.OnCloseGame += Close;
            mainVM.StartVM.OnClose += Close;
            DataContext = mainVM;
        }
    }
}