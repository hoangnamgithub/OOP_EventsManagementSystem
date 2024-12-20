using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Styles;
using OOP_EventsManagementSystem.Utilities;


namespace OOP_EventsManagementSystem.ViewModel
{
    class EmployeeVM : INotifyPropertyChanged
    {

        private readonly EventManagementDbContext _context;
        public ObservableCollection<EmployeeDisplayModel> TodayEmployees { get; set; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public EmployeeVM(EventManagementDbContext context) 
        {
            AddCommand = new RelayCommand(AddNewEmployee);
            EditCommand = new RelayCommand(EditEmployee);
            DeleteCommand = new RelayCommand(DeleteEmployee);

            _context = context; 
            LoadData();

        }

        private void LoadData()
        {
            var today = DateTime.Today;
            var engagedToday = _context.Engageds
                .Include(e => e.Employee)
                .Include(e => e.Event)
                .Include(e => e.Employee.Role)
                .Where(e => e.Event.StartDate.ToDateTime(TimeOnly.MinValue) <= today &&
                            e.Event.EndDate.ToDateTime(TimeOnly.MinValue) >= today)
                .Select(e => new EmployeeDisplayModel
                {
                    FullName = e.Employee.FullName,
                    Contact = e.Employee.Contact,
                    RoleName = e.Employee.Role.RoleName,
                    EventName = e.Event.EventName
                })
                .ToList();

            TodayEmployees = new ObservableCollection<EmployeeDisplayModel>(engagedToday);
            OnPropertyChanged(nameof(TodayEmployees));
        }

        private void AddNewEmployee (Object parameter) 
        {
            // Logic khi nhấn nút Add (mở ShowDescription)
            var employeeDescriptionWindow = new EmployeeDescription();
            employeeDescriptionWindow.Show();
        }
        private void EditEmployee(Object parameter) 
        {
            // Logic khi nhấn nút Add (mở ShowDescription)
            var employeeDescriptionWindow = new EmployeeDescription();
            employeeDescriptionWindow.Show();
        }
        private void DeleteEmployee(Object parameter) { }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
    public class EmployeeDisplayModel
    {
        public string FullName { get; set; }
        public string Contact { get; set; }
        public string RoleName { get; set; }
        public string EventName { get; set; }
    }

}
