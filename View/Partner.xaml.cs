using OOP_EventsManagementSystem.Styles;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OOP_EventsManagementSystem.ViewModel;
using OOP_EventsManagementSystem.Model;
using System.Windows.Controls.Primitives;
using Microsoft.Extensions.Logging;


namespace OOP_EventsManagementSystem.View
{
    /// <summary>
    /// Interaction logic for Partner.xaml
    /// </summary>
    public partial class Partner : UserControl
    {
        private readonly EventManagementDbContext _context;
        private PartnerVM _viewModel;
        public Partner()
        {
            InitializeComponent();
            // Khởi tạo DbContext
            _context = new EventManagementDbContext();

            // Khởi tạo ViewModel
            _viewModel = new PartnerVM(_context);

            // Đặt DataContext của cửa sổ với ViewModel
            DataContext = _viewModel;
        }

        // Xử lý sự kiện chọn sự kiện từ DataGrid
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedEvent = (EventModel)((DataGrid)sender).SelectedItem;

            if (selectedEvent != null)
            {
                _viewModel.SelectedEventId = selectedEvent.EventId;
                _viewModel.LoadSponsorsForSelectedEvent();
            }
        }
        
        private void AddButton_Click(object sender, RoutedEventArgs e) 
        {
            Description.Visibility = Visibility.Visible;
        }
        private bool isEditing = false; // Cờ để theo dõi trạng thái chỉnh sửa
        private object currentSelectedItem = null; // Hàng đang chỉnh sửa

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SponsorDataGrid.SelectedItem != null)
            {
                currentSelectedItem = SponsorDataGrid.SelectedItem; // Ghi lại hàng được chọn
                foreach (var column in SponsorDataGrid.Columns)
                {
                    if (column is DataGridTextColumn textColumn)
                    {
                        textColumn.IsReadOnly = false; // Bật chỉnh sửa
                    }
                }

                SponsorDataGrid.IsReadOnly = false; // Bật chỉnh sửa
                isEditing = true; // Đặt cờ trạng thái chỉnh sửa

                EditButton.IsEnabled = false; // Vô hiệu hóa nút Edit
                ConfirmButton.IsEnabled = true; // Bật nút Confirm
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hàng để chỉnh sửa.", "Thông báo");
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentSelectedItem != null)
            {
                foreach (var column in SponsorDataGrid.Columns)
                {
                    if (column is DataGridTextColumn textColumn)
                    {
                        textColumn.IsReadOnly = true; // Khóa chỉnh sửa
                    }
                }

                SponsorDataGrid.IsReadOnly = true; // Khóa DataGrid
                isEditing = false; // Tắt trạng thái chỉnh sửa
                currentSelectedItem = null; // Xóa hàng hiện tại

                EditButton.IsEnabled = true; // Bật lại nút Edit
                ConfirmButton.IsEnabled = false; // Vô hiệu hóa nút Confirm
            }
        }

        private void SponsorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isEditing && currentSelectedItem != null && SponsorDataGrid.SelectedItem != currentSelectedItem)
            {
                // Ngăn thay đổi hàng khi đang chỉnh sửa
                MessageBox.Show("Bạn không thể chọn hàng khác khi đang chỉnh sửa.", "Thông báo");
                SponsorDataGrid.SelectedItem = currentSelectedItem; // Đặt lại hàng được chọn
            }
        }

        private void SponsorDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (!isEditing || e.Row.Item != currentSelectedItem)
            {
                e.Cancel = true; // Ngăn chỉnh sửa nếu không phải hàng đang được chỉnh sửa
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem có sponsor nào được chọn trong DataGrid không
            var selectedSponsor = SponsorDataGrid.SelectedItem as SponsorModel;
            if (selectedSponsor != null)
            {
                // Hiển thị MessageBox xác nhận xóa
                var result = MessageBox.Show($"Are you sure you want to delete {selectedSponsor.SponsorName}?",
                                              "Confirm Delete",
                                              MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    // Gọi hàm xóa từ ViewModel
                    var partnerVM = (PartnerVM)DataContext;
                    partnerVM.DeleteSponsorFromEvent(selectedSponsor);
                }
            }
            else
            {
                MessageBox.Show("Please select a sponsor to delete.");
            }
        }


    }
}
