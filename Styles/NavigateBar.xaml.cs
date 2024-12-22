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
using OOP_EventsManagementSystem.Utilities;
using OOP_EventsManagementSystem.View;
using OOP_EventsManagementSystem.ViewModel;

namespace OOP_EventsManagementSystem.Styles
{
    /// <summary>
    /// Interaction logic for NavigateBar.xaml
    /// </summary>
    public partial class NavigateBar : UserControl
    {
        public NavigateBar()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            AccountContextMenu.IsOpen = true;
        }

        private void ViewAccount_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý logic khi người dùng chọn "Xem thông tin tài khoản"
            MessageBox.Show("Xem thông tin tài khoản");
        }

        private void EditAccount_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý logic khi người dùng chọn "Chỉnh sửa tài khoản"
            MessageBox.Show("Chỉnh sửa tài khoản");
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Xác nhận đăng xuất", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Nếu người dùng chọn Yes, thực hiện đăng xuất
            if (result == MessageBoxResult.Yes)
            {
                // Đóng toàn bộ cửa sổ đang mở
                foreach (Window window in Application.Current.Windows)
                {
                    if (window != Window.GetWindow(this))
                    {
                        window.Close();
                    }
                }

                // Đóng cửa sổ chính sau cùng
                Window.GetWindow(this).Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Reset the background color of all buttons
            HomeButton.Background = Brushes.Transparent;
            EventButton.Background = Brushes.Transparent;
            ShowButton.Background = Brushes.Transparent;
            PartnerButton.Background = Brushes.Transparent;
            EmployeeButton.Background = Brushes.Transparent;
            EquipmentButton.Background = Brushes.Transparent;
            LocationButton.Background = Brushes.Transparent;

            // Set the background color of the clicked button
            if (sender is Button clickedButton)
            {
                clickedButton.Foreground = Brushes.Red;
            }
        }
    }
}

