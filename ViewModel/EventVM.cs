using System;
using System.Collections.ObjectModel;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class EventVM
    {
        public string ImagePath { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Weekday { get; set; }
        public string Name { get; set; }

    }

    public class EventListVM
    {
        public ObservableCollection<EventVM> Events { get; set; }

        public EventListVM()
        {
            // Khởi tạo danh sách các sự kiện với tên
            Events = new ObservableCollection<EventVM>
        {
            new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "01", Month = "Dec", Weekday = "Friday", Name = "Christmas Market" },
            new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "02", Month = "Dec", Weekday = "Saturday", Name = "Art Exhibition" },
            new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "03", Month = "Dec", Weekday = "Sunday", Name = "Music Concert" },
            new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "04", Month = "Dec", Weekday = "Monday", Name = "Tech Conference" },
            new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "05", Month = "Dec", Weekday = "Tuesday", Name = "Book Fair" },
            new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "06", Month = "Dec", Weekday = "Wednesday", Name = "Food Festival" },
            new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "07", Month = "Dec", Weekday = "Thursday", Name = "Fashion Show" },
            new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "08", Month = "Dec", Weekday = "Friday", Name = "Community Meetup" },
            new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "09", Month = "Dec", Weekday = "Saturday", Name = "Charity Run" },
            new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "10", Month = "Dec", Weekday = "Sunday", Name = "Yoga Retreat" },
        };
        }
    }
}
