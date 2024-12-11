using OOP_EventsManagementSystem.Styles;
using System;
using System.ComponentModel;
using System.Windows.Input;
using OOP_EventsManagementSystem.Utilities;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class EventVM : INotifyPropertyChanged
    {
        public ICommand AddCommand { get; set; }
        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                if (_currentDate != value)
                {
                    _currentDate = value;
                    OnPropertyChanged(nameof(CurrentDate));
                }
            }
        }

        // Constructor
        public EventVM()
        {
            AddCommand = new RelayCommand(ExecuteAddCommand);
            CurrentDate = DateTime.Now;
        }
        private void ExecuteAddCommand(object obj)
        {
            // Logic khi nhấn nút Add (mở ShowDescription)
            var eventDescriptionWindow = new EventDescription();
            eventDescriptionWindow.Show();
        }

        // Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }

}
