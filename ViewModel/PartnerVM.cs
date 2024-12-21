using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;
using System.Data.Entity;
using System.Windows;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class PartnerVM : INotifyPropertyChanged
    { 
        private readonly EventManagementDbContext _context;
    
        public ObservableCollection<EventModel> Events { get; set; }
        // ObservableCollection để hiển thị dữ liệu nhà tài trợ
        public ObservableCollection<SponsorModel> Sponsors { get; set; }

        // EventId của sự kiện được chọn
        public int SelectedEventId { get; set; }

        public PartnerVM(EventManagementDbContext context)
        {
            _context = context;
            Events = new ObservableCollection<EventModel>();
            Sponsors = new ObservableCollection<SponsorModel>();
            // Lấy dữ liệu từ cơ sở dữ liệu và chuyển thành ObservableCollection
            LoadEvents();
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SponsorModel
    {
        public int SponsorId { get; set; }
        public string SponsorName { get; set; }
        public string? SponsorDetails { get; set; }
        public string SponsorTierName { get; set; }       
        // Thêm thuộc tính liên kết với SponsorTier
        public int SponsorTierId { get; set; }
        public virtual SponsorTier SponsorTier { get; set; } = null!;
    }

    public class EventModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
    }

}
