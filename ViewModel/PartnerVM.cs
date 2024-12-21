using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class PartnerVM : INotifyPropertyChanged
    {
        public ICommand ScrollChangedCommand { get; }
        private ObservableCollection<SponsorDisplayModel> _sponsors;
        public ObservableCollection<SponsorDisplayModel> Sponsors
        {
            get => _sponsors;
            set
            {
                _sponsors = value;
                OnPropertyChanged(nameof(Sponsors));
            }
        }

        private bool _isLoading = false; // Biến để kiểm tra đang tải hay không

        public PartnerVM()
        {
            ScrollChangedCommand = new RelayCommand(OnScrollChanged);
            // Tải dữ liệu ban đầu
            LoadSponsorsAsync();
        }

        // Phương thức tải dữ liệu ban đầu
        private async Task LoadSponsorsAsync(int skip = 0, int take = 20)
        {
            if (_isLoading) return; // Nếu đang tải dữ liệu thì không tải lại

            _isLoading = true;
            using (var context = new EventManagementDbContext())
            {
                // Tải dữ liệu từ cơ sở dữ liệu
                var sponsors = await context.IsSponsors
                                             .Skip(skip)
                                             .Take(take)
                                             .Select(isSponsor => new SponsorDisplayModel
                                             {
                                                 SponsorName = isSponsor.Sponsor.SponsorName,
                                                 SponsorDetails = isSponsor.Sponsor.SponsorDetails,
                                                 SponsorTierName = isSponsor.SponsorTier.TierName,
                                                 EventName = isSponsor.Event.EventName
                                             })
                                             .ToListAsync();

                // Cập nhật danh sách sponsor
                foreach (var sponsor in sponsors)
                {
                    Sponsors.Add(sponsor); // Thêm dữ liệu vào ObservableCollection
                }
            }
            _isLoading = false;
        }

        // Phương thức kiểm tra sự kiện cuộn
        private void OnScrollChanged(object parameter)
        {
            var scrollViewer = parameter as ScrollViewer;
            if (scrollViewer != null && scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
            {
                LoadSponsorsAsync(Sponsors.Count, 20); // Tải thêm dữ liệu khi cuộn đến cuối
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SponsorDisplayModel
    {
        public string SponsorName { get; set; } = string.Empty;
        public string? SponsorDetails { get; set; }
        public string SponsorTierName { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
    }
}
