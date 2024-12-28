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
            // Tạo thông tin tài khoản từ UserAccount
            string fullName = UserAccount.FullName;
            string contact = UserAccount.Contact;
            int? employeeId = UserAccount.EmployeeId;
            string roleName = UserAccount.RoleName;
            string email = UserAccount.Email;
            string password = UserAccount.Password;
            string permission = UserAccount.Permission1;
            string engagedEvents = UserAccount.EngagedEvent;

            // Tạo instance của Account Window
            Account accountWindow = new Account();

            // Truyền thông tin tài khoản vào Account Window
            accountWindow.SetAccountInfo(
                fullName,
                contact,
                employeeId,
                roleName,
                email,
                password,
                permission
            );

            // Hiển thị cửa sổ Account
            accountWindow.Show();
        }

        private void EditAccount_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý logic khi người dùng chọn "Chỉnh sửa tài khoản"
            MessageBox.Show("Chỉnh sửa tài khoản");
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            MessageBoxResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất không?",
                "Xác nhận đăng xuất",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            // Nếu người dùng chọn Yes, thực hiện đăng xuất
            if (result == MessageBoxResult.Yes)
            {
                // Đóng tất cả các cửa sổ hiện tại đang mở
                foreach (Window window in Application.Current.Windows)
                {
                    // Kiểm tra để không đóng cửa sổ đăng xuất hiện tại
                    if (window != Window.GetWindow(this))
                    {
                        window.Close();
                    }
                }

                // Sau khi đóng các cửa sổ, mở cửa sổ đăng ký (SignUp)
                SignUp signupWindow = new SignUp(); // Đảm bảo rằng SignUp là lớp của cửa sổ Signup.xaml
                signupWindow.Show(); // Hiển thị cửa sổ SignUp

                // Đóng cửa sổ hiện tại (cửa sổ đang thực thi đăng xuất)
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
                clickedButton.Background = Brushes.Red;
            }
        }
    }

    // Trong code-behind của cả Account.xaml.cs và NavigateBar.xaml.cs

    public static class User
    {
        public static string AvatarPath { get; set; } = string.Empty; // Đường dẫn tạm thời cho avatar
    }
}
