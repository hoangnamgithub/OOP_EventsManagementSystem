using Microsoft.EntityFrameworkCore;
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Styles;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using OOP_EventsManagementSystem.ViewModel;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class EventVM : INotifyPropertyChanged
    {
        private readonly EventManagementDbContext _context;

        private ObservableCollection<EventViewModel> _events;
        public ObservableCollection<EventViewModel> Events
        {
            get => _events;
            set
            {
                _events = value;
                OnPropertyChanged(nameof(Events));
                UpdateCategorizedCollections();
            }
        }

        private ObservableCollection<EventViewModel> _upcomingEvents;
        public ObservableCollection<EventViewModel> UpcomingEvents
        {
            get => _upcomingEvents;
            set
            {
                _upcomingEvents = value;
                OnPropertyChanged(nameof(UpcomingEvents));
            }
        }

        private ObservableCollection<EventViewModel> _happeningEvents;
        public ObservableCollection<EventViewModel> HappeningEvents
        {
            get => _happeningEvents;
            set
            {
                _happeningEvents = value;
                OnPropertyChanged(nameof(HappeningEvents));
            }
        }

        private ObservableCollection<EventViewModel> _completedEvents;
        public ObservableCollection<EventViewModel> CompletedEvents
        {
            get => _completedEvents;
            set
            {
                _completedEvents = value;
                OnPropertyChanged(nameof(CompletedEvents));
            }
        }

        public EventVM(EventManagementDbContext context)
        {
            _context = context;
            LoadData();
        }

        private void LoadData()
        {
            var eventList = _context.Events
                .Include(e => e.Venue)
                .Select(e => new EventViewModel(e))
                .ToList();

            Events = new ObservableCollection<EventViewModel>(eventList);
        }

        private void UpdateCategorizedCollections()
        {
            var today = DateTime.Now.Date;

            // Filter the collections
            UpcomingEvents = new ObservableCollection<EventViewModel>(
                Events.Where(e => e.StartDateAsDateTime > today));

            HappeningEvents = new ObservableCollection<EventViewModel>(
                Events.Where(e => e.StartDateAsDateTime <= today && e.EndDateAsDateTime >= today));

            CompletedEvents = new ObservableCollection<EventViewModel>(
                Events.Where(e => e.EndDateAsDateTime < today));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EventViewModel
    {
        private readonly Event _event;

        public EventViewModel(Event eventModel)
        {
            _event = eventModel;
            OpenEventDetailCommand = new RelayCommand(param => OpenEventDetail(), param => true);
        }

        public string EventName => _event.EventName;

        public string VenueName => _event.Venue.VenueName;

        public string StartDate => _event.StartDate.ToDateTime(TimeOnly.MinValue).ToString("dd/MM/yyyy");

        public string EndDate => _event.EndDate.ToDateTime(TimeOnly.MinValue).ToString("dd/MM/yyyy");

        public DateTime StartDateAsDateTime => _event.StartDate.ToDateTime(TimeOnly.MinValue);

        public DateTime EndDateAsDateTime => _event.EndDate.ToDateTime(TimeOnly.MinValue);

        public ICommand OpenEventDetailCommand { get; }

        private void OpenEventDetail()
        {
            // Create a new instance of the EventDetailVM to pass the event data
            var eventDetailVM = new EventDetailsVM
            {
                EventName = _event.EventName,
                ExpectedAttendee = _event.ExptedAttendee.ToString(), // Convert to string
                VenueName = _event.Venue.VenueName,
                EventDescription = _event.EventDescription, // Map to EventDescription
                StartDate = _event.StartDate.ToDateTime(TimeOnly.MinValue), // Convert DateOnly to DateTime
                EndDate = _event.EndDate.ToDateTime(TimeOnly.MinValue) // Convert DateOnly to DateTime
            };

            // Create and show the EventDetails window
            var eventDetailsWindow = new EventDetails
            {
                DataContext = eventDetailVM // Pass the EventDetailVM as the DataContext
            };
            eventDetailsWindow.ShowDialog(); // Open the window as a dialog
        }
    }
}
