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
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using OOP_EventsManagementSystem.Model;
using static MaterialDesignThemes.Wpf.Theme;

namespace OOP_EventsManagementSystem.View
{
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : Window
    {
        private EventManagementDbContext _context;

        public Account()
        {
            InitializeComponent();
            _context = new EventManagementDbContext();
            LoadAccountData();
        }

        private void LoadAccountData()
        {
            // Check if the PermissionId of the logged-in user is 1
            if (UserAccount.PermissionId == 1)
            {
                var accounts = _context
                    .Accounts.Include(a => a.Employee) // Include Employee to get FullName and Role
                    .Include(a => a.Permission) // Include Permission to get Permission Name
                    .Where(a => a.PermissionId == 3) // Filter by PermissionId = 1
                    .Select(a => new
                    {
                        a.Employee.FullName,
                        a.Permission.Permission1, // Assuming 'Permission1' is the field for the Permission name
                        a.Email,
                        a.Password,
                        a.Employee.EmployeeId,
                        a.Employee.Role.RoleName,
                    })
                    .ToList();

                // Set the DataGrid's items source to the query result
                dataGrid.ItemsSource = accounts;
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Mở hộp thoại OpenFileDialog để chọn ảnh
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png"; // Chỉ chấp nhận file .jpg và .png

            if (openFileDialog.ShowDialog() == true)
            {
                // Lấy đường dẫn file ảnh đã chọn
                string selectedFilePath = openFileDialog.FileName;

                try
                {
                    // Cập nhật nguồn hình ảnh cho imgEvent
                    imgEvent.Source = new BitmapImage(new Uri(selectedFilePath));

                    // Lưu đường dẫn ảnh vào UserAccount (chỉ cần lưu đường dẫn tạm thời trong bộ nhớ)
                    User.AvatarPath = selectedFilePath;

                    // Cập nhật hình ảnh ở NavigateBar.xaml
                    UpdateNavigateBarAvatar(selectedFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Không thể tải ảnh: {ex.Message}",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private void UpdateNavigateBarAvatar(string avatarPath)
        {
            // Cập nhật hình ảnh ở NavigateBar.xaml
            // Giả sử bạn đã có đối tượng Image tên là imgNavigateBar trong NavigateBar.xaml
            var navigateBar = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (navigateBar != null)
            {
                var imgNavigateBar = navigateBar.FindName("imgNavigateBar") as Image;
                if (imgNavigateBar != null)
                {
                    imgNavigateBar.Source = new BitmapImage(new Uri(avatarPath));
                }
            }
        }

        public void SetAccountInfo(
            string fullName,
            int? employeeId,
            string roleName,
            string email,
            string password,
            string permission
        )
        {
            txtfull_name.Text = fullName;
            txtemployee_id.Text = employeeId.ToString();
            txtRole.Text = roleName;
            txtEmail.Text = email;
            txtPassword.Text = password;
            txtPermission.Text = permission;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Ẩn nút "Edit", hiển thị nút "Confirm"
            btn_Edit.Visibility = Visibility.Collapsed;
            btnConfirm.Visibility = Visibility.Visible;

            // Kích hoạt các TextBox để người dùng có thể chỉnh sửa
            txtfull_name.IsEnabled = true;
            txtEmail.IsEnabled = true;
            txtPassword.IsEnabled = true;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            // Lấy dữ liệu từ các TextBox
            string fullName = txtfull_name.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            // Kiểm tra định dạng email
            if (!IsValidEmail(email))
            {
                MessageBox.Show(
                    "Email must have the domain .easys.com",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            // Kiểm tra xem các trường có hợp lệ không
            if (
                string.IsNullOrEmpty(fullName)
                || string.IsNullOrEmpty(email)
                || string.IsNullOrEmpty(password)
            )
            {
                MessageBox.Show(
                    "Please fill in all fields before confirming.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            // Lấy EmployeeId từ TextBox
            if (!int.TryParse(txtemployee_id.Text, out int employeeId))
            {
                MessageBox.Show(
                    "Invalid Employee ID.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            try
            {
                // Tìm kiếm Account dựa trên EmployeeId
                var account = _context
                    .Accounts.Include(a => a.Employee) // Bao gồm thông tin Employee để cập nhật FullName
                    .FirstOrDefault(a => a.EmployeeId == employeeId); // Tìm kiếm bằng EmployeeId

                if (account != null)
                {
                    // Cập nhật thông tin vào Account
                    account.Password = password; // Lưu mật khẩu đã mã hóa vào
                    account.Email = email;
                    account.Employee.FullName = fullName;
                    // Cập nhật thông tin vào Employee nếu có
                    if (account.Employee != null)
                    {
                        account.Employee.FullName = fullName; // Cập nhật FullName của Employee
                    }
                    else
                    {
                        // Nếu không có Employee, có thể tạo một Employee mới hoặc thông báo lỗi
                        MessageBox.Show(
                            "Employee not found.",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                        return;
                    }

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _context.SaveChanges();

                    // Cập nhật lại các TextBox trên giao diện
                    txtfull_name.Text = account.Employee.FullName; // Hiển thị lại FullName
                    txtEmail.Text = account.Email; // Hiển thị lại Email
                    txtPassword.Text = account.Password; // Hiển thị lại Password (có thể mã hóa lại nếu cần)

                    // Cập nhật lại UserAccount với các giá trị mới
                    UserAccount.Email = account.Email;
                    UserAccount.Password = account.Password;
                    UserAccount.FullName = account.Employee.FullName;

                    // Hiển thị thông báo thành công
                    MessageBox.Show(
                        "Your account information has been updated successfully.",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        "User not found.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
            catch (Exception ex)
            {
                // Hiển thị lỗi nếu có
                MessageBox.Show(
                    $"An error occurred: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }

            // Ẩn nút "Confirm", hiển thị lại nút "Edit"
            btnConfirm.Visibility = Visibility.Collapsed;
            btn_Edit.Visibility = Visibility.Visible;

            // Vô hiệu hóa các TextBox khi người dùng đã chỉnh sửa xong
            txtfull_name.IsEnabled = false;
            txtEmail.IsEnabled = false;
            txtPassword.IsEnabled = false;
        }

        // Kiểm tra định dạng email có đuôi ".easys.com"
        private bool IsValidEmail(string email)
        {
            return email.EndsWith("@easys.com", StringComparison.OrdinalIgnoreCase);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Lấy danh sách các tài khoản đã chọn từ DataGrid
            var selectedAccounts = dataGrid.SelectedItems.Cast<dynamic>().ToList();

            if (selectedAccounts.Any())
            {
                // Hiển thị hộp thoại xác nhận
                var result = MessageBox.Show(
                    "Are you sure you want to delete the selected accounts and their related data?",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        foreach (var account in selectedAccounts)
                        {
                            // Lấy EmployeeId từ tài khoản
                            int employeeId = account.EmployeeId;

                            // Tìm kiếm tài khoản trong cơ sở dữ liệu
                            var accountToDelete = _context.Accounts.FirstOrDefault(a =>
                                a.EmployeeId == employeeId
                            );

                            if (accountToDelete != null)
                            {
                                // Xóa Account (cascade delete sẽ xóa cả Employee liên quan)
                                _context.Accounts.Remove(accountToDelete);
                            }
                        }

                        // Lưu thay đổi vào cơ sở dữ liệu
                        _context.SaveChanges();

                        // Tải lại dữ liệu để cập nhật DataGrid
                        LoadAccountData();

                        // Thông báo thành công
                        MessageBox.Show(
                            "Selected accounts and related data have been deleted successfully.",
                            "Success",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );
                    }
                    catch (Exception ex)
                    {
                        // Hiển thị lỗi nếu có
                        MessageBox.Show(
                            $"An error occurred: {ex.Message}",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    }
                }
            }
            else
            {
                MessageBox.Show(
                    "No accounts selected to delete.",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    // Trong code-behind của cả Account.xaml.cs và NavigateBar.xaml.cs
    public static class User
    {
        public static string AvatarPath { get; set; } = string.Empty; // Đường dẫn tạm thời cho avatar
    }
}
