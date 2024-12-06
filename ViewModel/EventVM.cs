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
    }

    public class EventListVM
    {
        public ObservableCollection<EventVM> Events { get; set; }

        public EventListVM()
        {
            // Khởi tạo danh sách các sự kiện
            Events = new ObservableCollection<EventVM>
            {
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "01", Month = "Dec", Weekday = "Friday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "02", Month = "Dec", Weekday = "Saturday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "03", Month = "Dec", Weekday = "Sunday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "01", Month = "Dec", Weekday = "Friday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "02", Month = "Dec", Weekday = "Saturday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "03", Month = "Dec", Weekday = "Sunday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "01", Month = "Dec", Weekday = "Friday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "02", Month = "Dec", Weekday = "Saturday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "03", Month = "Dec", Weekday = "Sunday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "01", Month = "Dec", Weekday = "Friday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "02", Month = "Dec", Weekday = "Saturday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "03", Month = "Dec", Weekday = "Sunday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "01", Month = "Dec", Weekday = "Friday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "02", Month = "Dec", Weekday = "Saturday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "03", Month = "Dec", Weekday = "Sunday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "01", Month = "Dec", Weekday = "Friday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "02", Month = "Dec", Weekday = "Saturday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "03", Month = "Dec", Weekday = "Sunday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "01", Month = "Dec", Weekday = "Friday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "02", Month = "Dec", Weekday = "Saturday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "03", Month = "Dec", Weekday = "Sunday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "01", Month = "Dec", Weekday = "Friday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "02", Month = "Dec", Weekday = "Saturday" },
                new EventVM { ImagePath = "pack://application:,,,/Resources/Images/event2.jpg", Day = "03", Month = "Dec", Weekday = "Sunday" }

            };
        }
    }
}
