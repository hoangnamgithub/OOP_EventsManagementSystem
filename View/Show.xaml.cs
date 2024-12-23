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
using OOP_EventsManagementSystem.Styles;
using OOP_EventsManagementSystem.ViewModel;

namespace OOP_EventsManagementSystem.View
{
    /// <summary>
    /// Interaction logic for Show.xaml
    /// </summary>
    public partial class Show : UserControl
    {
        public Show()
        {
            InitializeComponent();
            var viewModel = new ShowVM();
            this.DataContext = viewModel;
            viewModel.LoadPerformers();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SponsorDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }

        private void SponsorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ConfirmAddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SponsorNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ConfirmAddExistButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
