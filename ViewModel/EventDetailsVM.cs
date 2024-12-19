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
        private string _eventName;
        private string _expectedAttendee;
        private string _venueName;
        private string _eventDescription;
        private DateTime _startDate;
        private DateTime _endDate;

        public string EventName
        {
            get => _eventName;
            set
            {
                _eventName = value;
                OnPropertyChanged(nameof(EventName));
            }
        }

        public string ExpectedAttendee
        {
            get => _expectedAttendee;
            set
            {
                _expectedAttendee = value;
                OnPropertyChanged(nameof(ExpectedAttendee));
            }
        }

        public string VenueName
        {
            get => _venueName;
            set
            {
                _venueName = value;
                OnPropertyChanged(nameof(VenueName));
            }
        }

        public string EventDescription
        {
            get => _eventDescription;
            set
            {
                _eventDescription = value;
                OnPropertyChanged(nameof(EventDescription));
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
