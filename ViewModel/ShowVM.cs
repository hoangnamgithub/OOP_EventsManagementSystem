using OOP_EventsManagementSystem.Styles;
using OOP_EventsManagementSystem.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OOP_EventsManagementSystem.ViewModel
{
    class ShowVM : INotifyPropertyChanged
    {
        public ICommand AddCommand { get;set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        private bool _isButtonEnabled;
        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set
            {
                if (_isButtonEnabled != value)
                {
                    _isButtonEnabled = value;
                    OnPropertyChanged(nameof(IsButtonEnabled));
                    CommandManager.InvalidateRequerySuggested(); // Update CanExecute
                }
            }
        }

        public ShowVM() 
        {
            AddCommand = new RelayCommand (AddShowDetails);
            EditCommand = new RelayCommand(EditShowDetails);
            DeleteCommand = new RelayCommand(DeleteShowDetails);
            IsButtonEnabled = true;
        }

        private void AddShowDetails (object obj)
        {
            IsButtonEnabled = false; // Disable the button

            var showDescriptionWindow = new ShowDescription();
            showDescriptionWindow.Closed += (s, e) =>
            {
                IsButtonEnabled = true; // Enable the button when the window is closed
            };
            showDescriptionWindow.Show();
        }
        private void EditShowDetails(object obj)
        {
            IsButtonEnabled = false; // Disable the button

            var showDescriptionWindow = new ShowDescription();
            showDescriptionWindow.Closed += (s, e) =>
            {
                IsButtonEnabled = true; // Enable the button when the window is closed
            };
            showDescriptionWindow.Show();
        }
        private void DeleteShowDetails(object obj)
        {
            
        }
        private bool CanExecuteAddCommand(object obj)
        {
            return IsButtonEnabled;
        }

        // Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
