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
using Microsoft.EntityFrameworkCore;
using OOP_EventsManagementSystem.Model;

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

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(
                    "Email và mật khẩu không thể để trống.",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            try
            {
                var account = _context
                    .Accounts.Include(a => a.Permission)
                    .Include(a => a.Employee)
                    .ThenInclude(e => e.Role)
                    .Include(a => a.Engageds)
                    .ThenInclude(e => e.Event)
                    .FirstOrDefault(a => a.Email == email && a.Password == password);

                if (account == null)
                {
                    MessageBox.Show(
                        "Email hoặc mật khẩu không đúng.",
                        "Thông báo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    return;
                }

                // Lưu thông tin tài khoản vào lớp UserAccount
                UserAccount.Email = account.Email;
                UserAccount.Password = account.Password;
                UserAccount.PermissionId = account.PermissionId;
                UserAccount.Permission1 = account.Permission.Permission1;

                if (account.Employee != null)
                {
                    UserAccount.EmployeeId = account.Employee.EmployeeId;
                    UserAccount.FullName = account.Employee.FullName;
                    UserAccount.RoleName = account.Employee.Role?.RoleName;
                }
                else
                {
                    UserAccount.EmployeeId = null;
                    UserAccount.FullName = "Admin";
                    UserAccount.RoleName = "Admin";
                }

                var engagedEventNames = account.Engageds.Select(e => e.Event.EventName).ToList();
                UserAccount.EngagedEvent = string.Join(", ", engagedEventNames);

                // Truyền PermissionId vào MainWindowVM khi mở cửa sổ MainWindow
                MainWindow mainWindow = new MainWindow(new MainWindowVM(account.PermissionId)); // Truyền PermissionId vào constructor của MainWindowVM
                mainWindow.Show();

                // Đóng cửa sổ SignUp sau khi đăng nhập
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Đã xảy ra lỗi: {ex.Message}",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
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
                    Source = new BitmapImage(
                        new Uri("pack://application:,,,/Resources/Images/eye_on.png")
                    ),
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
                    Source = new BitmapImage(
                        new Uri("pack://application:,,,/Resources/Images/eye_off.png")
                    ),
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

        private void EventDescription_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }

    public static class UserAccount
    {
        public static string Email { get; set; }
        public static string Password { get; set; }
        public static int PermissionId { get; set; }
        public static string Permission1 { get; set; } // Quyền
        public static int? EmployeeId { get; set; } // Mã nhân viên
        public static string FullName { get; set; } // Tên đầy đủ
        public static string RoleName { get; set; } // Vai trò
        public static string EngagedEvent { get; set; } // Sự kiện tham gia
    }
}
