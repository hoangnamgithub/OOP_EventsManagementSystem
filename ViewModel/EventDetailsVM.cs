using OOP_EventsManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class EventDetailsVM : INotifyPropertyChanged
    {
        private OOP_EventsManagementSystem.Model.Event _selectedEvent;
        private int _selectedVenueId;
        private int _selectedEventTypeId;
        private List<Venue> _venues;
        private List<EventType> _eventTypes;
        private DateTime _startDate;
        private DateTime _endDate;

        public OOP_EventsManagementSystem.Model.Event SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                if (_selectedEvent != value)
                {
                    _selectedEvent = value;
                    OnPropertyChanged(nameof(SelectedEvent));
                }
            }
        }

        public int SelectedVenueId
        {
            get => _selectedVenueId;
            set
            {
                if (_selectedVenueId != value)
                {
                    _selectedVenueId = value;
                    OnPropertyChanged(nameof(SelectedVenueId));
                }
            }
        }

        public int SelectedEventTypeId
        {
            get => _selectedEventTypeId;
            set
            {
                if (_selectedEventTypeId != value)
                {
                    _selectedEventTypeId = value;
                    OnPropertyChanged(nameof(SelectedEventTypeId));
                }
            }
        }

        public List<Venue> Venues
        {
            get => _venues;
            set
            {
                if (_venues != value)
                {
                    _venues = value;
                    OnPropertyChanged(nameof(Venues));
                }
            }
        }

        public List<EventType> EventTypes
        {
            get => _eventTypes;
            set
            {
                if (_eventTypes != value)
                {
                    _eventTypes = value;
                    OnPropertyChanged(nameof(EventTypes));
                }
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        public EventDetailsVM(OOP_EventsManagementSystem.Model.Event selectedEvent, List<Venue> venues, List<EventType> eventTypes)
        {
            SelectedEvent = selectedEvent;
            SelectedVenueId = selectedEvent.VenueId;  // Set the venue to the one associated with the selected event
            SelectedEventTypeId = selectedEvent.EventTypeId;  // Set the event type to the one associated with the selected event
            Venues = venues;  // List of venues
            EventTypes = eventTypes;  // List of event types

            // Set the Start and End dates from the selected event
            StartDate = selectedEvent.StartDate.ToDateTime(TimeOnly.MinValue);
            EndDate = selectedEvent.EndDate.ToDateTime(TimeOnly.MinValue);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
