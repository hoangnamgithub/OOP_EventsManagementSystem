using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Styles;
using OOP_EventsManagementSystem.Utilities;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class EmployeeVM : INotifyPropertyChanged
    {
        private readonly EventManagementDbContext _context;
        private ObservableCollection<EmployeeEventViewModel> _allEmployees;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public ObservableCollection<EmployeeEventViewModel> Employees { get; set; }

        public EmployeeVM()
        {
            _context = new EventManagementDbContext();
            LoadData();

            AddCommand = new RelayCommand(AddNewEmployee);
            EditCommand = new RelayCommand(EditEmployee);
            DeleteCommand = new RelayCommand(DeleteEmployee);
        }

        private void LoadData()
        {
            DateTime today = DateTime.Now.Date;

            var employeesToday = from employee in _context.Employees
                                 join account in _context.Accounts on employee.EmployeeId equals account.EmployeeId
                                 join engaged in _context.Engageds on account.AccountId equals engaged.AccountId
                                 join eventItem in _context.Events on engaged.EventId equals eventItem.EventId
                                 select new EmployeeEventViewModel
                                 {
                                     EmployeeId = employee.EmployeeId,
                                     FullName = employee.FullName,
                                     Contact = employee.Contact,
                                     Role = employee.Role.RoleName, // Assuming RoleName is a property in EmployeeRole
                                     EventId = eventItem.EventId,
                                     EventName = eventItem.EventName,
                                     EventStartDate = eventItem.StartDate,
                                     EventEndDate = eventItem.EndDate
                                 };

            _allEmployees = new ObservableCollection<EmployeeEventViewModel>(employeesToday.AsEnumerable()
                .Where(e => e.EventStartDate.ToDateTime(TimeOnly.MinValue) <= today && e.EventEndDate.ToDateTime(TimeOnly.MinValue) >= today)
                .Distinct()
                .ToList());

            Employees = new ObservableCollection<EmployeeEventViewModel>(_allEmployees);
            OnPropertyChanged(nameof(Employees));
        }

        public void SearchEmployees(string query)
        {
            var filteredEmployees = _allEmployees.Where(e =>
                e.EmployeeId.ToString().Contains(query, StringComparison.OrdinalIgnoreCase) ||
                e.FullName.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

            Employees = new ObservableCollection<EmployeeEventViewModel>(filteredEmployees);
            OnPropertyChanged(nameof(Employees));
        }

        private void AddNewEmployee(object parameter)
        {
            var employeeDescriptionWindow = new EmployeeDescription();
            employeeDescriptionWindow.Show();
        }

        private void EditEmployee(object parameter)
        {
            var employeeDescriptionWindow = new EmployeeDescription();
            employeeDescriptionWindow.Show();
        }

        private void DeleteEmployee(object parameter) { }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EmployeeEventViewModel
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Contact { get; set; }
        public string Role { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateOnly EventStartDate { get; set; }
        public DateOnly EventEndDate { get; set; }
    }
}
