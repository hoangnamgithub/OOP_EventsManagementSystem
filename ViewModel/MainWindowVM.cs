using System.ComponentModel;
using System.Windows.Input;
using OOP_EventsManagementSystem.View;
using OOP_EventsManagementSystem.Utilities;

namespace OOP_EventsManagementSystem
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ICommand HomeCommand { get; }
        public ICommand EventCommand { get; }
        public ICommand ShowCommand { get; }
        public ICommand PartnerCommand { get; }
        public ICommand EmployeeCommand { get; }
        public ICommand EquipmentCommand { get; }
        public ICommand LocationCommand { get; }

        public MainWindowVM()
        {
            HomeCommand = new RelayCommand(ExecuteHomeCommand);
            EventCommand = new RelayCommand(ExecuteEventCommand);
            ShowCommand = new RelayCommand(ExecuteShowCommand);
            PartnerCommand = new RelayCommand(ExecutePartnerCommand);
            EmployeeCommand = new RelayCommand(ExecuteEmployeeCommand);
            EquipmentCommand = new RelayCommand(ExecuteEquipmentCommand);
            LocationCommand = new RelayCommand(ExecuteLocationCommand);

            // Đặt view mặc định là Event
            CurrentView = new Home();
        }

        private void ExecuteHomeCommand(object parameter)
        {
            CurrentView = new Home();
        }

        private void ExecuteEventCommand(object parameter)
        {
            CurrentView = new Event();
        }

        private void ExecuteShowCommand(object parameter)
        {
            CurrentView = new Show();
        }

        private void ExecutePartnerCommand(object parameter)
        {
            CurrentView = new Partner();
        }

        private void ExecuteEmployeeCommand(object parameter)
        {
            CurrentView = new Employee();
        }

        private void ExecuteEquipmentCommand(object parameter)
        {
            CurrentView = new Equipment();
        }

        private void ExecuteLocationCommand(object parameter)
        {
            CurrentView = new Location();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
