using Microsoft.EntityFrameworkCore;
using OOP_EventsManagementSystem.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class EquipmentVM : INotifyPropertyChanged
    {
        private readonly EventManagementDbContext _context;

        public EquipmentVM()
        {
            _context = new EventManagementDbContext();

        }

        // Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

