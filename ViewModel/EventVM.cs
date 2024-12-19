using Microsoft.EntityFrameworkCore;
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Styles;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

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
                UpdateCategorizedCollections(); // Update filtered collections when Events is updated
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

    }
}
