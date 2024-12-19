using Microsoft.EntityFrameworkCore;
using OOP_EventsManagementSystem.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class EventVM : INotifyPropertyChanged
    {
        private readonly EventManagementDbContext _context;

        private ObservableCollection<Model.Event> _events;
        public ObservableCollection<Model.Event> Events
        {
            get => _events;
            set
            {
                _events = value;
                OnPropertyChanged(nameof(Events));
            }
        }

        private ObservableCollection<Venue> _venues;
        public ObservableCollection<Venue> Venues
        {
            get => _venues;
            set
            {
                _venues = value;
                OnPropertyChanged(nameof(Venues));
            }
        }

        public EventVM(EventManagementDbContext context)
        {
            _context = context;

            // Load data from the database
            LoadData();
        }

        private void LoadData()
        {
            var eventList = _context.Events
                .Include(e => e.Venue)
                .Select(e => new Event
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    StartDate = DateOnly.FromDateTime(e.StartDate.ToDateTime(TimeOnly.MinValue)),
                    EndDate = DateOnly.FromDateTime(e.EndDate.ToDateTime(TimeOnly.MinValue)),
                    Venue = e.Venue
                })
                .ToList();

            Events = new ObservableCollection<Model.Event>(eventList);
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
