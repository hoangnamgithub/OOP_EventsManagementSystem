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
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.ViewModel;

namespace OOP_EventsManagementSystem.View
{
    /// <summary>
    /// Interaction logic for Employee.xaml
    /// </summary>
    public partial class Employee : UserControl
    {
        public Employee()
        {
            InitializeComponent();
            DataContext = new EmployeeVM(new EventManagementDbContext());
        }
        private void Dtgrd_TodayEvent_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = sender as DataGrid;

            // Hit test to find the row being clicked
            var hitResult = VisualTreeHelper.HitTest(dataGrid, e.GetPosition(dataGrid));
            if (hitResult != null)
            {
                var row = ItemsControl.ContainerFromElement(dataGrid, hitResult.VisualHit) as DataGridRow;
                if (row != null)
                {
                    // Check if the row is already selected
                    if (dataGrid.SelectedItem == row.Item)
                    {
                        // Deselect the row by setting SelectedItem to null
                        dataGrid.SelectedItem = null;
                    }
                }
            }
        }

    }
}
