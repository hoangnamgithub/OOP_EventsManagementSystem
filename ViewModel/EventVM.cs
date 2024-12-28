using System;
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
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Styles;
using OOP_EventsManagementSystem.Utilities;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class EventVM : INotifyPropertyChanged
    {
        public ICommand OpenEventDetailCommand { get; set; }
        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public ICommand DeleteShowCommand { get; }
        public ICommand DeleteSponsorCommand { get; }
        public ICommand DeleteEventCommand { get; }

        private readonly EventManagementDbContext _context;

        public PaginationHelper<FilteredSponsor> SponsorsPagination { get; set; }
        public PaginationHelper<Model.Event> UpcomingPagination { get; set; }
        public PaginationHelper<Model.Event> HappeningPagination { get; set; }
        public PaginationHelper<Model.Event> CompletedPagination { get; set; }

        public ObservableCollection<Model.Event> UpcomingEvents { get; set; }
        public ObservableCollection<Model.Event> HappeningEvents { get; set; }
        public ObservableCollection<Model.Event> CompletedEvents { get; set; }
        public ObservableCollection<Model.EventType> EventTypes { get; set; }
        public ObservableCollection<Model.Venue> Venues { get; set; }
        public ObservableCollection<Model.Show> Shows { get; set; }
        public ObservableCollection<FilteredSponsor> Sponsors { get; set; }
        public ObservableCollection<Model.SponsorTier> SponsorTiers { get; set; }
        public ObservableCollection<Model.Employee> Employees { get; set; }
        public ObservableCollection<Model.EmployeeRole> EmployeeRoles { get; set; }
        public ObservableCollection<Model.EquipmentName> EquipmentNames { get; set; }
        public ObservableCollection<Model.Show> PagedShows { get; set; }

        // Properties -------------------------------------
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

        private ObservableCollection<FilteredEmployeeRole> _filteredEmployeeRoles;
        public ObservableCollection<FilteredEmployeeRole> FilteredEmployeeRoles
        {
            get => _filteredEmployeeRoles;
            set
            {
                _filteredEmployeeRoles = value;
                OnPropertyChanged(nameof(FilteredEmployeeRoles));
            }
        }

        private ObservableCollection<FilteredEquipment> _filteredEquipments;
        public ObservableCollection<FilteredEquipment> FilteredEquipments
        {
            get => _filteredEquipments;
            set
            {
                _filteredEquipments = value;
                OnPropertyChanged(nameof(FilteredEquipments));
            }
        }

        private decimal _totalShowCost;
        public decimal TotalShowCost
        {
            get => _totalShowCost;
            set
            {
                if (_totalShowCost != value)
                {
                    _totalShowCost = value;
                    OnPropertyChanged(nameof(TotalShowCost));
                    UpdateServiceCost();
                    UpdateTotalCost();
                }
            }
        }

        private decimal _totalLocationCost;
        public decimal TotalLocationCost
        {
            get => _totalLocationCost;
            set
            {
                if (_totalLocationCost != value)
                {
                    _totalLocationCost = value;
                    OnPropertyChanged(nameof(TotalLocationCost));
                    UpdateServiceCost();
                    UpdateTotalCost();
                }
            }
        }

        private decimal _totalEmployeeCost;
        public decimal TotalEmployeeCost
        {
            get => _totalEmployeeCost;
            set
            {
                if (_totalEmployeeCost != value)
                {
                    _totalEmployeeCost = value;
                    OnPropertyChanged(nameof(TotalEmployeeCost));
                    UpdateServiceCost();
                    UpdateTotalCost();
                }
            }
        }

        private decimal _totalEquipmentCost;
        public decimal TotalEquipmentCost
        {
            get => _totalEquipmentCost;
            set
            {
                if (_totalEquipmentCost != value)
                {
                    _totalEquipmentCost = value;
                    OnPropertyChanged(nameof(TotalEquipmentCost));
                    UpdateServiceCost();
                    UpdateTotalCost();
                }
            }
        }

        private decimal _serviceCost;
        public decimal ServiceCost
        {
            get => _serviceCost;
            set
            {
                if (_serviceCost != value)
                {
                    _serviceCost = value;
                    OnPropertyChanged(nameof(ServiceCost));
                    UpdateTotalCost();
                }
            }
        }

        private decimal _totalCost;
        public decimal TotalCost
        {
            get => _totalCost;
            set
            {
                if (_totalCost != value)
                {
                    _totalCost = value;
                    OnPropertyChanged(nameof(TotalCost));
                }
            }
        }

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

        private string _description;
        private int _selectedEventId;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }
        public int SelectedEventId
        {
            get => _selectedEventId;
            set
            {
                if (_selectedEventId != value)
                {
                    _selectedEventId = value;
                    OnPropertyChanged(nameof(SelectedEventId));
                }
            }
        }

        private ObservableCollection<FilteredSponsor> _selectedSponsors;

        public ObservableCollection<FilteredSponsor> SelectedSponsors
        {
            get => _selectedSponsors;
            set
            {
                _selectedSponsors = value;
                OnPropertyChanged(nameof(SelectedSponsors));
            }
        }

        // Dữ liệu gốc chưa bị lọc
        private ObservableCollection<Model.Show> _allShows;
        public ObservableCollection<Model.Show> AllShows
        {
            get { return _allShows; }
            set
            {
                _allShows = value;
                OnPropertyChanged(nameof(AllShows));
            }
        }

        private ObservableCollection<FilteredSponsor> _allSponsors;
        public ObservableCollection<FilteredSponsor> AllSponsors
        {
            get { return _allSponsors; }
            set
            {
                _allSponsors = value;
                OnPropertyChanged(nameof(AllSponsors));
            }
        }

        // constructor ----------------------------------------------
        public EventVM()
        {
            _context = new EventManagementDbContext();
            OpenEventDetailCommand = new RelayCommand(ExecuteOpenEventDetailCommand);
            EditCommand = new RelayCommand(_ => ToggleEditing());
            SaveCommand = new RelayCommand(_ => SaveChanges());
            DeleteShowCommand = new RelayCommand(ExecuteDeleteShowCommand);
            DeleteSponsorCommand = new RelayCommand(ExecuteDeleteSponsorCommand);
            DeleteEventCommand = new RelayCommand(ExecuteDeleteEventCommand); // Initialize the command

            // Initialize commands
            LoadData();
            NextPageCommand = new RelayCommand(ExecuteNextPage);
            PreviousPageCommand = new RelayCommand(ExecutePreviousPage);
        }

        //--------------------------------------------------------------
        private void ExecuteDeleteEventCommand(object parameter)
        {
            // Show a confirmation message box
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete this event?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            // If the user clicks 'Yes', proceed with the deletion
            if (result == MessageBoxResult.Yes)
            {
                var eventToDelete = _context.Events.FirstOrDefault(e =>
                    e.EventId == SelectedEventId
                );

                if (eventToDelete != null)
                {
                    // Remove associated ShowSchedules
                    var showSchedules = _context
                        .ShowSchedules.Where(ss => ss.EventId == SelectedEventId)
                        .ToList();
                    _context.ShowSchedules.RemoveRange(showSchedules);

                    // Remove associated Needs
                    var needs = _context.Needs.Where(n => n.EventId == SelectedEventId).ToList();
                    _context.Needs.RemoveRange(needs);

                    // Remove associated IsSponsors
                    var isSponsors = _context
                        .IsSponsors.Where(isSponsor => isSponsor.EventId == SelectedEventId)
                        .ToList();
                    _context.IsSponsors.RemoveRange(isSponsors);

                    // Remove associated Requireds
                    var requireds = _context
                        .Requireds.Where(r => r.EventId == SelectedEventId)
                        .ToList();
                    _context.Requireds.RemoveRange(requireds);

                    // Remove the Event itself
                    _context.Events.Remove(eventToDelete);

                    // Save changes to the database
                    _context.SaveChanges();

                    // Refresh data
                    LoadData();

                    // Close the EventDetails window
                    Application
                        .Current.Windows.OfType<Window>()
                        .SingleOrDefault(w => w.IsActive)
                        ?.Close();
                }
            }
        }

        private void RecalculateAndRefreshCosts()
        {
            // Recalculate the total show cost
            var showIds = _context
                .ShowSchedules.Where(ss => ss.EventId == SelectedEventId)
                .Select(ss => ss.ShowId)
                .ToList();

            var filteredShows = _context
                .Shows.Include(s => s.Performer)
                .Include(s => s.Genre)
                .Where(s => showIds.Contains(s.ShowId))
                .ToList();

            TotalShowCost = filteredShows.Sum(s => s.Cost);

            // Recalculate the total location cost
            var selectedVenue = _context.Venues.FirstOrDefault(v => v.VenueId == SelectedVenueId);
            TotalLocationCost = selectedVenue?.Cost ?? 0;

            // Recalculate the total employee cost
            var needs = _context.Needs.Where(n => n.EventId == SelectedEventId).ToList();
            TotalEmployeeCost = needs.Sum(n => n.Quantity * n.Role.Salary);

            // Recalculate the total equipment cost
            var requiredEquipments = _context
                .Requireds.Include(r => r.EquipName)
                .Where(r => r.EventId == SelectedEventId)
                .ToList();
            TotalEquipmentCost = requiredEquipments.Sum(r =>
                r.Quantity * (r.EquipName?.EquipCost ?? 0)
            );

            // Update service cost and total cost
            UpdateServiceCost();
            UpdateTotalCost();

            // Notify property changes
            OnPropertyChanged(nameof(TotalShowCost));
            OnPropertyChanged(nameof(TotalLocationCost));
            OnPropertyChanged(nameof(TotalEmployeeCost));
            OnPropertyChanged(nameof(TotalEquipmentCost));
            OnPropertyChanged(nameof(ServiceCost));
            OnPropertyChanged(nameof(TotalCost));
        }

        private void ExecuteDeleteShowCommand(object parameter)
        {
            // Show a confirmation message box
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete the selected shows and sponsors?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            // If the user clicks 'Yes', proceed with the deletion
            if (result == MessageBoxResult.Yes)
            {
                // Handle deleting selected shows
                var showsToDelete = ShowsPagination
                    .PagedCollection.Where(show => show.IsChecked)
                    .ToList();

                foreach (var show in showsToDelete)
                {
                    // Remove associated ShowSchedules
                    var showSchedules = _context
                        .ShowSchedules.Where(ss =>
                            ss.ShowId == show.ShowId && ss.EventId == SelectedEventId
                        )
                        .ToList();
                    _context.ShowSchedules.RemoveRange(showSchedules);

                    // Remove the Show itself
                    _context.Shows.Remove(show);
                }

                // Save changes to the database
                _context.SaveChanges();

                // Reload data for shows
                LoadData();

                // Reapply filtering logic for the selected event's shows
                var showIds = _context
                    .ShowSchedules.Where(ss => ss.EventId == SelectedEventId)
                    .Select(ss => ss.ShowId)
                    .ToList();

                var filteredShows = _context
                    .Shows.Include(s => s.Performer)
                    .Include(s => s.Genre)
                    .Where(s => showIds.Contains(s.ShowId))
                    .ToList();

                ShowsPagination = new PaginationHelper<Model.Show>(filteredShows, 9); // Set the number of items per page

                // Reapply filtering logic for the selected event's sponsors
                var filteredSponsors = _context
                    .IsSponsors.Include(isSponsor => isSponsor.Sponsor) // Bao gồm dữ liệu từ bảng Sponsor
                    .Include(isSponsor => isSponsor.SponsorTier) // Bao gồm dữ liệu từ bảng SponsorTier
                    .Where(isSponsor => isSponsor.EventId == SelectedEventId)
                    .Select(isSponsor => new FilteredSponsor
                    {
                        SponsorId = isSponsor.Sponsor.SponsorId, // Lấy thông tin từ Sponsor
                        SponsorName = isSponsor.Sponsor.SponsorName, // Lấy tên của sponsor
                        TierName = isSponsor.SponsorTier.TierName, // Lấy tên của sponsor tier
                    })
                    .ToList();

                SponsorsPagination = new PaginationHelper<FilteredSponsor>(filteredSponsors, 8); // Set the number of items per page

                // Trigger UI update
                OnPropertyChanged(nameof(ShowsPagination));
            }
        }

        private void ExecuteDeleteSponsorCommand(object obj)
        {
            try
            {
                if (SelectedSponsors == null || !SelectedSponsors.Any())
                {
                    MessageBox.Show("No sponsors selected for deletion.");
                    return;
                }

                // Lấy danh sách SponsorId từ các hàng được chọn
                var sponsorsToDelete = SelectedSponsors
                    .Select(sponsor => sponsor.SponsorId)
                    .ToList();

                // Xóa các sponsor trong bảng IsSponsor liên quan đến sự kiện hiện tại
                var isSponsorRecords = _context
                    .IsSponsors.Where(isSponsor =>
                        sponsorsToDelete.Contains(isSponsor.SponsorId)
                        && isSponsor.EventId == SelectedEventId
                    )
                    .ToList();

                if (isSponsorRecords.Any())
                {
                    _context.IsSponsors.RemoveRange(isSponsorRecords);
                }

                // Lưu thay đổi vào database
                _context.SaveChanges();

                // Cập nhật lại danh sách SponsorsPagination sau khi xóa
                LoadSponsorsForEvent(SelectedEventId);

                // Hiển thị thông báo thành công
                MessageBox.Show("Selected sponsors have been successfully deleted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting sponsors: {ex.Message}");
            }
        }

        private void ToggleEditing()
        {
            // Check if the PermissionId is 3 (or any other value you want to block)
            if (UserAccount.PermissionId == 3)
            {
                // Show a message saying the user doesn't have permission
                MessageBox.Show(
                    "Bạn không có quyền để thực hiện hành động này.",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return; // Do not proceed with toggling the editing state
            }

            // If the user has permission, toggle the editing state
            IsEditing = !IsEditing;
            OnPropertyChanged(nameof(IsEditing));
        }

        private bool ValidateAttendeeCount()
        {
            var selectedVenue = _context.Venues.FirstOrDefault(v => v.VenueId == SelectedVenueId);
            if (selectedVenue != null && ExpectedAttendee > selectedVenue.Capacity)
            {
                MessageBox.Show(
                    $"The number of attendees ({ExpectedAttendee}) exceeds the venue capacity ({selectedVenue.Capacity}).",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return false;
            }
            return true;
        }

        private void UpdateServiceCost()
        {
            ServiceCost =
                0.12m
                * (TotalShowCost + TotalLocationCost + TotalEmployeeCost + TotalEquipmentCost);
        }

        private void UpdateTotalCost()
        {
            TotalCost =
                TotalShowCost
                + TotalLocationCost
                + TotalEmployeeCost
                + TotalEquipmentCost
                + ServiceCost;
        }

        private void SaveChanges()
        {
            var warnings = new StringBuilder();

            // Validate all fields before saving
            if (!ValidateFields(warnings))
            {
                MessageBox.Show(
                    warnings.ToString(),
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            if (!ValidateAttendeeCount(warnings))
            {
                MessageBox.Show(
                    warnings.ToString(),
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            if (!ValidateDates(warnings))
            {
                MessageBox.Show(
                    warnings.ToString(),
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            if (warnings.Length > 0)
            {
                MessageBox.Show(
                    warnings.ToString(),
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            // Confirm save operation
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to save the changes?",
                "Confirm Save",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                var eventToUpdate = _context.Events.FirstOrDefault(e =>
                    e.EventId == SelectedEventId
                );

                if (eventToUpdate != null)
                {
                    // Ensure ExpectedAttendee is updated properly based on venue capacity
                    var selectedVenue = _context.Venues.FirstOrDefault(v =>
                        v.VenueId == SelectedVenueId
                    );

                    if (ExpectedAttendee <= 0 && selectedVenue != null)
                    {
                        ExpectedAttendee = selectedVenue.Capacity;
                    }

                    // Update the event's details
                    eventToUpdate.EventName = EventName;
                    eventToUpdate.ExptedAttendee = ExpectedAttendee;
                    eventToUpdate.VenueId = SelectedVenueId;
                    eventToUpdate.EventTypeId = SelectedEventTypeId;
                    eventToUpdate.StartDate = DateOnly.FromDateTime(StartDate);
                    eventToUpdate.EndDate = DateOnly.FromDateTime(EndDate);
                    eventToUpdate.EventDescription = Description;

                    _context.SaveChanges();

                    // Refresh data in Event.xaml
                    LoadData();

                    // Reapply filtering logic for the selected event's shows
                    var showIds = _context
                        .ShowSchedules.Where(ss => ss.EventId == SelectedEventId)
                        .Select(ss => ss.ShowId)
                        .ToList();

                    var filteredShows = _context
                        .Shows.Include(s => s.Performer)
                        .Include(s => s.Genre)
                        .Where(s => showIds.Contains(s.ShowId))
                        .ToList();

                    ShowsPagination = new PaginationHelper<Model.Show>(filteredShows, 9); // Set the number of items per page

                    // Reapply filtering logic for the selected event's sponsors
                    var filteredSponsors = _context
                        .IsSponsors.Where(isSponsor => isSponsor.EventId == SelectedEventId)
                        .Select(isSponsor => new FilteredSponsor
                        {
                            SponsorId = isSponsor.Sponsor.SponsorId,
                            SponsorName = isSponsor.Sponsor.SponsorName,
                            TierName = isSponsor.SponsorTier.TierName,
                        })
                        .ToList();

                    SponsorsPagination = new PaginationHelper<FilteredSponsor>(filteredSponsors, 8); // Set the number of items per page

                    // Update Employees DataGrid changes
                    foreach (var employee in FilteredEmployeeRoles)
                    {
                        // Lấy thông tin từ bảng Need dựa trên RoleId và EventId
                        var need = _context.Needs.FirstOrDefault(n =>
                            n.RoleId == employee.RoleId && n.EventId == SelectedEventId
                        );

                        if (need != null)
                        {
                            // Cập nhật Quantity từ bảng Need vào EmployeeRole
                            need.Quantity = employee.Quantity;

                            _context.SaveChanges();
                        }
                    }

                    // Update Equipments DataGrid changes
                    foreach (var equipment in FilteredEquipments)
                    {
                        // Tìm đối tượng Required tương ứng với EquipNameId
                        var requiredToUpdate = _context.Requireds.FirstOrDefault(r =>
                            r.EquipNameId == equipment.EquipNameId && r.EventId == SelectedEventId
                        ); // Thêm điều kiện EventId nếu cần

                        if (requiredToUpdate != null)
                        {
                            // Cập nhật Quantity và EquipCost trong bảng Required
                            requiredToUpdate.Quantity = equipment.Quantity;

                            // Nếu bạn cũng muốn cập nhật EquipCost trong bảng Required, thêm logic ở đây
                            // requiredToUpdate.EquipCost = equipment.EquipCost;

                            // Lưu thay đổi vào cơ sở dữ liệu
                            _context.SaveChanges();
                        }
                    }

                    // Recalculate and refresh costs
                    RecalculateAndRefreshCosts();

                    // Toggle editing mode
                    ToggleEditing();
                }
            }
        }

        private bool ValidateFields(StringBuilder warnings)
        {
            if (string.IsNullOrWhiteSpace(EventName))
            {
                warnings.AppendLine("Event name cannot be left blank.");
            }

            if (StartDate == default)
            {
                warnings.AppendLine("Start date cannot be left blank.");
            }

            if (EndDate == default)
            {
                warnings.AppendLine("End date cannot be left blank.");
            }

            return warnings.Length == 0;
        }

        private bool ValidateAttendeeCount(StringBuilder warnings)
        {
            var selectedVenue = _context.Venues.FirstOrDefault(v => v.VenueId == SelectedVenueId);
            if (selectedVenue != null && ExpectedAttendee > selectedVenue.Capacity)
            {
                warnings.AppendLine(
                    $"The number of attendees ({ExpectedAttendee}) exceeds the venue capacity ({selectedVenue.Capacity})."
                );
                return false;
            }
            return true;
        }

        private bool ValidateDates(StringBuilder warnings)
        {
            if (EndDate < StartDate)
            {
                warnings.AppendLine("End date cannot be before the start date.");
                return false;
            }

            return true;
        }

        // Thực thi lệnh để mở cửa sổ EventDescription
        private void ExecuteOpenEventDetailCommand(object obj)
        {
            if (obj is Model.Event selectedEvent)
            {
                // Lấy tất cả SponsorTier từ cơ sở dữ liệu
                SponsorTiers = new ObservableCollection<Model.SponsorTier>();

                // Set properties based on the selected event
                EventName = selectedEvent.EventName;
                ExpectedAttendee = selectedEvent.ExptedAttendee;
                SelectedVenueId = selectedEvent.VenueId;
                SelectedEventTypeId = selectedEvent.EventTypeId;
                StartDate = selectedEvent.StartDate.ToDateTime(TimeOnly.MinValue); // Convert DateOnly to DateTime
                EndDate = selectedEvent.EndDate.ToDateTime(TimeOnly.MinValue); // Convert DateOnly to DateTime
                Description = selectedEvent.EventDescription;
                SelectedEventId = selectedEvent.EventId;

                // Calculate the total show cost
                var showIds = _context
                    .ShowSchedules.Where(ss => ss.EventId == selectedEvent.EventId)
                    .Select(ss => ss.ShowId)
                    .ToList();

                var filteredShows = _context
                    .Shows.Include(s => s.Performer)
                    .Include(s => s.Genre)
                    .Where(s => showIds.Contains(s.ShowId))
                    .ToList();

                TotalShowCost = filteredShows.Sum(s => s.Cost);

                // Calculate the total location cost
                var selectedVenue = _context.Venues.FirstOrDefault(v =>
                    v.VenueId == SelectedVenueId
                );
                TotalLocationCost = selectedVenue?.Cost ?? 0;

                // Calculate the total employee cost
                var needs = _context.Needs.Where(n => n.EventId == selectedEvent.EventId).ToList();
                TotalEmployeeCost = needs.Sum(n => n.Quantity * n.Role.Salary);

                ShowsPagination = new PaginationHelper<Model.Show>(filteredShows, 9); // Set the number of items per page

                // Filter equipment details for the selected event
                var filteredEquipments = _context
                    .Requireds.Where(required => required.EventId == selectedEvent.EventId)
                    .Select(required => new FilteredEquipment
                    {
                        EquipNameId = required.EquipName.EquipNameId,
                        EquipName = required.EquipName.EquipName,
                        TypeName = required.EquipName.EquipType.TypeName,
                        Quantity = required.Quantity,
                        EquipCost = required.EquipName.EquipCost,
                    })
                    .ToList();

                var requiredEquipments = _context
                    .Requireds.Include(r => r.EquipName)
                    .Where(r => r.EventId == selectedEvent.EventId)
                    .ToList();
                TotalEquipmentCost = requiredEquipments.Sum(r =>
                    r.Quantity * (r.EquipName?.EquipCost ?? 0)
                );

                FilteredEquipments = new ObservableCollection<FilteredEquipment>(
                    filteredEquipments
                );

                // Filter employee roles for the selected event
                var filteredEmployeeRoles = _context
                    .Needs.Where(need => need.EventId == selectedEvent.EventId)
                    .Select(need => new FilteredEmployeeRole
                    {
                        RoleId = need.Role.RoleId,
                        RoleName = need.Role.RoleName,
                        Quantity = need.Quantity,
                    })
                    .ToList();

                FilteredEmployeeRoles = new ObservableCollection<FilteredEmployeeRole>(
                    filteredEmployeeRoles
                );

                // Filter sponsors for the selected event
                var filteredSponsors = _context
                    .IsSponsors.Where(isSponsor => isSponsor.EventId == selectedEvent.EventId)
                    .Select(isSponsor => new FilteredSponsor
                    {
                        SponsorId = isSponsor.Sponsor.SponsorId,
                        SponsorName = isSponsor.Sponsor.SponsorName,
                        TierName = isSponsor.SponsorTier.TierName,
                    })
                    .ToList();

                SponsorsPagination = new PaginationHelper<FilteredSponsor>(filteredSponsors, 8); // Set the number of items per page

                // Open the EventDetails window
                var eventDetailsWindow = new EventDetails
                {
                    DataContext = this, // Pass the current ViewModel as DataContext
                };
                eventDetailsWindow.Show();
            }
        }

        // method -------------------------------------
        public void LoadData()
        {
            var allEvents = _context.Events.Include(e => e.Venue).ToList();
            IEnumerable<Model.Event> engagedEvents = Enumerable.Empty<Model.Event>();

            // Use UserAccount.PermissionId instead of PermissionId
            if (UserAccount.PermissionId == 3) // Only show events the employee is engaged in
            {
                var engagedEventIds = _context
                    .Engageds.Where(e => e.Account.EmployeeId == UserAccount.EmployeeId) // Filter by logged-in employee's ID
                    .Select(e => e.EventId)
                    .ToList();

                engagedEvents = allEvents.Where(e => engagedEventIds.Contains(e.EventId));
            }
            else
            {
                engagedEvents = allEvents; // Show all events if not PermissionId == 3
            }

            // Filter upcoming events
            UpcomingPagination = new PaginationHelper<Model.Event>(
                engagedEvents.Where(e => e.StartDate.ToDateTime(TimeOnly.MinValue) > DateTime.Now),
                9
            );

            // Filter happening events
            HappeningPagination = new PaginationHelper<Model.Event>(
                engagedEvents.Where(e =>
                    e.StartDate.ToDateTime(TimeOnly.MinValue) <= DateTime.Now
                    && e.EndDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Now
                ),
                9
            );

            // Filter completed events
            CompletedPagination = new PaginationHelper<Model.Event>(
                engagedEvents.Where(e => e.EndDate.ToDateTime(TimeOnly.MinValue) < DateTime.Now),
                9
            );

            // Load shows và lưu vào AllShows
            var shows = _context.Shows.Include(s => s.Performer).Include(s => s.Genre).ToList();
            ShowsPagination = new PaginationHelper<Model.Show>(shows, 9);
            AllShows = new ObservableCollection<Model.Show>(shows); // Lưu dữ liệu gốc

            // Load filtered sponsors và lưu vào AllSponsors
            var filteredSponsors = _context
                .IsSponsors.Include(isSponsor => isSponsor.Sponsor)
                .Include(isSponsor => isSponsor.SponsorTier)
                .Where(isSponsor => isSponsor.EventId == SelectedEventId)
                .Select(isSponsor => new FilteredSponsor
                {
                    SponsorId = isSponsor.Sponsor.SponsorId,
                    SponsorName = isSponsor.Sponsor.SponsorName,
                    TierName = isSponsor.SponsorTier.TierName,
                })
                .ToList();
            SponsorsPagination = new PaginationHelper<FilteredSponsor>(filteredSponsors, 9);
            AllSponsors = new ObservableCollection<FilteredSponsor>(filteredSponsors); // Lưu dữ liệu gốc

            OnPropertyChanged(nameof(UpcomingPagination));
            OnPropertyChanged(nameof(HappeningPagination));
            OnPropertyChanged(nameof(CompletedPagination));
            OnPropertyChanged(nameof(ShowsPagination));
            OnPropertyChanged(nameof(SponsorsPagination));

            UpcomingEvents = new ObservableCollection<Model.Event>(
                allEvents.Where(e => e.StartDate.ToDateTime(TimeOnly.MinValue) > DateTime.Now)
            );

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
            Shows = new ObservableCollection<Model.Show>(shows);
            Sponsors = new ObservableCollection<FilteredSponsor>(filteredSponsors);
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

        public void LoadShowsForEvent(int selectedEventId)
        {
            try
            {
                // Lọc các show cho sự kiện cụ thể
                var showIds = _context
                    .ShowSchedules.Where(ss => ss.EventId == selectedEventId)
                    .Select(ss => ss.ShowId)
                    .ToList();

                var filteredShows = _context
                    .Shows.Include(s => s.Performer)
                    .Include(s => s.Genre)
                    .Where(s => showIds.Contains(s.ShowId))
                    .ToList();

                ShowsPagination = new PaginationHelper<Model.Show>(filteredShows, 9); // Phân trang cho danh sách show

                Shows = new ObservableCollection<Model.Show>(filteredShows); // Cập nhật lại danh sách shows

                OnPropertyChanged(nameof(ShowsPagination)); // Cập nhật thông tin về ShowsPagination
                OnPropertyChanged(nameof(Shows)); // Cập nhật thông tin về Shows
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show($"Error loading shows for event: {ex.Message}");
            }
        }

        public void LoadSponsorsForEvent(int selectedEventId)
        {
            try
            {
                // Lọc các sponsorId và sponsorTierId cho sự kiện cụ thể từ bảng IsSponsor
                var sponsorsWithTierIds = _context
                    .IsSponsors.Where(isSponsor => isSponsor.EventId == selectedEventId)
                    .Select(isSponsor => new { isSponsor.SponsorId, isSponsor.SponsorTierId })
                    .ToList(); // Execute the query and bring the results into memory

                // Lọc các sponsor từ bảng Sponsor theo sponsorId đã chọn
                var filteredSponsors = _context
                    .Sponsors.Where(s =>
                        sponsorsWithTierIds.Select(x => x.SponsorId).Contains(s.SponsorId)
                    )
                    .ToList(); // Load the filtered Sponsors into memory

                // Lọc các SponsorTier từ bảng SponsorTier theo sponsorTierId đã chọn
                var sponsorTiers = _context
                    .SponsorTiers.Where(st =>
                        sponsorsWithTierIds.Select(x => x.SponsorTierId).Contains(st.SponsorTierId)
                    )
                    .ToList(); // Load the SponsorTiers into memory

                // Kết hợp thông tin từ Sponsor và SponsorTier để tạo thành FilteredSponsor
                var filteredSponsorList = filteredSponsors
                    .Join(
                        sponsorTiers,
                        sponsor =>
                            sponsorsWithTierIds
                                .FirstOrDefault(x => x.SponsorId == sponsor.SponsorId)
                                ?.SponsorTierId,
                        sponsorTier => sponsorTier.SponsorTierId,
                        (sponsor, sponsorTier) =>
                            new FilteredSponsor
                            {
                                SponsorId = sponsor.SponsorId,
                                SponsorName = sponsor.SponsorName,
                                TierName = sponsorTier.TierName, // Get TierName from SponsorTier
                                // Add any other properties you want to copy here
                            }
                    )
                    .ToList();

                // Giả sử bạn có một ObservableCollection để hiển thị danh sách sponsors
                Sponsors = new ObservableCollection<FilteredSponsor>(filteredSponsorList);

                // Phân trang cho danh sách Sponsors (nếu cần)
                SponsorsPagination = new PaginationHelper<FilteredSponsor>(filteredSponsorList, 8);

                // Cập nhật lại thông tin về Sponsors
                OnPropertyChanged(nameof(Sponsors));
                OnPropertyChanged(nameof(SponsorsPagination));
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show($"Error loading sponsors for event: {ex.Message}");
            }
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

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class FilteredEquipment
    {
        public int EquipNameId { get; set; }
        public string EquipName { get; set; }
        public string TypeName { get; set; }
        public int Quantity { get; set; }
        public decimal EquipCost { get; set; }
    }

    public class FilteredEmployeeRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int Quantity { get; set; }
    }

    public class FilteredSponsor
    {
        public int SponsorId { get; set; }
        public string SponsorName { get; set; }
        public string TierName { get; set; }
    }
}
