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
            get => _currentDate;
            set
            {
                if (_currentDate != value)
                {
                    _currentDate = value;
                    OnPropertyChanged(nameof(CurrentDate));
                }
            }
        }

        private bool _isAddButtonEnabled;
        public bool IsAddButtonEnabled
        {
            get => _isAddButtonEnabled;
            set
            {
                if (_isAddButtonEnabled != value)
                {
                    _isAddButtonEnabled = value;
                    OnPropertyChanged(nameof(IsAddButtonEnabled));
                    CommandManager.InvalidateRequerySuggested(); // Update CanExecute
                }
            }
        }

        public EventVM()
        {
            AddCommand = new RelayCommand(ExecuteAddCommand, CanExecuteAddCommand);
            IsAddButtonEnabled = true;
            CurrentDate = DateTime.Now;
        }

        private void ExecuteAddCommand(object obj)
        {
            IsAddButtonEnabled = false; // Disable the button

            var eventDescriptionWindow = new EventDescription();
            eventDescriptionWindow.Closed += (s, e) =>
            {
                IsAddButtonEnabled = true; // Enable the button when the window is closed
            };
            eventDescriptionWindow.Show();
        }

        private bool CanExecuteAddCommand(object obj)
        {
            return IsAddButtonEnabled;
        }

        // Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}