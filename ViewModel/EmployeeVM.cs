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
        private Event _selectedEvent;

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
                    .Distinct() // Ensure no duplicates if an employee is engaged in multiple events
                    .ToList();

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

                EngagedEmployees = new ObservableCollection<Employee>(employees);
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
