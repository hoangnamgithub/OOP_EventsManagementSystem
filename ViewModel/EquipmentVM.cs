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

        public ObservableCollection<EquipmentViewModel> Equipments { get; set; }

        public EquipmentVM()
        {
            _context = new EventManagementDbContext();
            LoadData();
        }

        private void LoadData()
        {
            // Load and join data from the equipment, equipment_name, and equipment_type tables
            var equipmentData = from equipment in _context.Equipment
                                join equipName in _context.EquipmentNames on equipment.EquipNameId equals equipName.EquipNameId
                                join equipType in _context.EquipmentTypes on equipName.EquipTypeId equals equipType.EquipTypeId
                                select new EquipmentViewModel
                                {
                                    EquipmentId = equipment.EquipmentId,
                                    EquipmentName = equipName.EquipName,
                                    EquipmentType = equipType.TypeName
                                };

            Equipments = new ObservableCollection<EquipmentViewModel>(equipmentData.ToList());

            OnPropertyChanged(nameof(Equipments));
        }

        // Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // ViewModel for displaying equipment data
    public class EquipmentViewModel
    {
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentType { get; set; }
    }
}
