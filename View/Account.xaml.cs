using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OOP_EventsManagementSystem.View
{
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : Window
    {
        private EventManagementDbContext _context;
        public ObservableCollection<EmployeeRole> EmployeeRoles { get; set; }

        public Account()
        {
            InitializeComponent();
            _context = new EventManagementDbContext();
            LoadAccountData();
            LoadEmployeeRoles();
        }

        private void LoadAccountData()
        {
            // Lấy danh sách Account và hiển thị
            var accounts = _context.Accounts
                .Include(a => a.Employee)
                .Include(a => a.Permission)
                .Include(a => a.Employee.Role) // Bao gồm Role để lấy RoleName
                .Where(a => a.PermissionId == 3)
                .Select(a => new AccountViewModel
                {
                    FullName = a.Employee.FullName,
                    Email = a.Email,
                    Password = a.Password,
                    RoleName = a.Employee.Role.RoleName, // Gán RoleName từ Employee.Role
                    Contact = a.Employee.Contact // Lấy Contact từ bảng Employee
                })
                .ToList();

            dataGrid.ItemsSource = accounts;
        }

        private void LoadEmployeeRoles()
        {
            // Khởi tạo ObservableCollection với dữ liệu từ bảng EmployeeRole
            EmployeeRoles = new ObservableCollection<EmployeeRole>(_context.EmployeeRoles.ToList());
            DataContext = this; // Đặt DataContext để sử dụng trong XAML
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png" // Chỉ chấp nhận file .jpg và .png
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
                    MessageBox.Show($"Không thể tải ảnh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void BtnEditAccount_Click(object sender, RoutedEventArgs e)
        {
            btn_Edit.Visibility = Visibility.Collapsed;
            btnConfirm.Visibility = Visibility.Visible;
            dataGrid.IsReadOnly = false;
            dataGrid.SelectionMode = DataGridSelectionMode.Single;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var updatedAccounts = dataGrid.ItemsSource.Cast<AccountViewModel>().ToList();

                foreach (var updatedAccount in updatedAccounts)
                {
                    var account = _context.Accounts
                        .Include(a => a.Employee)
                        .Include(a => a.Employee.Role)
                        .FirstOrDefault(a => a.Employee.FullName == updatedAccount.FullName);

                    if (account != null)
                    {
                        account.Email = updatedAccount.Email;
                        account.Password = updatedAccount.Password;

                        if (account.Employee != null)
                        {
                            account.Employee.FullName = updatedAccount.FullName;
                            account.Employee.Contact = updatedAccount.Contact;

                            var selectedRole = _context.EmployeeRoles
                                .FirstOrDefault(r => r.RoleName == updatedAccount.RoleName);

                            if (selectedRole != null)
                            {
                                account.Employee.RoleId = selectedRole.RoleId;
                            }
                        }
                    }
                }

                _context.SaveChanges();
                MessageBox.Show("All changes have been saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            btnConfirm.Visibility = Visibility.Collapsed;
            btn_Edit.Visibility = Visibility.Visible;
            dataGrid.IsReadOnly = true;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedAccounts = dataGrid.SelectedItems.Cast<AccountViewModel>().ToList();

            if (selectedAccounts.Any())
            {
                try
                {
                    foreach (var account in selectedAccounts)
                    {
                        var accountToDelete = _context.Accounts
                            .FirstOrDefault(a => a.Employee.FullName == account.FullName);

                        if (accountToDelete != null)
                        {
                            _context.Accounts.Remove(accountToDelete);
                        }
                    }

                    _context.SaveChanges();
                    LoadAccountData();
                    MessageBox.Show("Selected accounts and related data have been deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No accounts selected to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
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
        }
    }
}
