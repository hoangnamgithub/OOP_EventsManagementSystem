﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.ViewModel;

namespace OOP_EventsManagementSystem.View
{
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : Window
    {
        private EventManagementDbContext _context;
        public ObservableCollection<EmployeeRole> EmployeeRoles { get; set; }
        public ObservableCollection<Permission> Permissions { get; set; }
        private List<AccountViewModel> allAccounts;
        public bool IsVisible { get; set; }
        private bool isEditing = false;

        public Account()
        {
            _context = new EventManagementDbContext();
            InitializeComponent();
            LoadAccountData();
            LoadEmployeeRoles();

            // Kiểm tra PermissionId và thiết lập IsVisible
            if (UserAccount.PermissionId == 1)
            {
                dtgr.Visibility = Visibility.Visible; // Nếu PermissionId = 1, hiển thị phần tử
                border.Height = 550;
            }
            else
            {
                dtgr.Visibility = Visibility.Collapsed;
                border.Height = 410; // Nếu PermissionId khác 1, ẩn phần tử
            }
        }

        // Sự kiện TextChanged của TextBox tìm kiếm
        private void SearchBox_show_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchTerm = SearchBox_show.Text.ToLower(); // Lấy từ khóa tìm kiếm và chuyển thành chữ thường

            var filteredAccounts = allAccounts
                .Where(account =>
                    account.FullName.ToLower().Contains(searchTerm)
                    || account.Contact.ToLower().Contains(searchTerm)
                    || account.Email.ToLower().Contains(searchTerm)
                    || account.Password.ToLower().Contains(searchTerm)
                    || account.Permission.ToLower().Contains(searchTerm)
                    || account.RoleName.ToLower().Contains(searchTerm)
                )
                .ToList();

            dataGrid.ItemsSource = filteredAccounts; // Cập nhật lại DataGrid với dữ liệu đã lọc
        }

        private void LoadAccountData()
        {
            // Lấy danh sách Account và hiển thị
            var accounts = _context
                .Accounts.Include(a => a.Employee)
                .Include(a => a.Permission)
                .Include(a => a.Employee.Role) // Bao gồm Role để lấy RoleName
                .Where(a => a.PermissionId != 1)
                .Select(a => new AccountViewModel
                {
                    FullName = a.Employee.FullName,
                    Email = a.Email,
                    Password = a.Password,
                    RoleName = a.Employee.Role.RoleName, // Gán RoleName từ Employee.Role
                    Contact = a.Employee.Contact, // Lấy Contact từ bảng Employee
                    Permission = a.Permission.Permission1, // Thêm Permission1 từ bảng Permission
                    PermissionID = a.PermissionId,
                })
                .ToList();

            allAccounts = accounts; // Lưu tất cả các tài khoản
            dataGrid.ItemsSource = allAccounts; // Hiển thị lên DataGrid
        }

        private void LoadEmployeeRoles()
        {
            // Khởi tạo ObservableCollection với dữ liệu từ bảng EmployeeRole
            EmployeeRoles = new ObservableCollection<EmployeeRole>(_context.EmployeeRoles.ToList());
            Permissions = new ObservableCollection<Permission>(_context.Permissions.ToList());
            DataContext = this; // Đặt DataContext để sử dụng trong XAML
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png", // Chỉ chấp nhận file .jpg và .png
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                try
                {
                    imgEvent.Source = new BitmapImage(new Uri(selectedFilePath));
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btn_Edit.Visibility = Visibility.Collapsed;
            btnConfirm.Visibility = Visibility.Visible;

            // Cho phép chỉnh sửa các trường cần thiết
            txtfull_name.IsEnabled = true;
            txtEmail.IsEnabled = true;
            txtContact.IsEnabled = true;
            txtPassword.IsEnabled = true;

            // Không cho phép chỉnh sửa các trường khác
            txtPermission.IsEnabled = false;
            txtRole.IsEnabled = false;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            // Lấy dữ liệu từ các TextBox
            string fullName = txtfull_name.Text.Trim();
            string email = txtEmail.Text.Trim();
            string contact = txtContact.Text.Trim();
            string password = txtPassword.Text.Trim();
            int? employeeId = UserAccount.EmployeeId;

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

            // Kiểm tra các trường không được để trống
            if (
                string.IsNullOrEmpty(fullName)
                || string.IsNullOrEmpty(email)
                || string.IsNullOrEmpty(contact)
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

            try
            {
                // Tìm kiếm tài khoản dựa trên EmployeeId
                var account = _context
                    .Accounts.Include(a => a.Employee) // Bao gồm thông tin Employee
                    .Include(a => a.Employee.Role) // Bao gồm Role
                    .FirstOrDefault(a => a.Employee.EmployeeId == employeeId); // Tìm kiếm theo EmployeeId

                if (account != null)
                {
                    // Cập nhật thông tin tài khoản
                    account.Email = email;
                    account.Password = password;

                    if (account.Employee != null)
                    {
                        // Cập nhật thông tin Employee
                        account.Employee.FullName = fullName;
                        account.Employee.Contact = contact;

                        // Cập nhật Role nếu cần thiết
                        var selectedRole = _context.EmployeeRoles.FirstOrDefault(r =>
                            r.RoleName == txtRole.Text.Trim()
                        );

                        if (selectedRole != null)
                        {
                            account.Employee.RoleId = selectedRole.RoleId;
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            "Employee record not found.",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                        return;
                    }

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _context.SaveChanges();

                    // Hiển thị thông báo thành công
                    MessageBox.Show(
                        "Your account information has been updated successfully.",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                    // Cập nhật lại giao diện
                    txtfull_name.Text = account.Employee.FullName;
                    txtEmail.Text = account.Email;
                    txtContact.Text = account.Employee.Contact;
                    txtPassword.Text = account.Password;

                    // Cập nhật UserAccount
                    UserAccount.Email = account.Email;
                    UserAccount.Password = account.Password;
                    UserAccount.FullName = account.Employee.FullName;
                    UserAccount.Contact = account.Employee.Contact;
                }
                else
                {
                    MessageBox.Show(
                        "Account not found.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi
                MessageBox.Show(
                    $"An error occurred: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }

            // Vô hiệu hóa các TextBox và hiển thị lại nút "Edit"
            txtfull_name.IsEnabled = false;
            txtEmail.IsEnabled = false;
            txtContact.IsEnabled = false;
            txtPassword.IsEnabled = false;
            btnConfirm.Visibility = Visibility.Collapsed;
            btn_Edit.Visibility = Visibility.Visible;
        }

        // Kiểm tra định dạng email có đuôi ".easys.com"
        private bool IsValidEmail(string email)
        {
            return email.EndsWith("@easys.com", StringComparison.OrdinalIgnoreCase);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedAccounts = dataGrid.SelectedItems.Cast<AccountViewModel>().ToList();

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
                        foreach (var selectedAccount in selectedAccounts)
                        {
                            // Tìm tài khoản trong cơ sở dữ liệu bằng Email hoặc FullName
                            var accountToDelete = _context
                                .Accounts.Include(a => a.Employee)
                                .FirstOrDefault(a => a.Email == selectedAccount.Email);

                            if (accountToDelete != null)
                            {
                                // Xóa tài khoản
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

        public void SetAccountInfo(
            string fullName,
            string contact,
            int? employeeId,
            string roleName,
            string email,
            string password,
            string permission
        )
        {
            txtfull_name.Text = fullName;
            txtContact.Text = contact;
            txtRole.Text = roleName;
            txtEmail.Text = email;
            txtPassword.Text = password;
            txtPermission.Text = permission;
        }

        private void edit_btn_Click(object sender, RoutedEventArgs e)
        {
            // Enable DataGrid editing and disable Edit button
            dataGrid.IsReadOnly = false;
            btn_editShow.IsEnabled = false; // Disable Edit button
            btn_comfirmShow.IsEnabled = true; // Enable Confirm button
            isEditing = true;
        }

        // Confirm Button Click - Save Changes
        private void comfirm_btn_Click(object sender, RoutedEventArgs e)
        {
            // Save changes to the database
            SaveChangesToDatabase();

            // Disable DataGrid editing and disable Confirm button
            dataGrid.IsReadOnly = true;
            btn_editShow.IsEnabled = true; // Re-enable Edit button
            btn_comfirmShow.IsEnabled = false; // Disable Confirm button
            isEditing = false;
        }

        // Save Changes to Database (Example method)
        private void SaveChangesToDatabase()
        {
            try
            {
                // First, ensure that the currently selected account in the DataGrid has been modified
                // Iterate through the rows of the DataGrid to check for any changes made
                foreach (var item in dataGrid.ItemsSource.Cast<AccountViewModel>())
                {
                    var account = _context
                        .Accounts.Include(a => a.Employee) // Include Employee for full access
                        .Include(a => a.Employee.Role) // Include Role for full access
                        .FirstOrDefault(a => a.Employee.FullName == item.FullName);

                    if (account != null)
                    {
                        // Update the account and employee properties based on the values from the DataGrid
                        account.Email = item.Email;
                        account.Password = item.Password;
                        account.Employee.FullName = item.FullName;
                        account.Employee.Contact = item.Contact;

                        // If you are editing the role, update it as well
                        var selectedRole = _context.EmployeeRoles.FirstOrDefault(r =>
                            r.RoleName == item.RoleName
                        );
                        if (selectedRole != null)
                        {
                            account.Employee.RoleId = selectedRole.RoleId; // Update RoleId
                        }

                        // Update Permission if necessary (e.g., Permission ID change)
                        var selectedPermission = _context.Permissions.FirstOrDefault(p =>
                            p.Permission1 == item.Permission
                        );
                        if (selectedPermission != null)
                        {
                            account.PermissionId = selectedPermission.PermissionId;
                        }
                    }
                }

                // Save all changes to the database
                _context.SaveChanges();

                // Notify user that the changes have been saved
                MessageBox.Show(
                    "Account information updated successfully.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // Reload account data to reflect changes in the DataGrid
                LoadAccountData();
            }
            catch (Exception ex)
            {
                // Handle any errors during the save process
                MessageBox.Show(
                    $"An error occurred while saving changes: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void Border_MouseLeftButtonDown(
            object sender,
            System.Windows.Input.MouseButtonEventArgs e
        )
        {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public class AccountViewModel
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string RoleName { get; set; }
            public string Contact { get; set; }
            public string Permission { get; set; } // Thêm thuộc tính Permission
            public int PermissionID { get; set; }
        }
    }
}
