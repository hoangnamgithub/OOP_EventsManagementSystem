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
using OOP_EventsManagementSystem.Utilities;

namespace OOP_EventsManagementSystem.Styles
{
    /// <summary>
    /// Interaction logic for EventDescription.xaml
    /// </summary>
    public partial class EventDescription : Window
    {
        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }
        public EventDescription()
        {
            InitializeComponent();
            ConfirmCommand = new RelayCommand(ExecuteConfirmCommand); 
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
            this.DataContext = this;
        }
        private void ExecuteConfirmCommand(object parameter)
        { 

        } 
        private void ExecuteCancelCommand(object parameter) 
        { 
            this.Close();
        }
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e) { if (SearchBox.Text == "Search......") { SearchBox.Text = ""; SearchBox.CaretBrush = System.Windows.Media.Brushes.Black; } }
        private void SearchBox_LostFocus(object sender, RoutedEventArgs e) { if (string.IsNullOrWhiteSpace(SearchBox.Text)) { SearchBox.Text = "Search......"; SearchBox.CaretBrush = System.Windows.Media.Brushes.Transparent; } }
        private void eventname_GotFocus(object sender, RoutedEventArgs e) { if (eventname_txtbox.Text == "Name here") { eventname_txtbox.Text = ""; eventname_txtbox.CaretBrush = System.Windows.Media.Brushes.Black; } }
        private void eventname_LostFocus(object sender, RoutedEventArgs e) { if (string.IsNullOrWhiteSpace(eventname_txtbox.Text)) { eventname_txtbox.Text = "Name here"; eventname_txtbox.CaretBrush = System.Windows.Media.Brushes.Transparent; } }
        private void des_GotFocus(object sender, RoutedEventArgs e) { if (des_txtbox.Text == "Description here") { des_txtbox.Text = ""; des_txtbox.CaretBrush = System.Windows.Media.Brushes.Black; } }
        private void des_LostFocus(object sender, RoutedEventArgs e) { if (string.IsNullOrWhiteSpace(des_txtbox.Text)) { des_txtbox.Text = "Description here"; des_txtbox.CaretBrush = System.Windows.Media.Brushes.Transparent; } }
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EventDescription_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { if (e.ButtonState == MouseButtonState.Pressed) { this.DragMove(); } }
    }
}
