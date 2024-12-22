﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Styles;
using OOP_EventsManagementSystem.Utilities;
using OOP_EventsManagementSystem.View;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class EventVM : INotifyPropertyChanged
    {
        public ICommand OpenEventDetailCommand { get; set; }
        public ICommand ConfirmCommand { get; }
        public ICommand EditCommand { get; }

        public PaginationHelper<Model.Event> UpcomingPagination { get; set; }
        public PaginationHelper<Model.Event> HappeningPagination { get; set; }
        public PaginationHelper<Model.Event> CompletedPagination { get; set; }
        public PaginationHelper<dynamic> SponsorsPagination { get; set; }

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        private readonly EventManagementDbContext _context;

        public ObservableCollection<Model.Event> UpcomingEvents { get; set; }
        public ObservableCollection<Model.Event> HappeningEvents { get; set; }
        public ObservableCollection<Model.Event> CompletedEvents { get; set; }
        public ObservableCollection<Model.EventType> EventTypes { get; set; }
        public ObservableCollection<Model.Venue> Venues { get; set; }
        public ObservableCollection<Model.Show> Shows { get; set; }
        public ObservableCollection<Model.Sponsor> Sponsors { get; set; }
        public ObservableCollection<Model.Employee> Employees { get; set; }

        public ObservableCollection<Model.EmployeeRole> EmployeeRoles { get; set; }
        public ObservableCollection<Model.EquipmentName> EquipmentNames { get; set; }

        public ObservableCollection<Model.Show> PagedShows { get; set; }

        private ObservableCollection<Model.Show> _filteredShows;
        public ObservableCollection<Model.Show> FilteredShows
        {
            get => _filteredShows;
            set
            {
                _filteredShows = value;
                OnPropertyChanged(nameof(FilteredShows));
            }
        }

        private PaginationHelper<Model.Show> _showsPagination;
        public PaginationHelper<Model.Show> ShowsPagination
        {
            get => _showsPagination;
            set
            {
                _showsPagination = value;
                OnPropertyChanged(nameof(ShowsPagination));
            }
        }

        private ObservableCollection<object> _filteredEmployeeRoles;
        public ObservableCollection<object> FilteredEmployeeRoles
        {
            get => _filteredEmployeeRoles;
            set
            {
                _filteredEmployeeRoles = value;
                OnPropertyChanged(nameof(FilteredEmployeeRoles));
            }
        }

        private ObservableCollection<object> _filteredEquipments;
        public ObservableCollection<object> FilteredEquipments
        {
            get => _filteredEquipments;
            set
            {
                _filteredEquipments = value;
                OnPropertyChanged(nameof(FilteredEquipments));
            }
        }

        // properties -------------------------------------
        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (_isEditing != value)
                {
                    _isEditing = value;
                    OnPropertyChanged(nameof(IsEditing));
                }
            }
        }

        private string _eventName;
        public string EventName
        {
            get => _eventName;
            set
            {
                if (_eventName != value)
                {
                    _eventName = value;
                    OnPropertyChanged(nameof(EventName));
                }
            }
        }

        private int _expectedAttendee;
        public int ExpectedAttendee
        {
            get => _expectedAttendee;
            set
            {
                if (_expectedAttendee != value)
                {
                    _expectedAttendee = value;
                    OnPropertyChanged(nameof(ExpectedAttendee));
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
                }
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

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        // Property for binding TextBox Text
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description)); // Notify when the property changes
                }
            }
        }

        // constructor -------------------------------------
        public EventVM()
        {
            OpenEventDetailCommand = new RelayCommand(ExecuteOpenEventDetailCommand);

            _context = new EventManagementDbContext();
            EditCommand = new RelayCommand(_ => ToggleEditing());

            // Initialize commands
            LoadData();
            NextPageCommand = new RelayCommand(ExecuteNextPage);
            PreviousPageCommand = new RelayCommand(ExecutePreviousPage);
        }

        private void ToggleEditing()
        {
            IsEditing = !IsEditing;
        }

        private void ExecuteConfirmCommand(object parameter)
        {
            // Thực hiện logic xác nhận tại đây
        }

        // Thực thi lệnh để mở cửa sổ EventDescription
        private void ExecuteOpenEventDetailCommand(object obj)
        {
            if (obj is Model.Event selectedEvent)
            {
                // Set properties based on the selected event
                EventName = selectedEvent.EventName;
                ExpectedAttendee = selectedEvent.ExptedAttendee;
                SelectedVenueId = selectedEvent.VenueId;
                SelectedEventTypeId = selectedEvent.EventTypeId;
                StartDate = selectedEvent.StartDate.ToDateTime(TimeOnly.MinValue); // Convert DateOnly to DateTime
                EndDate = selectedEvent.EndDate.ToDateTime(TimeOnly.MinValue); // Convert DateOnly to DateTime
                Description = selectedEvent.EventDescription;

                // Filter equipment details for the selected event
                var filteredEquipments = _context
                    .Requireds.Where(required => required.EventId == selectedEvent.EventId)
                    .Select(required => new
                    {
                        EquipNameId = required.EquipName.EquipNameId,
                        EquipName = required.EquipName.EquipName,
                        TypeName = required.EquipName.EquipType.TypeName,
                        Quantity = required.Quantity,
                        EquipCost = required.EquipName.EquipCost,
                    })
                    .ToList();

                FilteredEquipments = new ObservableCollection<object>(filteredEquipments);

                // Filter employee roles for the selected event
                var filteredEmployeeRoles = _context
                    .Needs.Where(need => need.EventId == selectedEvent.EventId)
                    .Select(need => new
                    {
                        RoleId = need.Role.RoleId,
                        RoleName = need.Role.RoleName,
                        Quantity = need.Quantity,
                    })
                    .ToList();

                FilteredEmployeeRoles = new ObservableCollection<object>(filteredEmployeeRoles);

                // Filter sponsors for the selected event
                var filteredSponsors = _context
                    .IsSponsors.Where(isSponsor => isSponsor.EventId == selectedEvent.EventId)
                    .Select(isSponsor => new
                    {
                        SponsorId = isSponsor.Sponsor.SponsorId,
                        SponsorName = isSponsor.Sponsor.SponsorName,
                        TierName = isSponsor.SponsorTier.TierName,
                    })
                    .ToList();

                SponsorsPagination = new PaginationHelper<object>(filteredSponsors, 9); // Set the number of items per page

                // Filter shows for the selected event
                var showIds = _context
                    .ShowSchedules.Where(ss => ss.EventId == selectedEvent.EventId)
                    .Select(ss => ss.ShowId)
                    .ToList();

                var filteredShows = _context
                    .Shows.Include(s => s.Performer)
                    .Include(s => s.Genre)
                    .Where(s => showIds.Contains(s.ShowId))
                    .ToList();

                ShowsPagination = new PaginationHelper<Model.Show>(filteredShows, 9); // Set the number of items per page

                // Open the EventDetails window
                var eventDetailsWindow = new EventDetails
                {
                    DataContext = this, // Pass the current ViewModel as DataContext
                };
                eventDetailsWindow.Show();
            }
        }

        // method -------------------------------------
        private void LoadData()
        {
            var allEvents = _context.Events.Include(e => e.Venue).ToList();

            UpcomingPagination = new PaginationHelper<Model.Event>(
                allEvents.Where(e => e.StartDate.ToDateTime(TimeOnly.MinValue) > DateTime.Now),
                9
            );

            HappeningPagination = new PaginationHelper<Model.Event>(
                allEvents.Where(e =>
                    e.StartDate.ToDateTime(TimeOnly.MinValue) <= DateTime.Now
                    && e.EndDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Now
                ),
                9
            );

            CompletedPagination = new PaginationHelper<Model.Event>(
                allEvents.Where(e => e.EndDate.ToDateTime(TimeOnly.MinValue) < DateTime.Now),
                9
            );

            ShowsPagination = new PaginationHelper<Model.Show>(
                _context.Shows.Include(s => s.Performer).Include(s => s.Genre).ToList(),
                9
            );
            SponsorsPagination = new PaginationHelper<dynamic>(_context.Sponsors.ToList(), 9);

            OnPropertyChanged(nameof(UpcomingPagination));
            OnPropertyChanged(nameof(HappeningPagination));
            OnPropertyChanged(nameof(CompletedPagination));
            OnPropertyChanged(nameof(ShowsPagination));
            OnPropertyChanged(nameof(SponsorsPagination));

            UpcomingEvents = new ObservableCollection<Model.Event>(
                allEvents.Where(e => e.StartDate.ToDateTime(TimeOnly.MinValue) > DateTime.Now)
            );

            // Các dữ liệu khác
            HappeningEvents = new ObservableCollection<Model.Event>(
                allEvents.Where(e =>
                    e.StartDate.ToDateTime(TimeOnly.MinValue) <= DateTime.Now
                    && e.EndDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Now
                )
            );
            CompletedEvents = new ObservableCollection<Model.Event>(
                allEvents.Where(e => e.EndDate.ToDateTime(TimeOnly.MinValue) < DateTime.Now)
            );
            EventTypes = new ObservableCollection<Model.EventType>(_context.EventTypes.ToList());
            Venues = new ObservableCollection<Model.Venue>(_context.Venues.ToList());
            Shows = new ObservableCollection<Model.Show>(
                _context.Shows.Include(s => s.Performer).Include(s => s.Genre).ToList()
            );
            Sponsors = new ObservableCollection<Sponsor>(_context.Sponsors.ToList());
            Employees = new ObservableCollection<Model.Employee>(
                _context.Employees.Include(e => e.Role).ToList()
            );
            EmployeeRoles = new ObservableCollection<Model.EmployeeRole>(
                _context.EmployeeRoles.ToList()
            );
            EquipmentNames = new ObservableCollection<Model.EquipmentName>(
                _context.EquipmentNames.ToList()
            );

            OnPropertyChanged(nameof(UpcomingEvents));
            OnPropertyChanged(nameof(HappeningEvents));
            OnPropertyChanged(nameof(CompletedEvents));
            OnPropertyChanged(nameof(EventTypes));
            OnPropertyChanged(nameof(Venues));
            OnPropertyChanged(nameof(Shows));
            OnPropertyChanged(nameof(Sponsors));
            OnPropertyChanged(nameof(Employees));
            OnPropertyChanged(nameof(EmployeeRoles));
            OnPropertyChanged(nameof(EquipmentNames));
        }

        private void ExecuteNextPage(object parameter)
        {
            if (parameter?.ToString() == "Upcoming")
                UpcomingPagination.NextPage();
            if (parameter?.ToString() == "Happening")
                HappeningPagination.NextPage();
            if (parameter?.ToString() == "Completed")
                CompletedPagination.NextPage();
            if (parameter?.ToString() == "Shows")
                ShowsPagination.NextPage();
            if (parameter?.ToString() == "Sponsors")
                SponsorsPagination.NextPage();
        }

        private void ExecutePreviousPage(object parameter)
        {
            if (parameter?.ToString() == "Upcoming")
                UpcomingPagination.PreviousPage();
            if (parameter?.ToString() == "Happening")
                HappeningPagination.PreviousPage();
            if (parameter?.ToString() == "Completed")
                CompletedPagination.PreviousPage();
            if (parameter?.ToString() == "Shows")
                ShowsPagination.PreviousPage();
            if (parameter?.ToString() == "Sponsors")
                SponsorsPagination.PreviousPage();
        }

        // Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
