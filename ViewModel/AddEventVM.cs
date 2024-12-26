using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP_EventsManagementSystem.Model;

namespace OOP_EventsManagementSystem.ViewModel
{
    internal class AddEventVM : INotifyPropertyChanged
    {
        private readonly EventManagementDbContext _context;

        public AddEventVM()
        {
            _context = new EventManagementDbContext();
            LoadShows();
            LoadSponsors();
            LoadSponsorTiers();
            LoadEmployeeRoles();
            LoadEquipments();
            LoadVenues();
            LoadEventTypes();
        }

        private ObservableCollection<ShowWrapper> _shows;
        public ObservableCollection<ShowWrapper> Shows
        {
            get => _shows;
            set
            {
                _shows = value;
                OnPropertyChanged(nameof(Shows));
            }
        }

        private void LoadShows()
        {
            Shows = new ObservableCollection<ShowWrapper>(
                _context
                    .Shows.Include(s => s.Performer)
                    .Include(s => s.Genre)
                    .Select(s => new ShowWrapper
                    {
                        ShowId = s.ShowId,
                        ShowName = s.ShowName,
                        PerformerName = s.Performer.FullName,
                        GenreName = s.Genre.Genre1,
                        Cost = s.Cost,
                        IsChecked = false,
                    })
                    .ToList()
            );
        }

        private ObservableCollection<SponsorWrapper> _sponsors;
        public ObservableCollection<SponsorWrapper> Sponsors
        {
            get => _sponsors;
            set
            {
                _sponsors = value;
                OnPropertyChanged(nameof(Sponsors));
            }
        }

        private ObservableCollection<SponsorTier> _sponsorTiers;
        public ObservableCollection<SponsorTier> SponsorTiers
        {
            get => _sponsorTiers;
            set
            {
                _sponsorTiers = value;
                OnPropertyChanged(nameof(SponsorTiers));
            }
        }

        private void LoadSponsors()
        {
            Sponsors = new ObservableCollection<SponsorWrapper>(
                _context
                    .Sponsors.Select(s => new SponsorWrapper
                    {
                        SponsorId = s.SponsorId,
                        SponsorName = s.SponsorName,
                        SponsorTierId = null, // Initially no tier selected
                        IsChecked = false, // Initially unchecked
                    })
                    .ToList()
            );
        }

        private void LoadEmployeeRoles()
        {
            EmployeeRoles = new ObservableCollection<EmployeeRoleWrapper>(
                _context
                    .EmployeeRoles.Select(er => new EmployeeRoleWrapper
                    {
                        RoleName = er.RoleName,
                        Quantity = null, // Assuming Quantity is the number of employees in that role
                    })
                    .ToList()
            );
        }

        private ObservableCollection<EmployeeRoleWrapper> _employeeRoles;
        public ObservableCollection<EmployeeRoleWrapper> EmployeeRoles
        {
            get => _employeeRoles;
            set
            {
                _employeeRoles = value;
                OnPropertyChanged(nameof(EmployeeRoles));
            }
        }

        private void LoadSponsorTiers()
        {
            SponsorTiers = new ObservableCollection<SponsorTier>(_context.SponsorTiers.ToList());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private void LoadEquipments()
        {
            Equipments = new ObservableCollection<EquipmentWrapper>(
                _context
                    .EquipmentNames.Include(en => en.EquipType)
                    .Select(en => new EquipmentWrapper
                    {
                        EquipNameId = en.EquipNameId,
                        EquipName = en.EquipName,
                        TypeName = en.EquipType.TypeName,
                        EquipCost = en.EquipCost,
                        Quantity = null, // Assuming Quantity is the sum of all required quantities
                    })
                    .ToList()
            );
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

        private ObservableCollection<EventType> _eventTypes;
        public ObservableCollection<EventType> EventTypes
        {
            get => _eventTypes;
            set
            {
                _eventTypes = value;
                OnPropertyChanged(nameof(EventTypes));
            }
        }

        private void LoadVenues()
        {
            Venues = new ObservableCollection<Venue>(_context.Venues.ToList());
        }

        private void LoadEventTypes()
        {
            EventTypes = new ObservableCollection<EventType>(_context.EventTypes.ToList());
        }
    }

    public class ShowWrapper
    {
        public int ShowId { get; set; }
        public string ShowName { get; set; }
        public string PerformerName { get; set; }
        public string GenreName { get; set; }
        public decimal Cost { get; set; }
        public bool IsChecked { get; set; }
    }

    public class SponsorWrapper
    {
        public int SponsorId { get; set; }
        public string SponsorName { get; set; }
        public int? SponsorTierId { get; set; }
        public bool IsChecked { get; set; }
    }

    public class EmployeeRoleWrapper
    {
        public string RoleName { get; set; }
        public int? Quantity { get; set; }
    }

    public class EquipmentWrapper
    {
        public int EquipNameId { get; set; }
        public string EquipName { get; set; }
        public string TypeName { get; set; }
        public decimal EquipCost { get; set; }
        public int? Quantity { get; set; }
    }
}
