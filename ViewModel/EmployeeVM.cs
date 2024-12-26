using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Styles;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class EmployeeVM : INotifyPropertyChanged
    {
        private readonly EventManagementDbContext _context;

        private ObservableCollection<Event> _todayEvents;
        private ObservableCollection<Employee> _engagedEmployees;
        private ObservableCollection<Employee> _employees;
        private ObservableCollection<EmployeeRole> _employeeRoles;
        private Event _selectedEvent;
        private EmployeeRole _selectedEmployeeRole;
        private string _employeeName;
        private string _employeeContact;
        private string _employeeAccount;
        private string _employeePassword;

        public ObservableCollection<Event> TodayEvents
        {
            get => _todayEvents;
            set
            {
                _todayEvents = value;
                OnPropertyChanged(nameof(TodayEvents));
            }
        }

        public ObservableCollection<Employee> EngagedEmployees
        {
            get => _engagedEmployees;
            set
            {
                _engagedEmployees = value;
                OnPropertyChanged(nameof(EngagedEmployees));
            }
        }

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public ObservableCollection<EmployeeRole> EmployeeRoles
        {
            get => _employeeRoles;
            set
            {
                _employeeRoles = value;
                OnPropertyChanged(nameof(EmployeeRoles));
            }
        }

        public EmployeeRole SelectedEmployeeRole
        {
            get => _selectedEmployeeRole;
            set
            {
                _selectedEmployeeRole = value;
                OnPropertyChanged(nameof(SelectedEmployeeRole));
            }
        }

        public string EmployeeName
        {
            get => _employeeName;
            set
            {
                _employeeName = value;
                OnPropertyChanged(nameof(EmployeeName));
                EmployeeAccount = $"{_employeeName}@easys.com";
            }
        }

        public string EmployeeContact
        {
            get => _employeeContact;
            set
            {
                _employeeContact = value;
                OnPropertyChanged(nameof(EmployeeContact));
            }
        }

        public string EmployeeAccount
        {
            get => _employeeAccount;
            set
            {
                _employeeAccount = value;
                OnPropertyChanged(nameof(EmployeeAccount));
            }
        }

        public string EmployeePassword
        {
            get => _employeePassword;
            set
            {
                _employeePassword = value;
                OnPropertyChanged(nameof(EmployeePassword));
            }
        }

        public Event SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                OnPropertyChanged(nameof(SelectedEvent));
                LoadEngagedEmployees();
            }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    LoadEngagedEmployees();
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand GeneratePasswordCommand { get; }
        public ICommand ConfirmAddEmployeeCommand { get; }

        public EmployeeVM(EventManagementDbContext context)
        {
            _context = context;
            AddCommand = new RelayCommand(AddNewEmployee);
            EditCommand = new RelayCommand(EditEmployee);
            DeleteCommand = new RelayCommand(DeleteEmployee);
            GeneratePasswordCommand = new RelayCommand(GeneratePassword);
            ConfirmAddEmployeeCommand = new RelayCommand(ConfirmAddEmployee);

            LoadTodayEvents();
            LoadAllEmployees();
            LoadEmployeeRoles();
            EngagedEmployees = new ObservableCollection<Employee>();
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

        private void DeleteEmployee(object parameter)
        {
            // Logic to delete an employee
        }

        private void LoadTodayEvents()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var events = _context.Events
                .Where(e => e.StartDate <= today && e.EndDate >= today)
                .ToList();

            TodayEvents = new ObservableCollection<Event>(events);
        }

        private void LoadAllEmployees()
        {
            var employees = _context.Employees.ToList();
            Employees = new ObservableCollection<Employee>(employees);
        }

        private void LoadEmployeeRoles()
        {
            var roles = _context.EmployeeRoles.ToList();
            EmployeeRoles = new ObservableCollection<EmployeeRole>(roles);
        }

        private void LoadEngagedEmployees()
        {
            if (SelectedEvent == null)
            {
                var today = DateOnly.FromDateTime(DateTime.Today);

                var engagedEmployees = _context.Engageds
                    .Include(e => e.Account)
                    .ThenInclude(a => a.Employee)
                    .Where(e => e.Event.StartDate <= today && e.Event.EndDate >= today)
                    .Select(e => e.Account.Employee)
                    .Distinct()
                    .ToList();

                if (!string.IsNullOrEmpty(SearchText))
                {
                    engagedEmployees = engagedEmployees
                        .Where(e => e.EmployeeId.ToString().Contains(SearchText) ||
                                    e.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                EngagedEmployees = new ObservableCollection<Employee>(engagedEmployees);
            }
            else
            {
                var employees = _context.Engageds
                    .Include(e => e.Account)
                    .ThenInclude(a => a.Employee)
                    .Where(e => e.EventId == SelectedEvent.EventId)
                    .Select(e => e.Account.Employee)
                    .ToList();

                if (!string.IsNullOrEmpty(SearchText))
                {
                    employees = employees
                        .Where(e => e.EmployeeId.ToString().Contains(SearchText) ||
                                    e.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                EngagedEmployees = new ObservableCollection<Employee>(employees);
            }
        }

        private void GeneratePassword(object parameter)
        {
            EmployeePassword = GenerateRandomPassword();
        }

        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var password = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }
            return password.ToString();
        }

        private void ConfirmAddEmployee(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to confirm adding this employee?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var newEmployee = new Employee
                {
                    FullName = EmployeeName,
                    Contact = EmployeeContact,
                    RoleId = SelectedEmployeeRole.RoleId,
                };

                _context.Employees.Add(newEmployee);
                _context.SaveChanges();

                var newAccount = new Account
                {
                    Email = EmployeeAccount,
                    Password = EmployeePassword,
                    PermissionId = 3,
                    EmployeeId = newEmployee.EmployeeId,
                };

                _context.Accounts.Add(newAccount);
                _context.SaveChanges();

                MessageBox.Show("Employee added successfully!");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
