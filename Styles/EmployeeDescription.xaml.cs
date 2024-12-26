using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.ViewModel;
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
using System.Windows.Shapes;

namespace OOP_EventsManagementSystem.Styles
{
    /// <summary>
    /// Interaction logic for EmployeeDescription.xaml
    /// </summary>
    public partial class EmployeeDescription : Window
    {
        public EmployeeDescription()
        {
            InitializeComponent();
            DataContext = new EmployeeVM(new EventManagementDbContext());
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) { this.DragMove(); }
        }

        private void txtbox_empName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is EmployeeVM viewModel && viewModel.GeneratePasswordCommand.CanExecute(null))
            {
                viewModel.GeneratePasswordCommand.Execute(null);
            }
        }
    }
}
