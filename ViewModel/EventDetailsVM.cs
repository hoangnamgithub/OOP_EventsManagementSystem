using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class EventDetailsVM
    {
        public string Name { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Weekday { get; set; }
        public string ImagePath { get; set; }

        public EventDetailsVM(string name, string day, string month, string weekday, string imagePath)
        {
            Name = name;
            Day = day;
            Month = month;
            Weekday = weekday;
            ImagePath = imagePath;
        }
    }
}
