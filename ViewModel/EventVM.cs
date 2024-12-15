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
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }


        private readonly EventManagementDbContext _context;

        public ObservableCollection<Model.Event> UpcomingEvents { get; set; }
        public ObservableCollection<Model.Event> HappeningEvents { get; set; }
        public ObservableCollection<Model.Event> CompletedEvents { get; set; }
        public ObservableCollection<Model.EventType> EventTypes { get; set; }
        public ObservableCollection<Model.Venue> Venues { get; set; }
        public ObservableCollection<Model.Show> Shows { get; set; }
        public ObservableCollection<Model.Sponsor> Sponsors { get; set; }
        public ObservableCollection<Model.Employee> Employees { get; set; }

        public ObservableCollection<Model.EmployeeRole> EmployeeRoles { get; set; }
        public ObservableCollection<Model.EquipmentName> EquipmentNames { get; set; }

        public ObservableCollection<Model.Show> PagedShows { get; set; }
        public int CurrentPage { get; private set; } = 1; // Trang hiện tại
        public int ItemsPerPage { get; set; } = 10; // Số lượng item mỗi trang


        // Phương thức cập nhật dữ liệu phân trang
        public void UpdatePagedShows()
        {
            if (Shows == null || Shows.Count == 0) return;

            PagedShows = new ObservableCollection<Model.Show>(
                Shows.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage));
            OnPropertyChanged(nameof(PagedShows));
        }
        public void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                UpdatePagedShows();
                OnPropertyChanged(nameof(CurrentPage));
                OnPropertyChanged(nameof(CanGoPrevious)); // Nếu Binding trực tiếp tới trạng thái
                OnPropertyChanged(nameof(CanGoNext));     // Nếu Binding trực tiếp tới trạng thái
            }
        }

        public void NextPage()
        {
            if (CurrentPage < Math.Ceiling((double)Shows.Count / ItemsPerPage))
            {
                CurrentPage++;
                UpdatePagedShows();
                OnPropertyChanged(nameof(CurrentPage));
                OnPropertyChanged(nameof(CanGoPrevious));
                OnPropertyChanged(nameof(CanGoNext)); // Thông báo cập nhật
            }
        }

        // Điều kiện để di chuyển tới trang trước
        public bool CanGoPrevious => CurrentPage >= 1;

        // Điều kiện để di chuyển tới trang kế
        public bool CanGoNext => CurrentPage < Math.Ceiling((double)Shows.Count / ItemsPerPage);


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

        // constructor -------------------------------------
        public EventVM()
        {
            AddCommand = new RelayCommand(ExecuteAddCommand, CanExecuteAddCommand);
            OpenEventDetailCommand = new RelayCommand(ExecuteOpenEventDetailCommand);
            IsAddButtonEnabled = true;
            CurrentDate = DateTime.Now;
            _context = new EventManagementDbContext();
            LoadData();

            // Khởi tạo các command
            ConfirmCommand = new Utilities.RelayCommand(ExecuteConfirmCommand);
            PreviousPageCommand = new Utilities.RelayCommand(ExecutePreviousPage, (obj) => CanGoPrevious);
            NextPageCommand = new Utilities.RelayCommand(ExecuteNextPage, (obj) => CanGoNext);
            UpdatePagedShows();
        }
        private void ExecuteConfirmCommand(object parameter)
        {
            // Thực hiện logic xác nhận tại đây
        }

        // Lệnh chuyển về trang trước
        private void ExecutePreviousPage(object parameter)
        {
            PreviousPage();
            CommandManager.InvalidateRequerySuggested(); // Cập nhật trạng thái các nút
        }

        // Lệnh chuyển tới trang kế
        private void ExecuteNextPage(object parameter)
        {
            NextPage();
            CommandManager.InvalidateRequerySuggested(); // Cập nhật trạng thái các nút
        }
       
        // Thực thi lệnh để mở cửa sổ EventDescription
        private void ExecuteOpenEventDetailCommand(object obj)
        {
            var eventDescriptionWindow = new EventDetails();
            eventDescriptionWindow.Show();

        }

        // method -------------------------------------
        private void LoadData()
        {
            var allEvents = _context.Events.Include(e => e.Venue).ToList();

            UpcomingEvents = new ObservableCollection<Model.Event>(allEvents.Where(e => e.StartDate.ToDateTime(TimeOnly.MinValue) > DateTime.Now));
            HappeningEvents = new ObservableCollection<Model.Event>(allEvents.Where(e => e.StartDate.ToDateTime(TimeOnly.MinValue) <= DateTime.Now && e.EndDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Now));
            CompletedEvents = new ObservableCollection<Model.Event>(allEvents.Where(e => e.EndDate.ToDateTime(TimeOnly.MinValue) < DateTime.Now));
            EventTypes = new ObservableCollection<Model.EventType>(_context.EventTypes.ToList());
            Venues = new ObservableCollection<Model.Venue>(_context.Venues.ToList());
            Shows = new ObservableCollection<Model.Show>(_context.Shows.Include(s => s.Performer).Include(s => s.Genre).ToList());
            Sponsors = new ObservableCollection<Sponsor>(_context.Sponsors.ToList());
            Employees = new ObservableCollection<Model.Employee>(_context.Employees.Include(e => e.Role).ToList());
            EmployeeRoles = new ObservableCollection<Model.EmployeeRole>(_context.EmployeeRoles.ToList());
            EquipmentNames = new ObservableCollection<Model.EquipmentName>(_context.EquipmentNames.ToList());

            OnPropertyChanged(nameof(UpcomingEvents));
            OnPropertyChanged(nameof(HappeningEvents));
            OnPropertyChanged(nameof(CompletedEvents));
            OnPropertyChanged(nameof(EventTypes));
            OnPropertyChanged(nameof(Venues));
            OnPropertyChanged(nameof(Shows));
            OnPropertyChanged(nameof(Sponsors));
            OnPropertyChanged(nameof(Employees));

            OnPropertyChanged(nameof(EmployeeRoles));
            OnPropertyChanged(nameof(EquipmentNames));

            UpdatePagedShows();

        }

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
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
