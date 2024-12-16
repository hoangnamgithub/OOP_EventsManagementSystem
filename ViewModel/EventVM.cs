using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Styles;
using OOP_EventsManagementSystem.Utilities;
using OOP_EventsManagementSystem.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class EventVM : INotifyPropertyChanged
    {
        public ICommand AddCommand { get; set; }
        public ICommand OpenEventDetailCommand { get; set; }
        public ICommand ConfirmCommand { get; }
        public ICommand PreviousPageCommand => new RelayCommand(GoToPreviousPage, () => CanGoPrevious);
        public ICommand NextPageCommand => new RelayCommand(GoToNextPage, () => CanGoNext);
        private const int ItemsPerPage = 9;
        private int _currentPage;
        
       

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
                UpdateCurrentPageItems();
            }
        }

        public bool CanGoPrevious => CurrentPage > 1;
        public bool CanGoNext => CurrentPage < (int)Math.Ceiling((double)UpcomingEvents.Count / ItemsPerPage);

        


        private readonly EventManagementDbContext _context;
        public ObservableCollection<Model.Event> CurrentPageItems { get; set; }
        public ObservableCollection<Model.Event> UpcomingEvents { get; set; }
        public ObservableCollection<Model.Event> HappeningEvents { get; set; }
        public ObservableCollection<Model.Event> CompletedEvents { get; set; }
        public ObservableCollection<Model.EventType> EventTypes { get; set; }
        public ObservableCollection<Model.Venue> Venues { get; set; }
        public ObservableCollection<Model.Show> Shows { get; set; }
        public ObservableCollection<Model.Sponsor> Sponsors { get; set; }
        public ObservableCollection<Model.Employee> Employees { get; set; }
        public ObservableCollection<Model.Show> PagedShows { get; set; }        

        // constructor -------------------------------------
        public EventVM()
        {
            AddCommand = new RelayCommand(ExecuteAddCommand, CanExecuteAddCommand);
            OpenEventDetailCommand = new RelayCommand(ExecuteOpenEventDetailCommand);
            IsAddButtonEnabled = true;
            CurrentDate = DateTime.Now;
            _context = new EventManagementDbContext();
           

            // Khởi tạo các command
            ConfirmCommand = new Utilities.RelayCommand(ExecuteConfirmCommand);
            CurrentPage = 1;
            CurrentPageItems = new ObservableCollection<Model.Event>();
            UpdateCurrentPageItems();


        }
        private void UpdateCurrentPageItems()
        {
            CurrentPageItems.Clear();
            var items = UpcomingEvents.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage);
            foreach (var item in items)
            {
                CurrentPageItems.Add(item);
            }

            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));
        }

        private void GoToPreviousPage()
        {
            if (CanGoPrevious)
            {
                CurrentPage--;
            }
        }

        private void GoToNextPage()
        {
            if (CanGoNext)
            {
                CurrentPage++;
            }
        }


        private void ExecuteConfirmCommand(object parameter)
        {
            // Thực hiện logic xác nhận tại đây
        }
    
        // Thực thi lệnh để mở cửa sổ EventDescription
        private void ExecuteOpenEventDetailCommand(object obj)
        {
            var eventDescriptionWindow = new EventDetails();
            eventDescriptionWindow.Show();
            
        }

        // method -------------------------------------

        private void ExecuteAddCommand(object obj)
        {
            IsAddButtonEnabled = false; // Disable the button

            var eventDescriptionWindow = new EventDetails();
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
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        // properties -------------------------------------
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

            private string _eventName;
            public string EventName
            {
                get => _eventName;
                set
                {
                    if (_eventName != value)
                    {
                        _eventName = value;
                        OnPropertyChanged(nameof(EventName));
                    }
                }
            }
    }
}
