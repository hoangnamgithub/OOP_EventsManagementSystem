using OOP_EventsManagementSystem.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OOP_EventsManagementSystem.View
{
    /// <summary>
    /// Interaction logic for EventDetails.xaml
    /// </summary>
    public partial class EventDetails : UserControl
    {
        public EventDetails()
        {
            InitializeComponent();
        }


        // Đặt phương thức PowerOff_Click bên trong lớp

        private void PowerOff_Click(object sender, RoutedEventArgs e)
        {
            // Lấy cửa sổ chứa UserControl (MainWindow)
            Window parentWindow = Window.GetWindow(this);

            // Đóng cửa sổ (nếu tồn tại)
            parentWindow?.Close();
        }
    }
}
