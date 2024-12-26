﻿using System;
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
                RecalculateTotalShowCost();
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
                        IsChecked = false, // Initial state
                    })
                    .ToList()
            );

            foreach (var show in Shows)
            {
                show.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(show.IsChecked))
                    {
                        RecalculateTotalShowCost();
                    }
                };
            }
        }

        private void RecalculateTotalShowCost()
        {
            TotalShowCost = Shows.Where(s => s.IsChecked).Sum(s => s.Cost);
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
                        Quantity = 0, // Initial quantity
                        EmpCost = er.Salary, // Assuming EmpCost is the salary for that role
                    })
                    .ToList()
            );

            foreach (var role in EmployeeRoles)
            {
                role.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(role.Quantity))
                    {
                        RecalculateTotalEmployeeCost();
                    }
                };
            }
        }

        private void RecalculateTotalEmployeeCost()
        {
            TotalEmployeeCost = EmployeeRoles.Sum(er => er.Quantity * er.EmpCost);
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

        private decimal _totalEquipmentCost;
        public decimal TotalEquipmentCost
        {
            get => _totalEquipmentCost;
            set
            {
                _totalEquipmentCost = value;
                OnPropertyChanged(nameof(TotalEquipmentCost));
                RecalculateServiceCost();
                RecalculateTotalCost();
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
                        Quantity = 0, // Initial quantity
                    })
                    .ToList()
            );

            foreach (var equipment in Equipments)
            {
                equipment.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(equipment.Quantity))
                    {
                        RecalculateTotalEquipmentCost();
                    }
                };
            }
        }

        private void RecalculateTotalEquipmentCost()
        {
            TotalEquipmentCost = Equipments.Sum(eq => eq.Quantity * eq.EquipCost);
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

        private decimal _totalLocationCost;
        public decimal TotalLocationCost
        {
            get => _totalLocationCost;
            set
            {
                _totalLocationCost = value;
                OnPropertyChanged(nameof(TotalLocationCost));
                RecalculateServiceCost();
                RecalculateTotalCost();
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

        private int _venueCapacity;
        public int VenueCapacity
        {
            get => _venueCapacity;
            set
            {
                _venueCapacity = value;
                OnPropertyChanged(nameof(VenueCapacity));
            }
        }

        private int _selectedVenueId;
        public int SelectedVenueId
        {
            get => _selectedVenueId;
            set
            {
                if (_selectedVenueId != value)
                {
                    _selectedVenueId = value;
                    OnPropertyChanged(nameof(SelectedVenueId));
                    UpdateTotalLocationCost();
                }
            }
        }

        private void UpdateTotalLocationCost()
        {
            var selectedVenue = Venues.FirstOrDefault(v => v.VenueId == SelectedVenueId);
            TotalLocationCost = selectedVenue?.Cost ?? 0;
            VenueCapacity = selectedVenue?.Capacity ?? 0;
        }

        private void LoadVenues()
        {
            Venues = new ObservableCollection<Venue>(_context.Venues.ToList());
        }

        private void LoadEventTypes()
        {
            EventTypes = new ObservableCollection<EventType>(_context.EventTypes.ToList());
        }

        private decimal _totalShowCost;
        public decimal TotalShowCost
        {
            get => _totalShowCost;
            set
            {
                _totalShowCost = value;
                OnPropertyChanged(nameof(TotalShowCost));
                RecalculateServiceCost();
                RecalculateTotalCost();
            }
        }

        private decimal _totalEmployeeCost;
        public decimal TotalEmployeeCost
        {
            get => _totalEmployeeCost;
            set
            {
                _totalEmployeeCost = value;
                OnPropertyChanged(nameof(TotalEmployeeCost));
                RecalculateServiceCost();
                RecalculateTotalCost();
            }
        }

        private decimal _serviceCost;
        public decimal ServiceCost
        {
            get => _serviceCost;
            set
            {
                _serviceCost = value;
                OnPropertyChanged(nameof(ServiceCost));
            }
        }

        private void RecalculateServiceCost()
        {
            var totalCost =
                TotalEmployeeCost + TotalEquipmentCost + TotalLocationCost + TotalShowCost;
            ServiceCost = totalCost * 0.15m;
        }

        private decimal _totalCost;
        public decimal TotalCost
        {
            get => _totalCost;
            set
            {
                _totalCost = value;
                OnPropertyChanged(nameof(TotalCost));
            }
        }

        private void RecalculateTotalCost()
        {
            TotalCost =
                TotalEmployeeCost
                + TotalEquipmentCost
                + TotalLocationCost
                + TotalShowCost
                + ServiceCost;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    public class ShowWrapper : INotifyPropertyChanged
    {
        private bool _isChecked;

        public int ShowId { get; set; }
        public string ShowName { get; set; }
        public string PerformerName { get; set; }
        public string GenreName { get; set; }
        public decimal Cost { get; set; }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SponsorWrapper
    {
        public int SponsorId { get; set; }
        public string SponsorName { get; set; }
        public int? SponsorTierId { get; set; }
        public bool IsChecked { get; set; }
    }

    public class EmployeeRoleWrapper : INotifyPropertyChanged
    {
        private int _quantity;

        public string RoleName { get; set; }
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }
        public decimal EmpCost { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EquipmentWrapper : INotifyPropertyChanged
    {
        private int _quantity;

        public int EquipNameId { get; set; }
        public string EquipName { get; set; }
        public string TypeName { get; set; }
        public decimal EquipCost { get; set; }
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
