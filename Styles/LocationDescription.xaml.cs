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
    /// Interaction logic for LocationDescription.xaml
    /// </summary>
    public partial class LocationDescription : Window
    {
        public int VenueId { get; set; }
        public string VenueName { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
        public decimal Cost { get; set; }
        private VenueViewModel VenueViewModel { get; set; }
        private Action ReloadAction { get; set; } // Delegate để gọi hàm reload từ cửa sổ cha

        public LocationDescription(VenueViewModel venueViewModel, Action reloadAction)
        {
            InitializeComponent();
            VenueViewModel = venueViewModel;
            ReloadAction = reloadAction;
            this.DataContext = VenueViewModel; // Gán DataContext là VenueViewModel
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) { this.DragMove(); }
        }

        private void btn_comfirm_Click(object sender, RoutedEventArgs e)
        {
            SaveChangesToDatabase();
        }


        private void SaveChangesToDatabase()
        {
            using (var context = new EventManagementDbContext())
            {
                // Tìm venue trong cơ sở dữ liệu theo VenueId
                var venue = context.Venues.FirstOrDefault(v => v.VenueId == VenueViewModel.VenueId);

                if (venue == null)
                {
                    // Nếu không tìm thấy venue, thêm mới
                    venue = new Venue
                    {
                        VenueName = VenueViewModel.VenueName,
                        Address = VenueViewModel.Address,
                        Cost = VenueViewModel.Cost,
                        Capacity = VenueViewModel.Capacity
                    };

                    context.Venues.Add(venue);
                }
                else
                {
                    // Cập nhật venue hiện tại
                    venue.VenueName = VenueViewModel.VenueName;
                    venue.Address = VenueViewModel.Address;
                    venue.Cost = VenueViewModel.Cost;
                    venue.Capacity = VenueViewModel.Capacity;
                }

                // Lưu thay đổi
                context.SaveChanges();

                // Hiển thị thông báo chính xác
                MessageBox.Show(
                    venue.VenueId > 0 ? "Thông tin đã được cập nhật thành công." : "Venue mới đã được thêm thành công.",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // Gọi hành động reload từ cửa sổ cha
                ReloadAction?.Invoke();

                // Đóng cửa sổ
                this.Close();
            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa địa điểm này?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                DeleteVenueFromDatabase();
            }
        }

        private void DeleteVenueFromDatabase()
        {
            using (var context = new EventManagementDbContext())
            {
                // Tìm venue trong cơ sở dữ liệu theo VenueId
                var venue = context.Venues.FirstOrDefault(v => v.VenueId == VenueViewModel.VenueId);

                if (venue != null)
                {
                    // Xóa venue khỏi database
                    context.Venues.Remove(venue);
                    context.SaveChanges();

                    // Xóa venue khỏi ObservableCollection
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var locationVM = Application.Current.MainWindow.DataContext as LocationVM;
                        var venueToRemove = locationVM?.Venues.FirstOrDefault(v => v.VenueId == VenueViewModel.VenueId);
                        if (venueToRemove != null)
                        {
                            locationVM.Venues.Remove(venueToRemove);
                        }
                    });

                    MessageBox.Show(
                        "Địa điểm đã được xóa thành công.",
                        "Thành công",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                    // Đóng cửa sổ sau khi xóa
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Không tìm thấy địa điểm trong cơ sở dữ liệu.",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

    }

}
