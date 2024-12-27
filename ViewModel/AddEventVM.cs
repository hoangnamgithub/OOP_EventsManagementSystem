using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Utilities;

namespace OOP_EventsManagementSystem.ViewModel
{
    internal class AddEventVM : INotifyPropertyChanged
    {
        private readonly EventManagementDbContext _context;

        public AddEventVM()
        {
            _context = new EventManagementDbContext();
            ConfirmCommand = new RelayCommand<Window>(SaveData); // Update this line
            LoadShows();
            LoadSponsors();
            LoadSponsorTiers();
            LoadEmployeeRoles();
            LoadEquipments();
            LoadVenues();
            LoadEventTypes();
        }

        public ICommand ConfirmCommand { get; }

        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public int ExpectedAttendee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        private void SaveData(Window window)
        {
            if (!ValidateInput())
            {
                return;
            }

            var newEvent = new Event
            {
                EventName = EventName,
                EventDescription = EventDescription,
                ExptedAttendee = ExpectedAttendee,
                StartDate = DateOnly.FromDateTime(StartDate),
                EndDate = DateOnly.FromDateTime(EndDate),
                EventTypeId = SelectedEventTypeId,
                VenueId = SelectedVenueId,
            };

            _context.Events.Add(newEvent);
            _context.SaveChanges();

            // Save checked sponsors
            foreach (var sponsor in Sponsors.Where(s => s.IsChecked))
            {
                var isSponsor = new IsSponsor
                {
                    EventId = newEvent.EventId,
                    SponsorId = sponsor.SponsorId,
                    SponsorTierId = sponsor.SponsorTierId ?? 0,
                };
                _context.IsSponsors.Add(isSponsor);
            }

            // Save checked shows
            foreach (var show in Shows.Where(s => s.IsChecked))
            {
                var showSchedule = new ShowSchedule
                {
                    EventId = newEvent.EventId,
                    ShowId = show.ShowId,
                };
                _context.ShowSchedules.Add(showSchedule);
            }

            // Save employee roles with quantities (including 0)
            foreach (var role in EmployeeRoles)
            {
                var need = new Need
                {
                    EventId = newEvent.EventId,
                    RoleId = role.RoleId,
                    Quantity = role.Quantity,
                };
                _context.Needs.Add(need);
            }

            // Save equipment with quantities (including 0)
            foreach (var equipment in Equipments)
            {
                var required = new Required
                {
                    EventId = newEvent.EventId,
                    EquipNameId = equipment.EquipNameId,
                    Quantity = equipment.Quantity,
                };
                _context.Requireds.Add(required);
            }

            _context.SaveChanges();

            // Display success message
            MessageBox.Show(
                "Event added successfully!",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );

            // Close the window
            window?.Close();
        }

        private bool ValidateInput()
        {
            if (
                string.IsNullOrWhiteSpace(EventName)
                || string.IsNullOrWhiteSpace(EventDescription)
                || ExpectedAttendee <= 0
                || SelectedVenueId <= 0
                || SelectedEventTypeId <= 0
                || StartDate == default
                || EndDate == default
            )
            {
                MessageBox.Show(
                    "Please fill in all fields.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return false;
            }

            if (StartDate < DateTime.Today)
            {
                MessageBox.Show(
                    "Start date must be from the current date onward.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return false;
            }

            if (EndDate <= StartDate)
            {
                MessageBox.Show(
                    "End date must be after the start date.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return false;
            }

            return true;
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

        private ObservableCollection<SponsorTierWrapper> _sponsorTiers;
        public ObservableCollection<SponsorTierWrapper> SponsorTiers
        {
            get => _sponsorTiers;
            set
            {
                _sponsorTiers = value;
                OnPropertyChanged(nameof(SponsorTiers));
            }
        }

        private void LoadSponsorTiers()
        {
            SponsorTiers = new ObservableCollection<SponsorTierWrapper>(
                _context
                    .SponsorTiers.Select(st => new SponsorTierWrapper
                    {
                        SponsorTierId = st.SponsorTierId,
                        TierName = st.TierName,
                    })
                    .ToList()
            );
        }

        private void LoadSponsors()
        {
            Sponsors = new ObservableCollection<SponsorWrapper>(
                _context
                    .Sponsors.Select(s => new SponsorWrapper
                    {
                        SponsorId = s.SponsorId,
                        SponsorName = s.SponsorName,
                        SponsorTierId = null,
                        IsChecked = false,
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
                        RoleId = er.RoleId,
                        RoleName = er.RoleName,
                        EmpCost = er.Salary,
                        Quantity = 0,
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
                    .EquipmentNames.Select(e => new EquipmentWrapper
                    {
                        EquipNameId = e.EquipNameId,
                        EquipName = e.EquipName,
                        TypeName = e.EquipType.TypeName,
                        EquipCost = e.EquipCost,
                        Quantity = 0,
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

        private int _selectedEventTypeId;
        public int SelectedEventTypeId
        {
            get => _selectedEventTypeId;
            set
            {
                if (_selectedEventTypeId != value)
                {
                    _selectedEventTypeId = value;
                    OnPropertyChanged(nameof(SelectedEventTypeId));
                }
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

        private void RecalculateTotalShowCost()
        {
            TotalShowCost = Shows.Where(s => s.IsChecked).Sum(s => s.Cost);
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

        private void RecalculateTotalEmployeeCost()
        {
            TotalEmployeeCost = EmployeeRoles.Sum(er => er.Quantity * er.EmpCost);
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

        private void RecalculateTotalEquipmentCost()
        {
            TotalEquipmentCost = Equipments.Sum(eq => eq.Quantity * eq.EquipCost);
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

        public class RelayCommand<T> : ICommand
        {
            private readonly Action<T> _execute;
            private readonly Func<bool> _canExecute;

            public RelayCommand(Action<T> execute, Func<bool> canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

            public void Execute(object parameter) => _execute((T)parameter);

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
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

    public class SponsorWrapper : INotifyPropertyChanged
    {
        private bool _isChecked;
        private int? _sponsorTierId;

        public int SponsorId { get; set; }
        public string SponsorName { get; set; }
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

        public int? SponsorTierId
        {
            get => _sponsorTierId;
            set
            {
                if (_sponsorTierId != value)
                {
                    _sponsorTierId = value;
                    OnPropertyChanged(nameof(SponsorTierId));
                }
            }
        }

        public string TierName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EmployeeRoleWrapper : INotifyPropertyChanged
    {
        private int _quantity;

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public decimal EmpCost { get; set; }
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

    public class SponsorTierWrapper : INotifyPropertyChanged
    {
        private int _sponsorTierId;
        private string _tierName;

        public int SponsorTierId
        {
            get => _sponsorTierId;
            set
            {
                if (_sponsorTierId != value)
                {
                    _sponsorTierId = value;
                    OnPropertyChanged(nameof(SponsorTierId));
                }
            }
        }

        public string TierName
        {
            get => _tierName;
            set
            {
                if (_tierName != value)
                {
                    _tierName = value;
                    OnPropertyChanged(nameof(TierName));
                }
            }
        }

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
