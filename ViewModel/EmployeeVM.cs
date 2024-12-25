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

        // Private fields for properties
        private ObservableCollection<Event> _todayEvents;
        private ObservableCollection<EngagedEmployeeWithAccount> _engagedEmployees;
        private ObservableCollection<EmployeeWithAccount> _employees;
        private ObservableCollection<EmployeeRole> _employeeRoles;
        private Event _selectedEvent;
        private EmployeeRole _selectedEmployeeRole;
        private EmployeeWithAccount _selectedEmployee;
        private string _employeeName;
        private string _employeeContact;
        private string _employeeAccount;
        private string _employeePassword;
        private string _searchText = string.Empty;

        // Public properties with change notification
        public ObservableCollection<Event> TodayEvents
        {
            get => _todayEvents;
            set
            {
                _todayEvents = value;
                OnPropertyChanged(nameof(TodayEvents));
            }
        }

        public ObservableCollection<EngagedEmployeeWithAccount> EngagedEmployees
        {
            get => _engagedEmployees;
            set
            {
                _engagedEmployees = value;
                OnPropertyChanged(nameof(EngagedEmployees));
            }
        }

        public ObservableCollection<EmployeeWithAccount> Employees
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

        public EmployeeWithAccount SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
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

        // Commands
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand GeneratePasswordCommand { get; }
        public ICommand ConfirmAddEmployeeCommand { get; }
        public ICommand DelCommand { get; }

        // Constructor
        public EmployeeVM(EventManagementDbContext context)
        {
            _context = context;
            AddCommand = new RelayCommand(AddNewEmployee);
            EditCommand = new RelayCommand(EditEmployee);
            DeleteCommand = new RelayCommand(DeleteSelectedEmployee);
            GeneratePasswordCommand = new RelayCommand(GeneratePassword);
            ConfirmAddEmployeeCommand = new RelayCommand(ConfirmAddEmployee);
            DelCommand = new RelayCommand(DeleteSelectedEmployee);
            EngagedEmployees = new ObservableCollection<EngagedEmployeeWithAccount>();

            LoadTodayEvents();
            LoadAllEmployees();
            LoadEmployeeRoles();
        }

        // Method to add a new employee
        private void AddNewEmployee(object parameter)
        {
            var employeeDescriptionWindow = new EmployeeDescription();
            employeeDescriptionWindow.Show();
        }

        // Method to edit an existing employee
        private void EditEmployee(object parameter)
        {
            var employeeDescriptionWindow = new EmployeeDescription();
            employeeDescriptionWindow.Show();
        }

        // Method to delete an employee
        private void DeleteSelectedEmployee(object parameter)
        {
            if (SelectedEmployee == null)
            {
                MessageBox.Show(
                    "Please select an employee to delete.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete {SelectedEmployee.FullName}?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var employee = _context
                        .Employees.Include(e => e.Accounts)
                        .FirstOrDefault(e => e.EmployeeId == SelectedEmployee.EmployeeId);
                    if (employee != null)
                    {
                        // Delete associated accounts
                        _context.Accounts.RemoveRange(employee.Accounts);
                        // Delete the employee
                        _context.Employees.Remove(employee);
                        _context.SaveChanges();

                        // Reload the Employees collection
                        LoadAllEmployees();

                        MessageBox.Show("Employee deleted successfully!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        // Method to load today's events
        private void LoadTodayEvents()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var events = _context
                .Events.Where(e => e.StartDate <= today && e.EndDate >= today)
                .ToList();
            TodayEvents = new ObservableCollection<Event>(events);
        }

        // Method to load all employees with account information
        private void LoadAllEmployees()
        {
            var employeesWithAccounts = _context
                .Employees.Include(e => e.Accounts)
                .Select(e => new EmployeeWithAccount
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName,
                    Contact = e.Contact,
                    Email = e.Accounts.FirstOrDefault().Email,
                    Password = e.Accounts.FirstOrDefault().Password,
                })
                .ToList();

            Employees = new ObservableCollection<EmployeeWithAccount>(employeesWithAccounts);
            OnPropertyChanged(nameof(Employees)); // Ensure property change notification is raised
        }

        // Method to load employee roles
        private void LoadEmployeeRoles()
        {
            var roles = _context.EmployeeRoles.ToList();
            EmployeeRoles = new ObservableCollection<EmployeeRole>(roles);
        }

        // Method to load engaged employees based on the selected event
        private void LoadEngagedEmployees()
        {
            if (SelectedEvent == null)
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var engagedEmployeesWithAccounts = _context
                    .Engageds.Include(e => e.Account)
                    .ThenInclude(a => a.Employee)
                    .Where(e => e.Event.StartDate <= today && e.Event.EndDate >= today)
                    .Select(e => new EngagedEmployeeWithAccount
                    {
                        EmployeeId = e.Account.Employee.EmployeeId,
                        FullName = e.Account.Employee.FullName,
                        Contact = e.Account.Employee.Contact,
                        Email = e.Account.Email,
                        Password = e.Account.Password,
                    })
                    .Distinct()
                    .ToList();

                if (!string.IsNullOrEmpty(SearchText))
                {
                    engagedEmployeesWithAccounts = engagedEmployeesWithAccounts
                        .Where(e =>
                            e.EmployeeId.ToString().Contains(SearchText)
                            || e.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                        )
                        .ToList();
                }

                EngagedEmployees = new ObservableCollection<EngagedEmployeeWithAccount>(
                    engagedEmployeesWithAccounts
                );
            }
            else
            {
                var engagedEmployeesWithAccounts = _context
                    .Engageds.Include(e => e.Account)
                    .ThenInclude(a => a.Employee)
                    .Where(e => e.EventId == SelectedEvent.EventId)
                    .Select(e => new EngagedEmployeeWithAccount
                    {
                        EmployeeId = e.Account.Employee.EmployeeId,
                        FullName = e.Account.Employee.FullName,
                        Contact = e.Account.Employee.Contact,
                        Email = e.Account.Email,
                        Password = e.Account.Password,
                    })
                    .ToList();

                if (!string.IsNullOrEmpty(SearchText))
                {
                    engagedEmployeesWithAccounts = engagedEmployeesWithAccounts
                        .Where(e =>
                            e.EmployeeId.ToString().Contains(SearchText)
                            || e.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                        )
                        .ToList();
                }

                EngagedEmployees = new ObservableCollection<EngagedEmployeeWithAccount>(
                    engagedEmployeesWithAccounts
                );
            }
        }

        // Method to generate a random password
        private void GeneratePassword(object parameter)
        {
            EmployeePassword = GenerateRandomPassword();
        }

        // Helper method to generate a random password
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

        // Method to confirm adding a new employee
        private void ConfirmAddEmployee(object parameter)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(EmployeeName))
            {
                MessageBox.Show(
                    "Employee name cannot be empty.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            if (string.IsNullOrWhiteSpace(EmployeeContact))
            {
                MessageBox.Show(
                    "Employee contact cannot be empty.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            if (SelectedEmployeeRole == null)
            {
                MessageBox.Show(
                    "Employee role must be selected.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            if (string.IsNullOrWhiteSpace(EmployeePassword))
            {
                MessageBox.Show(
                    "Employee password cannot be empty.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "Do you want to confirm adding this employee?",
                "Confirmation",
                MessageBoxButton.YesNo
            );
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var newEmployee = new Employee
                    {
                        FullName = EmployeeName,
                        Contact = EmployeeContact,
                        RoleId = SelectedEmployeeRole.RoleId,
                    };

                    _context.Employees.Add(newEmployee);
                    _context.SaveChanges();

                    // Get the latest EmployeeId and increment it by 1
                    var latestEmployeeId =
                        _context
                            .Employees.OrderByDescending(e => e.EmployeeId)
                            .FirstOrDefault()
                            ?.EmployeeId ?? 0;
                    var fakeEmployeeId = latestEmployeeId + 1;

                    // Generate the email using the fake EmployeeId
                    var sanitizedEmployeeName = EmployeeName.Replace(" ", "").ToLower();
                    var email = $"{sanitizedEmployeeName}{fakeEmployeeId}@sys.easys.com";

                    var newAccount = new Account
                    {
                        Email = email,
                        Password = EmployeePassword,
                        PermissionId = 3,
                        EmployeeId = newEmployee.EmployeeId,
                    };

                    _context.Accounts.Add(newAccount);
                    _context.SaveChanges();

                    // Reload the Employees collection
                    LoadAllEmployees();

                    MessageBox.Show("Employee added successfully!");

                    // Close the window if a parameter is passed
                    if (parameter is Window window)
                    {
                        window.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        // Event for property change notification
        public event PropertyChangedEventHandler? PropertyChanged;

        // Method to raise property change notification
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // New class to represent the combined data for employees
    public class EmployeeWithAccount
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // New class to represent the combined data for engaged employees
    public class EngagedEmployeeWithAccount
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
