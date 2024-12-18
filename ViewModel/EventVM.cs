using Microsoft.EntityFrameworkCore;
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Styles;
using OOP_EventsManagementSystem.Utilities;
using OOP_EventsManagementSystem.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class EventVM : INotifyPropertyChanged
    {
        private readonly EventManagementDbContext _context;

        public ICommand OpenEventDetailCommand { get; set; }
        public ICommand ConfirmCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public PaginationHelper<Model.Event> UpcomingPagination { get; set; }
        public PaginationHelper<Model.Event> HappeningPagination { get; set; }
        public PaginationHelper<Model.Event> CompletedPagination { get; set; }
        public PaginationHelper<Model.Show> ShowsPagination { get; set; }
        public PaginationHelper<Model.Sponsor> SponsorsPagination { get; set; }

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

        public EventVM()
        {
            _context = new EventManagementDbContext();
            OpenEventDetailCommand = new RelayCommand(ExecuteOpenEventDetailCommand);
            ConfirmCommand = new RelayCommand(ExecuteConfirmCommand);
            NextPageCommand = new RelayCommand(ExecuteNextPage);
            PreviousPageCommand = new RelayCommand(ExecutePreviousPage);
            LoadData();
        }

        private void ExecuteConfirmCommand(object parameter)
        {
            // Implement confirmation logic here
        }

        private void ExecuteOpenEventDetailCommand(object obj)
        {
            if (obj is Model.Event selectedEvent)
            {
                var eventFromDb = _context.Events
                                          .Include(e => e.Venue)
                                          .Include(e => e.EventType)
                                          .FirstOrDefault(e => e.EventId == selectedEvent.EventId);

                var venues = _context.Venues.ToList();
                var eventTypes = _context.EventTypes.ToList();

                if (eventFromDb != null)
                {
                    var eventDescriptionWindow = new EventDetails
                    {
                        DataContext = new EventDetailsVM(eventFromDb, venues, eventTypes)
                    };
                    eventDescriptionWindow.Show();
                }
            }
        }

        private void LoadData()
        {
            var allEvents = _context.Events.Include(e => e.Venue).ToList();

            UpcomingPagination = new PaginationHelper<Model.Event>(
                allEvents.Where(e => e.StartDate.ToDateTime(TimeOnly.MinValue) > DateTime.Now)
            );

            HappeningPagination = new PaginationHelper<Model.Event>(
                allEvents.Where(e => e.StartDate.ToDateTime(TimeOnly.MinValue) <= DateTime.Now &&
                                     e.EndDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Now)
            );

            CompletedPagination = new PaginationHelper<Model.Event>(
                allEvents.Where(e => e.EndDate.ToDateTime(TimeOnly.MinValue) < DateTime.Now)
            );

            ShowsPagination = new PaginationHelper<Model.Show>(_context.Shows.Include(s => s.Performer).Include(s => s.Genre).ToList());
            SponsorsPagination = new PaginationHelper<Model.Sponsor>(_context.Sponsors.ToList());

            OnPropertyChanged(nameof(UpcomingPagination));
            OnPropertyChanged(nameof(HappeningPagination));
            OnPropertyChanged(nameof(CompletedPagination));
            OnPropertyChanged(nameof(ShowsPagination));
            OnPropertyChanged(nameof(SponsorsPagination));

            UpcomingEvents = new ObservableCollection<Model.Event>(
                allEvents.Where(e => e.StartDate.ToDateTime(TimeOnly.MinValue) > DateTime.Now)
            );

            HappeningEvents = new ObservableCollection<Model.Event>(
                allEvents.Where(e => e.StartDate.ToDateTime(TimeOnly.MinValue) <= DateTime.Now &&
                                     e.EndDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Now)
            );

            CompletedEvents = new ObservableCollection<Model.Event>(
                allEvents.Where(e => e.EndDate.ToDateTime(TimeOnly.MinValue) < DateTime.Now)
            );

            EventTypes = new ObservableCollection<Model.EventType>(_context.EventTypes.ToList());
            Venues = new ObservableCollection<Model.Venue>(_context.Venues.ToList());
            Shows = new ObservableCollection<Model.Show>(_context.Shows.Include(s => s.Performer).Include(s => s.Genre).ToList());
            Sponsors = new ObservableCollection<Model.Sponsor>(_context.Sponsors.ToList());
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
        }

        private void ExecuteNextPage(object parameter)
        {
            if (parameter?.ToString() == "Upcoming") UpcomingPagination.NextPage();
            if (parameter?.ToString() == "Happening") HappeningPagination.NextPage();
            if (parameter?.ToString() == "Completed") CompletedPagination.NextPage();
            if (parameter?.ToString() == "Shows") ShowsPagination.NextPage();
            if (parameter?.ToString() == "Sponsors") SponsorsPagination.NextPage();
        }

        private void ExecutePreviousPage(object parameter)
        {
            if (parameter?.ToString() == "Upcoming") UpcomingPagination.PreviousPage();
            if (parameter?.ToString() == "Happening") HappeningPagination.PreviousPage();
            if (parameter?.ToString() == "Completed") CompletedPagination.PreviousPage();
            if (parameter?.ToString() == "Shows") ShowsPagination.PreviousPage();
            if (parameter?.ToString() == "Sponsors") SponsorsPagination.PreviousPage();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
