using OOP_EventsManagementSystem.Utilities;
using OOP_EventsManagementSystem.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using OOP_EventsManagementSystem.Model;
using Microsoft.Identity.Client;
using OOP_EventsManagementSystem.Styles;


namespace OOP_EventsManagementSystem.ViewModel
{
    public class EventVM : INotifyPropertyChanged
    {
        public ICommand AddCommand { get; set; }
        private readonly EventManagementDbContext _context;

        public ObservableCollection<Model.Event> Events { get; set; }

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
            _context = new EventManagementDbContext();
            LoadData();
        }
        private void LoadData()
        {
            Events = new ObservableCollection<Model.Event>(_context.Events.Include(e => e.Venue).ToList());
            OnPropertyChanged(nameof(Events));
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