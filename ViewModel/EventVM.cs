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


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
