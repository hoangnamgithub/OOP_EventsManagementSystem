using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OOP_EventsManagementSystem.Styles;
using OOP_EventsManagementSystem.Utilities;


namespace OOP_EventsManagementSystem.ViewModel
{
    class EmployeeVM : INotifyPropertyChanged
    {
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public EmployeeVM() 
        {
            AddCommand = new RelayCommand(AddNewEmployee);
            EditCommand = new RelayCommand(EditEmployee);
            DeleteCommand = new RelayCommand(DeleteEmployee);

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
