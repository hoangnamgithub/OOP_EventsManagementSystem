using Microsoft.EntityFrameworkCore;
using OOP_EventsManagementSystem.Model;
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

namespace OOP_EventsManagementSystem
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {              
            private readonly EventManagementDbContext _context;

            public SignUp()
            {
                InitializeComponent();
                _context = new EventManagementDbContext(); // Kết nối tới DbContext
            }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            string email = txt_box_email.Text;
            string password = txt_box_password.Password;

            // Kiểm tra nếu email và password không rỗng hoặc null
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Email và mật khẩu không thể để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Kiểm tra thông tin trong cơ sở dữ liệu và lấy thông tin Permission từ bảng Permission
                var account = _context.Accounts
                    .Include(a => a.Permission) // Bao gồm bảng Permission để lấy Permission1
                    .FirstOrDefault(a => a.Email == email && a.Password == password);

                // Kiểm tra nếu không tìm thấy tài khoản
                if (account == null)
                {
                    MessageBox.Show("Email hoặc mật khẩu không đúng.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Lưu thông tin tài khoản và Permission vào lớp UserAccount
                UserAccount.Email = account.Email;
                UserAccount.Password = account.Password;
                UserAccount.PermissionId = account.PermissionId;
                UserAccount.Permission1 = account.Permission.Permission1; // Lưu Permission1

                // Kiểm tra PermissionId
                if (account.PermissionId == 1)
                {
                    // Nếu quyền là 1, mở MainWindow
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close(); // Đóng cửa sổ đăng nhập
                }
                else
                {
                    // Nếu không có quyền, hiển thị thông báo
                    MessageBox.Show("Bạn không có quyền truy cập.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PowerOff_Click(object sender, RoutedEventArgs e)
        {
            // Tắt cửa sổ hiện tại
            this.Close();
        }

        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (txt_box_password.Visibility == Visibility.Visible)
            {
                // Hiển thị mật khẩu dạng văn bản
                txt_box_password_plain.Text = txt_box_password.Password;
                txt_box_password.Visibility = Visibility.Collapsed;
                txt_box_password_plain.Visibility = Visibility.Visible;

                // Đổi hình ảnh của nút (nếu cần)
                btn_toggle_password.Content = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/eye_on.png"))
                };
            }
            else
            {
                // Ẩn mật khẩu dạng văn bản
                txt_box_password.Password = txt_box_password_plain.Text;
                txt_box_password.Visibility = Visibility.Visible;
                txt_box_password_plain.Visibility = Visibility.Collapsed;

                // Đổi hình ảnh của nút (nếu cần)
                btn_toggle_password.Content = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/eye_off.png"))
                };
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Đồng bộ hóa nội dung từ PasswordBox đến TextBox
            if (txt_box_password.Visibility == Visibility.Visible)
            {
                txt_box_password_plain.Text = txt_box_password.Password;
            }
        }

    }
    public static class UserAccount
    {
        public static string Email { get; set; }
        public static string Password { get; set; }
        public static int PermissionId { get; set; }
        public static string Permission1 { get; set; } // Thêm Permission1 vào UserAccount
    }


}

