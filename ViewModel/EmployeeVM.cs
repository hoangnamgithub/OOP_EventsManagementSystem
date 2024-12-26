using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        private ObservableCollection<Employee> _employees;  // New collection for all employees
        private ObservableCollection<EmployeeRole> _employeeRoles; // New collection for employee roles
        private Event _selectedEvent;
        private EmployeeRole _selectedEmployeeRole; // New property for selected employee role

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

        public ObservableCollection<Employee> Employees  // New property for all employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public ObservableCollection<EmployeeRole> EmployeeRoles // New property for employee roles
        {
            get => _employeeRoles;
            set
            {
                _employeeRoles = value;
                OnPropertyChanged(nameof(EmployeeRoles));
            }
        }

        public EmployeeRole SelectedEmployeeRole // New property for selected employee role
        {
            get => _selectedEmployeeRole;
            set
            {
                _selectedEmployeeRole = value;
                OnPropertyChanged(nameof(SelectedEmployeeRole));
            }
        }

        public Event SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                OnPropertyChanged(nameof(SelectedEvent));
                LoadEngagedEmployees(); // Update the second DataGrid
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
                    LoadEngagedEmployees(); // Re-filter when search text changes
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public EmployeeVM(EventManagementDbContext context)
        {
            _context = context;
            AddCommand = new RelayCommand(AddNewEmployee);
            EditCommand = new RelayCommand(EditEmployee);
            DeleteCommand = new RelayCommand(DeleteEmployee);

            LoadTodayEvents();
            LoadAllEmployees(); // Load all employees initially
            LoadEmployeeRoles(); // Load employee roles
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
            // Fetch all employees from the database
            var employees = _context.Employees.ToList();
            Employees = new ObservableCollection<Employee>(employees);
        }

        private void LoadEmployeeRoles()
        {
            // Fetch all employee roles from the database
            var roles = _context.EmployeeRoles.ToList();
            EmployeeRoles = new ObservableCollection<EmployeeRole>(roles);
        }

        private void LoadEngagedEmployees()
        {
            if (SelectedEvent == null)
            {
                // If no event is selected, load all employees engaged in today's events
                var today = DateOnly.FromDateTime(DateTime.Today);

                var engagedEmployees = _context.Engageds
                    .Include(e => e.Account)
                    .ThenInclude(a => a.Employee)
                    .Where(e => e.Event.StartDate <= today && e.Event.EndDate >= today)
                    .Select(e => e.Account.Employee)
                    .Distinct() // Ensure no duplicates
                    .ToList();

                // Apply filtering based on SearchText
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
                // Load employees engaged in the selected event
                var employees = _context.Engageds
                    .Include(e => e.Account)
                    .ThenInclude(a => a.Employee)
                    .Where(e => e.EventId == SelectedEvent.EventId)
                    .Select(e => e.Account.Employee)
                    .ToList();

                // Apply filtering based on SearchText
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
