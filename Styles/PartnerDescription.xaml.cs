using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OOP_EventsManagementSystem.Styles
{
    /// <summary>
    /// Interaction logic for PartnerDescription.xaml
    /// </summary>
    public partial class PartnerDescription : Window
    {
        public int SelectedSponsorId { get; set; }
        public int? SelectedSponsorTierId { get; set; }
        public int SelectedEventId { get; set; }  // Thuộc tính lưu trữ EventId đã chọn

        public PartnerDescription(int selectedEventId)
        {
            InitializeComponent();
            SelectedEventId = selectedEventId;
            LoadDatas();
        }

        private void LoadDatas()
        {
            try
            {
                using (var context = new EventManagementDbContext())
                {
                    // Lấy danh sách Sponsor từ database
                    var sponsors = context.Sponsors.ToList();
                    sponsorComboBox.ItemsSource = sponsors;
                    sponsorComboBox.DisplayMemberPath = "SponsorName";
                    sponsorComboBox.SelectedValuePath = "SponsorId";

                    // Lấy danh sách SponsorTier từ database
                    var sponsorTiers = context.SponsorTiers.ToList();
                    tierComboBox.ItemsSource = sponsorTiers;
                    tierComboBox.DisplayMemberPath = "TierName";
                    tierComboBox.SelectedValuePath = "SponsorTierId";

                    // Khi người dùng chọn một Sponsor, cập nhật thông tin chi tiết và Tier
                    sponsorComboBox.SelectionChanged += (sender, e) =>
                    {
                        var selectedSponsor = sponsorComboBox.SelectedItem as Sponsor;
                        if (selectedSponsor != null)
                        {
                            // Cập nhật Details
                            txtDetails.Text = selectedSponsor.SponsorDetails;

                            // Cập nhật SponsorTier
                            var tier = selectedSponsor.IsSponsors
                                .FirstOrDefault()?.SponsorTier; // Assuming IsSponsor links Sponsor and Tier
                            if (tier != null)
                            {
                                tierComboBox.SelectedValue = tier.SponsorTierId;
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading sponsors and tiers: {ex.Message}");
            }
        }
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { if (e.ButtonState == MouseButtonState.Pressed) { this.DragMove(); } }
        private void btn_comfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new EventManagementDbContext())
                {
                    // Kiểm tra xem Sponsor có được chọn hay không
                    var selectedSponsorId = sponsorComboBox.SelectedValue as int?;
                    if (!selectedSponsorId.HasValue)
                    {
                        MessageBox.Show("Please select a sponsor.");
                        return; // Ngừng thực thi nếu không có sponsor được chọn
                    }

                    int sponsorId = selectedSponsorId.Value; // Lấy giá trị int từ selectedSponsorId

                    // Kiểm tra xem sự kiện đã được chọn chưa
                    var selectedEventId = SelectedEventId; // Bạn cần đảm bảo rằng SelectedEventId có giá trị hợp lệ
                    if (selectedEventId == 0)
                    {
                        MessageBox.Show("Please select an event.");
                        return; // Ngừng thực thi nếu không có sự kiện được chọn
                    }

                    // Kiểm tra xem Sponsor đã tồn tại trong Event chưa
                    var existingSponsor = context.IsSponsors
                        .FirstOrDefault(isSponsor => isSponsor.EventId == selectedEventId && isSponsor.SponsorId == sponsorId);

                    if (existingSponsor == null)
                    {
                        // Thêm Sponsor vào Event nếu chưa tồn tại
                        var selectedSponsorTierId = tierComboBox.SelectedValue as int?; // Nếu cần chọn SponsorTier

                        if (!selectedSponsorTierId.HasValue)
                        {
                            MessageBox.Show("Please select a sponsor tier.");
                            return;
                        }

                        var newIsSponsor = new IsSponsor
                        {
                            EventId = selectedEventId,
                            SponsorId = sponsorId,
                            SponsorTierId = selectedSponsorTierId.Value
                        };

                        context.IsSponsors.Add(newIsSponsor);
                        context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                        MessageBox.Show("Sponsor added successfully!");

                        // Cập nhật lại danh sách Sponsors trong DataGrid của cửa sổ EventDetails
                        var eventDetailsWindow = Application.Current.Windows.OfType<EventDetails>().FirstOrDefault();
                        if (eventDetailsWindow != null)
                        {
                            var viewModel = eventDetailsWindow.DataContext as EventVM;
                            if (viewModel != null)
                            {
                                viewModel.LoadSponsorsForEvent(selectedEventId);
                            }
                            else
                            {
                                MessageBox.Show("Event view model is not set.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Event details window not found.");
                        }

                        // Đóng cửa sổ sau khi lưu
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("This Sponsor is already added to the Event.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding Sponsor: {ex.Message}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
this.Close();
        }
    }

}

        