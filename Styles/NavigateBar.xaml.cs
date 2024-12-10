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
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem ContextMenu có xuất hiện không khi nhấn nút
            MessageBox.Show("Nút đã được nhấn");
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
            // Xử lý logic khi người dùng chọn "Log Out"
            MessageBox.Show("Đăng xuất");
        }

    }
}
