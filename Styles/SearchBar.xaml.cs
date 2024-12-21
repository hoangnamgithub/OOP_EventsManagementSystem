using System.Windows;
using System.Windows.Controls;
using OOP_EventsManagementSystem.ViewModel;

namespace OOP_EventsManagementSystem.Styles
{
    public partial class SearchBar : UserControl
    {
        public SearchBar()
        {
            InitializeComponent();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is EmployeeVM viewModel)
            {
                viewModel.SearchEmployees(SearchBox.Text);
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Search......")
            {
                SearchBox.Text = "";
                SearchBox.CaretBrush = System.Windows.Media.Brushes.Black;
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Search......";
                SearchBox.CaretBrush = System.Windows.Media.Brushes.Transparent;
            }
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            // Implement edit functionality here
            MessageBox.Show("Edit button clicked");
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            // Implement delete functionality here
            MessageBox.Show("Delete button clicked");
        }
    }
}
