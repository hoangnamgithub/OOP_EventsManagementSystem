using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OOP_EventsManagementSystem.Model;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class EquipmentVM : INotifyPropertyChanged
    {
        private readonly EventManagementDbContext _context;

        public EquipmentVM()
        {
            _context = new EventManagementDbContext();
            LoadEvents();
        }

        private ObservableCollection<Event> _events;
        public ObservableCollection<Event> Events
        {
            get => _events;
            set
            {
                _events = value;
                OnPropertyChanged(nameof(Events));
            }
        }

        private ObservableCollection<EquipmentWrapper> _equipments;
        public ObservableCollection<EquipmentWrapper> Equipments
        {
            get => _equipments;
            set
            {
                _equipments = value;
                OnPropertyChanged(nameof(Equipments));
            }
        }

        private Event _selectedEvent;
        public Event SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                OnPropertyChanged(nameof(SelectedEvent));
                LoadEquipmentsForSelectedEvent();
            }
        }

        private void LoadEvents()
        {
            Events = new ObservableCollection<Event>(_context.Events.ToList());
        }

        private void LoadEquipmentsForSelectedEvent()
        {
            if (SelectedEvent == null)
                return;

            Equipments = new ObservableCollection<EquipmentWrapper>(
                _context
                    .Requireds.Where(r => r.EventId == SelectedEvent.EventId)
                    .Select(r => new EquipmentWrapper
                    {
                        EquipNameId = r.EquipName.EquipNameId,
                        EquipName = r.EquipName.EquipName,
                        Quantity = r.Quantity,
                    })
                    .ToList()
            );
        }

        // Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
