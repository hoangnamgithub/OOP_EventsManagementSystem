using OOP_EventsManagementSystem.ViewModel;
using OOP_EventsManagementSystem.Model;
using System.Windows;
using System.Windows.Input;  // Make sure this namespace includes your DbContext

namespace OOP_EventsManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowVM _viewModel;

        // Add a constructor that accepts MainWindowVM
        public MainWindow(MainWindowVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            this.DataContext = _viewModel; // Set the DataContext to the viewModel
        }


        private void NavigateBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { if (e.ButtonState == MouseButtonState.Pressed) { this.DragMove(); } }


    }
}
