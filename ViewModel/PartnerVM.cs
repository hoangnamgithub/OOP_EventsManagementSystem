using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;
using System.Windows;


namespace OOP_EventsManagementSystem.ViewModel
{
    public class PartnerVM : INotifyPropertyChanged
    { 
        private readonly EventManagementDbContext _context;
    
        public ObservableCollection<EventModel> Events { get; set; }
        // ObservableCollection để hiển thị dữ liệu nhà tài trợ
        public ObservableCollection<SponsorModel> Sponsors { get; set; }
        public ObservableCollection<SponsorTier> SponsorTiers { get; set; } = new ObservableCollection<SponsorTier>();

        // EventId của sự kiện được chọn
        public int SelectedEventId { get; set; }

        public PartnerVM(EventManagementDbContext context)
        {
            _context = context;
            Events = new ObservableCollection<EventModel>();
            Sponsors = new ObservableCollection<SponsorModel>();
            // Lấy dữ liệu từ cơ sở dữ liệu và chuyển thành ObservableCollection
            LoadEvents();
            LoadSponsorTiers();
            LoadSponsors();
        }

        public ObservableCollection<SponsorModel> SponsorList { get; set; } = new ObservableCollection<SponsorModel>();

        private void LoadSponsors()
        {
            // Assuming you have a context for the database
            var sponsors = _context.Sponsors.Select(s => new SponsorModel
            {
                SponsorId = s.SponsorId,
                SponsorName = s.SponsorName,
                SponsorDetails = s.SponsorDetails
            }).ToList();

            SponsorList.Clear();
            foreach (var sponsor in sponsors)
            {
                SponsorList.Add(sponsor);
            }
        }

        private int _selectedSponsorId;
        public int SelectedSponsorId
        {
            get => _selectedSponsorId;
            set
            {
                if (_selectedSponsorId != value)
                {
                    _selectedSponsorId = value;
                    UpdateSponsorDetails(); // Update details when selection changes
                    OnPropertyChanged(nameof(SelectedSponsorId));
                }
            }
        }

        private string _selectedSponsorDetails;
        public string SelectedSponsorDetails
        {
            get => _selectedSponsorDetails;
            set
            {
                _selectedSponsorDetails = value;
                OnPropertyChanged(nameof(SelectedSponsorDetails));
            }
        }

        private void UpdateSponsorDetails()
        {
            var selectedSponsor = SponsorList.FirstOrDefault(s => s.SponsorId == SelectedSponsorId);
            if (selectedSponsor != null)
            {
                SelectedSponsorDetails = selectedSponsor.SponsorDetails; // Set details
            }
        }



        // Phương thức để tải các sự kiện từ cơ sở dữ liệu
        private void LoadEvents()
        {
            Events = new ObservableCollection<EventModel>(
                _context.Events.Select(e => new EventModel
                {
                    EventId = e.EventId,
                    EventName = e.EventName
                }).ToList());
        }
        private void LoadSponsorTiers()
        {
            using (var dbContext = new EventManagementDbContext())
            {
                var tierList = dbContext.SponsorTiers.ToList();
                foreach (var tier in tierList)
                {
                    SponsorTiers.Add(tier);
                }
            }
        }
        
        public void LoadSponsorsForSelectedEvent()
        {
            var sponsors = _context.IsSponsors
                .Where(isSponsor => isSponsor.EventId == SelectedEventId)
                .Select(isSponsor => new SponsorModel
                {
                    SponsorId = isSponsor.SponsorId,
                    SponsorName = isSponsor.Sponsor.SponsorName,
                    SponsorDetails = isSponsor.Sponsor.SponsorDetails,
                    SponsorTierName = isSponsor.SponsorTier.TierName
                }).ToList();

            Sponsors.Clear();
            foreach (var sponsor in sponsors)
            {
                Sponsors.Add(sponsor);
            }
        }
        public void DeleteSponsorFromEvent(SponsorModel sponsorToDelete)
        {
            // Lấy EventId đã chọn từ UI hoặc ViewModel
            int selectedEventId = SelectedEventId; // Đây là ID của sự kiện đã chọn, bạn cần đảm bảo có giá trị này

            // Tìm và xóa sponsor khỏi event trong cơ sở dữ liệu
            var sponsorToDeleteInDb = _context.IsSponsors
                .FirstOrDefault(isSponsor => isSponsor.SponsorId == sponsorToDelete.SponsorId && isSponsor.EventId == selectedEventId);

            if (sponsorToDeleteInDb != null)
            {
                _context.IsSponsors.Remove(sponsorToDeleteInDb); // Xóa sponsor khỏi event
                _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

                // Cập nhật danh sách sponsors trong UI sau khi xóa
                LoadSponsorsForSelectedEvent(); // Hàm này sẽ tải lại danh sách sponsors cho event hiện tại
            }
            else
            {
                MessageBox.Show("Sponsor not found in the selected event.");
            }
        }

        public void SaveChanges(SponsorModel sponsorToSave)
        {
            try
            {
                // Find the corresponding IsSponsor entry in the database
                var isSponsorToUpdate = _context.IsSponsors
    .Include(isSponsor => isSponsor.Sponsor)
    .Include(isSponsor => isSponsor.SponsorTier)
    .FirstOrDefault(isSponsor => isSponsor.SponsorId == sponsorToSave.SponsorId && isSponsor.EventId == SelectedEventId);


                if (isSponsorToUpdate != null && isSponsorToUpdate.Sponsor != null)
                {
                    // Update sponsor details
                    isSponsorToUpdate.Sponsor.SponsorName = sponsorToSave.SponsorName;
                    isSponsorToUpdate.Sponsor.SponsorDetails = sponsorToSave.SponsorDetails;

                    // Ensure SponsorTier is not null before updating
                    if (isSponsorToUpdate.SponsorTier != null)
                    {
                        isSponsorToUpdate.SponsorTier.TierName = sponsorToSave.SponsorTierName;
                    }

                    // Save changes to the database
                    _context.SaveChanges();

                    // Reload sponsors after the change
                    LoadSponsorsForSelectedEvent();
                    MessageBox.Show("Changes have been saved successfully.", "Success");
                }
                else
                {
                    MessageBox.Show("Sponsor or IsSponsor entry not found in the selected event.", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving changes: {ex.Message}", "Error");
            }
        }
        public void AddNewSponsor(string sponsorName, string sponsorDetails, string sponsorTierName, int selectedEventId)
{
    try
    {
        // Validate input
        if (string.IsNullOrEmpty(sponsorName) || string.IsNullOrEmpty(sponsorDetails))
        {
            MessageBox.Show("Please fill in all fields.");
            return;
        }

        // Create a new sponsor object for the database (Entity Framework model)
        var newSponsor = new Sponsor
        {
            SponsorName = sponsorName,
            SponsorDetails = sponsorDetails,
            // Set other properties as needed (for example, tier ID can be set below)
        };

        // Add the sponsor to the database
        _context.Sponsors.Add(newSponsor);
        _context.SaveChanges();  // Save the new sponsor in the database

        // Now associate the new sponsor with the selected event
        var sponsorToAdd = new IsSponsor
        {
            EventId = selectedEventId,  // Event ID that was passed
            SponsorId = newSponsor.SponsorId,  // Sponsor ID generated after saving
            SponsorTierId = GetSponsorTierId(sponsorTierName) // Use a method to get the tier ID
        };

        _context.IsSponsors.Add(sponsorToAdd);
        _context.SaveChanges();  // Save the association with the event

        // Reload sponsors for the selected event
        LoadSponsorsForSelectedEvent();

        MessageBox.Show("Sponsor added successfully.");
    }
    catch (Exception ex)
    {
        MessageBox.Show($"An error occurred: {ex.Message}", "Error");
    }
}

        private int GetSponsorTierId(string sponsorTierName)
        {
            var tier = _context.SponsorTiers.FirstOrDefault(t => t.TierName == sponsorTierName);
            return tier?.SponsorTierId ?? 0; // Return 0 if not found
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SponsorModel : INotifyPropertyChanged
    {
        private int _sponsorId;
        private string _sponsorName;
        private string? _sponsorDetails;
        private string _sponsorTierName;

        public int SponsorId
        {
            get => _sponsorId;
            set
            {
                _sponsorId = value;
                OnPropertyChanged(nameof(SponsorId));
            }
        }

        public string SponsorName
        {
            get => _sponsorName;
            set
            {
                _sponsorName = value;
                OnPropertyChanged(nameof(SponsorName));
            }
        }

        public string? SponsorDetails
        {
            get => _sponsorDetails;
            set
            {
                _sponsorDetails = value;
                OnPropertyChanged(nameof(SponsorDetails));
            }
        }

        public string SponsorTierName
        {
            get => _sponsorTierName;
            set
            {
                _sponsorTierName = value;
                OnPropertyChanged(nameof(SponsorTierName));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EventModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
    }

}
