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

namespace OOP_EventsManagementSystem.Styles
{
    /// <summary>
    /// Interaction logic for AddEmployeeForMana.xaml
    /// </summary>
    public partial class AddEmployeeForMana : Window
    {
        public string RoleName { get; set; }
        private employeeFormanager _employeeFormanager;

        public AddEmployeeForMana(string roleName, employeeFormanager employeeFormanager)
        {
            InitializeComponent();
            RoleName = roleName;
            _employeeFormanager = employeeFormanager;
            // Tải dữ liệu nhân viên dựa trên RoleName
            LoadEmployeesByRole();
        }

        private void LoadEmployeesByRole()
        {
            using (var context = new EventManagementDbContext())
            {
                FullNameComboBox.ItemsSource = context.Employees
    .Select(emp => new Employeevm
    {
        EmployeeId = emp.EmployeeId,
        FullName = emp.FullName,
        Contact = emp.Contact
    })
    .ToList();
            }
        }

        private void FullNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Kiểm tra nếu có mục nào được chọn
            if (FullNameComboBox.SelectedItem is Employeevm selectedEmployee)
            {
                // Hiển thị thông tin EmployeeId và Contact
                EmployeeIDTextBlock.Text = selectedEmployee.EmployeeId.ToString();
                ContactTextBlock.Text = selectedEmployee.Contact ?? "N/A";
            }
            else
            {
                // Reset giá trị nếu không có mục nào được chọn
                EmployeeIDTextBlock.Text = string.Empty;
                ContactTextBlock.Text = string.Empty;
            }
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy thông tin EmployeeId từ ComboBox
            if (FullNameComboBox.SelectedItem is null)
            {
                MessageBox.Show("Please select an employee.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Lấy thông tin EventId được truyền từ employeeFormanager
            if (DataContext is not int eventId)
            {
                MessageBox.Show("No event selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Lấy EmployeeId từ ComboBox
            dynamic selectedEmployee = FullNameComboBox.SelectedItem;
            int employeeId = selectedEmployee.EmployeeId;

            using (var context = new EventManagementDbContext())
            {
                // Kiểm tra xem nhân viên đã có Account chưa
                var account = context.Accounts.FirstOrDefault(a => a.EmployeeId == employeeId);
                if (account is null)
                {
                    MessageBox.Show("This employee does not have an account.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Kiểm tra xem nhân viên đã tham gia sự kiện chưa
                bool alreadyEngaged = context.Engageds.Any(e => e.AccountId == account.AccountId && e.EventId == eventId);
                if (alreadyEngaged)
                {
                    MessageBox.Show("This employee is already engaged in the selected event.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Thêm bản ghi mới vào bảng Engaged
                var newEngaged = new Engaged
                {
                    AccountId = account.AccountId,
                    EventId = eventId
                };
                context.Engageds.Add(newEngaged);

                // Lưu thay đổi vào cơ sở dữ liệu
                context.SaveChanges();

                MessageBox.Show("Employee successfully added to the event.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Gọi lại phương thức LoadEmployees trong employeeFormanager để cập nhật lại DataGrid
                _employeeFormanager.LoadEmployees(eventId, RoleName); // Sử dụng RoleName từ đối tượng hiện tại

                // Đóng cửa sổ
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

    }
    public class Employeevm
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Contact { get; set; }
    }

}
