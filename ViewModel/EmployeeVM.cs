using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Styles;

namespace OOP_EventsManagementSystem.ViewModel
{
    class EmployeeVM : INotifyPropertyChanged
    {
        private readonly EventManagementDbContext _context;

        // Observable collection to bind to the DataGrid
        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public EmployeeVM(EventManagementDbContext context)
        {
            AddCommand = new RelayCommand(AddNewEmployee);
            EditCommand = new RelayCommand(EditEmployee);
            DeleteCommand = new RelayCommand(DeleteEmployee);

            _context = context;
            _employees = new ObservableCollection<Employee>();

            LoadEmployeesEngagedToday();
        }

        // Fetch employees engaged in today's event from the database
        private async void LoadEmployeesEngagedToday()
        {
            // Get today's date
            var today = DateOnly.FromDateTime(DateTime.Now);

            // Query for employees engaged in today's event
            var employeesFromDb = await _context.Employees
                .Where(e => e.Accounts
                    .Any(a => a.Engageds
                        .Any(eng => eng.Event.StartDate == today)))
                .Include(e => e.Role)  // Include role data
                .Include(e => e.Accounts)  // Include related accounts
                    .ThenInclude(a => a.Engageds)  // Include engagements related to accounts
                    .ThenInclude(eng => eng.Event)  // Include events related to engagements
                .ToListAsync();

            Employees = new ObservableCollection<Employee>(employeesFromDb);
        }

        private void AddNewEmployee(Object parameter)
        {
            var employeeDescriptionWindow = new EmployeeDescription();
            employeeDescriptionWindow.Show();
        }

        private void EditEmployee(Object parameter)
        {
            var employeeDescriptionWindow = new EmployeeDescription();
            employeeDescriptionWindow.Show();
        }

        private void DeleteEmployee(Object parameter)
        {
            // Logic to delete an employee
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
