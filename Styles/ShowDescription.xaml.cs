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
    /// Interaction logic for ShowDescription.xaml
    /// </summary>
    public partial class ShowDescription : Window
    {
        public ShowDescription()
        {
            InitializeComponent();
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { if (e.ButtonState == MouseButtonState.Pressed) { this.DragMove(); } }

        private void btn_addPerformer_Click(object sender, RoutedEventArgs e)
        {
            addPerformer.Visibility = Visibility.Visible; // Thay đổi thuộc tính Visibility
            gridbtn_add.Visibility = Visibility.Collapsed;
            gridbtn_close.Visibility = Visibility.Visible;
            performerchoose.Visibility = Visibility.Collapsed;
        }

        private void btn_closePerformer_Click(object sender, RoutedEventArgs e)
        {
            addPerformer.Visibility = Visibility.Collapsed; // Thay đổi thuộc tính Visibility
            gridbtn_add.Visibility = Visibility.Visible;
            gridbtn_close.Visibility = Visibility.Collapsed;
            performerchoose.Visibility = Visibility.Visible;

        }
    }
}
