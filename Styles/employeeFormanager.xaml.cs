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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OOP_EventsManagementSystem.Styles
{
    /// <summary>
    /// Interaction logic for employeeFormanager.xaml
    /// </summary>
    public partial class employeeFormanager : UserControl
    {
        private dynamic? SelectedEmployee { get; set; }

        public employeeFormanager()
        {
            InitializeComponent();
            LoadEvents();
        }

        private void LoadEvents()
        {
            try
            {
                using (var context = new EventManagementDbContext())
                {
                    // Lấy dữ liệu từ database
                    var events = context.Events
    .Select(e => new
    {
        e.EventId, // Đảm bảo EventId có trong dữ liệu
        e.EventName,
        StartDate = e.StartDate.ToDateTime(TimeOnly.MinValue).ToString("yyyy-MM-dd"),
        EndDate = e.EndDate.ToDateTime(TimeOnly.MinValue).ToString("yyyy-MM-dd")
    })
    .ToList();


                    // Gán dữ liệu cho DataGrid
                    EventDataGrid.ItemsSource = events;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading events: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void EventDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (EventDataGrid.SelectedItem is null) return;

            // Lấy EventId từ sự kiện được chọn
            var selectedEvent = EventDataGrid.SelectedItem;
            var eventId = (int)selectedEvent.GetType().GetProperty("EventId")?.GetValue(selectedEvent, null);

            LoadNeeds(eventId);
        }

        private void LoadNeeds(int eventId)
        {
            try
            {
                using (var context = new EventManagementDbContext())
                {
                    // Lấy danh sách Needs liên quan đến EventId
                    var needs = context.Needs
                        .Where(n => n.EventId == eventId)
                        .Select(n => new
                        {
                            n.Role.RoleName,
                            n.Quantity
                        })
                        .ToList();

                    NeedDataGrid.ItemsSource = needs;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading needs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NeedDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (NeedDataGrid.SelectedItem == null) return;

            // Lấy RoleName từ hàng được chọn
            dynamic selectedNeed = NeedDataGrid.SelectedItem;
            string selectedRoleName = selectedNeed.RoleName;

            // Lấy RoleName của UserAccount
            string userRoleName = UserAccount.RoleName; // Phương thức này trả về RoleName của UserAccount

            // Kiểm tra RoleName có trùng với RoleName của UserAccount không
            if (!string.Equals(selectedRoleName, userRoleName, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("You can only select roles that match your account's role.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Lấy EventId của sự kiện hiện tại
            if (EventDataGrid.SelectedItem == null) return;
            dynamic selectedEvent = EventDataGrid.SelectedItem;
            int eventId = selectedEvent.EventId;

            // Tải danh sách nhân viên
            LoadEmployees(eventId, selectedRoleName);
        }

        private void LoadEmployees(int eventId, string roleName)
        {
            try
            {
                using (var context = new EventManagementDbContext())
                {
                    var employees = context.Engageds
                        .Where(e => e.EventId == eventId && e.Account.Employee.Role.RoleName == roleName)
                        .Select(e => new EmployeeViewModel
                        {
                            EmployeeId = e.Account.Employee.EmployeeId,
                            FullName = e.Account.Employee.FullName,
                            Contact = e.Account.Employee.Contact,
                            Salary = e.Account.Employee.Role.Salary
                        })
                        .ToList();

                    EmployeeDataGrid.ItemsSource = employees;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EmployeeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeeDataGrid.SelectedItem is not null)
            {
                SelectedEmployee = EmployeeDataGrid.SelectedItem;
            }
            else
            {
                SelectedEmployee = null;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select an employee to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var context = new EventManagementDbContext())
                {
                    // Lấy EmployeeId từ nhân viên được chọn
                    dynamic selectedEmployee = EmployeeDataGrid.SelectedItem;
                    int employeeId = selectedEmployee.EmployeeId;

                    // Xác định EventId từ sự kiện hiện tại
                    if (EventDataGrid.SelectedItem == null)
                    {
                        MessageBox.Show("Please select an event first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    dynamic selectedEvent = EventDataGrid.SelectedItem;
                    int eventId = selectedEvent.EventId;

                    // Lấy RoleName từ NeedDataGrid
                    if (NeedDataGrid.SelectedItem == null)
                    {
                        MessageBox.Show("Please select a role first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    dynamic selectedRole = NeedDataGrid.SelectedItem;
                    string roleName = selectedRole.RoleName;

                    // Tìm và xóa nhân viên trong cơ sở dữ liệu
                    var employeeToDelete = context.Employees.FirstOrDefault(emp => emp.EmployeeId == employeeId);
                    if (employeeToDelete != null)
                    {
                        context.Employees.Remove(employeeToDelete);
                        context.SaveChanges();
                        MessageBox.Show("Employee deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    // Cập nhật lại bảng nhân viên
                    LoadEmployees(eventId, roleName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting employee: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Bật chế độ chỉnh sửa cho DataGrid
            EmployeeDataGrid.IsReadOnly = false;

            // Hiển thị nút Confirm, ẩn nút Edit
            EditButton.Visibility = Visibility.Collapsed;
            ConfirmButton.Visibility = Visibility.Visible;
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new EventManagementDbContext())
                {
                    foreach (var item in EmployeeDataGrid.Items)
                    {
                        if (item is EmployeeViewModel employeeData)
                        {
                            // Tìm nhân viên trong cơ sở dữ liệu và tải trước Role
                            var employee = context.Employees
                                .Include(emp => emp.Role)
                                .FirstOrDefault(emp => emp.EmployeeId == employeeData.EmployeeId);

                            if (employee != null)
                            {
                                // Cập nhật thông tin nhân viên
                                employee.FullName = employeeData.FullName;
                                employee.Contact = employeeData.Contact;

                                if (employee.Role != null)
                                {
                                    employee.Role.Salary = employeeData.Salary;
                                }
                                else
                                {
                                    MessageBox.Show($"Role for employee ID {employee.EmployeeId} not found.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            }
                        }
                    }

                    // Lưu thay đổi
                    context.SaveChanges();
                    MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Tắt chế độ chỉnh sửa
                EmployeeDataGrid.IsReadOnly = true;

                // Hiển thị lại nút Edit, ẩn nút Confirm
                ConfirmButton.Visibility = Visibility.Collapsed;
                EditButton.Visibility = Visibility.Visible;

                // Cập nhật lại bảng dữ liệu
                if (NeedDataGrid.SelectedItem is Need selectedNeed && EventDataGrid.SelectedItem is Event selectedEvent)
                {
                    LoadEmployees(selectedEvent.EventId, selectedNeed.Role.RoleName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Contact { get; set; }
        public decimal Salary { get; set; }
    }

}
