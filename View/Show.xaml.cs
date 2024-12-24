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
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Styles;
using OOP_EventsManagementSystem.ViewModel;

namespace OOP_EventsManagementSystem.View
{
    /// <summary>
    /// Interaction logic for Show.xaml
    /// </summary>
    public partial class Show : UserControl
    {
        private readonly ShowVM _viewModel;
        public Show()
        {
            InitializeComponent();
            _viewModel = new ShowVM(); // Khởi tạo ViewModel
            DataContext = _viewModel;
            _viewModel.LoadPerformers();
        }

        private void PerformerDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedPerformer = (Performer)((DataGrid)sender).SelectedItem;

            if (selectedPerformer != null)
            {
                _viewModel.SelectedPerformerId = selectedPerformer.PerformerId;
                _viewModel.LoadShowsForSelectedPerformer();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Choose.IsOpen = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem có show nào được chọn trong DataGrid không
            var selectedShow = ShowDataGrid.SelectedItem as OOP_EventsManagementSystem.Model.Show;
            if (selectedShow != null)
            {
                // Hiển thị MessageBox xác nhận xóa
                var result = MessageBox.Show($"Are you sure you want to delete {selectedShow.ShowName}?",
                                              "Confirm Delete",
                                              MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    // Gọi hàm xóa từ ViewModel
                    var performerVM = (ShowVM)DataContext;
                    performerVM.DeleteShow(selectedShow);
                }
            }
            else
            {
                MessageBox.Show("Please select a show to delete.");
            }
        }
        private bool isEditing = false; // Flag to track editing state
        private object currentSelectedItem = null; // The item currently being edited
        private bool isSelectionChangeAllowed = true; // Flag to control selection change
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShowDataGrid.SelectedItem != null) // Nếu bảng Performer, đổi thành PerformerDataGrid
            {
                currentSelectedItem = ShowDataGrid.SelectedItem; // Lưu lại hàng hiện tại đang được chọn
                foreach (var column in ShowDataGrid.Columns)
                {
                    if (column is DataGridTextColumn textColumn)
                    {
                        textColumn.IsReadOnly = false; // Cho phép chỉnh sửa cột
                    }
                }

                ShowDataGrid.IsReadOnly = false; // Kích hoạt chỉnh sửa trên bảng
                isEditing = true; // Đánh dấu trạng thái chỉnh sửa

                EditButton.IsEnabled = false; // Ẩn nút Edit
                EditButton.Visibility = Visibility.Collapsed;
                ConfirmButton.IsEnabled = true; // Hiển thị nút Confirm
                ConfirmButton.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hàng để chỉnh sửa.", "Thông báo"); // Thông báo nếu không có hàng được chọn
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentSelectedItem != null)
            {
                try
                {
                    // Ngăn thay đổi lựa chọn trong quá trình xác nhận
                    isSelectionChangeAllowed = false;

                    // Giả sử currentSelectedItem là kiểu Show
                    var showToSave = currentSelectedItem as OOP_EventsManagementSystem.Model.Show;

                    if (showToSave != null)
                    {
                        // Lưu thay đổi qua ViewModel
                        var showVM = (ShowVM)DataContext; // Lấy ViewModel hiện tại
                        showVM.SaveChanges(showToSave); // Lưu thay đổi

                        // Sau khi lưu, khóa bảng và đặt lại trạng thái UI
                        foreach (var column in ShowDataGrid.Columns)
                        {
                            if (column is DataGridTextColumn textColumn)
                            {
                                textColumn.IsReadOnly = true; // Khóa chỉnh sửa
                            }
                        }

                        ShowDataGrid.IsReadOnly = true; // Khóa toàn bộ bảng
                        isEditing = false; // Thoát khỏi chế độ chỉnh sửa
                        currentSelectedItem = null; // Xóa chọn hàng hiện tại

                        EditButton.IsEnabled = true; // Hiển thị lại nút Edit
                        EditButton.Visibility = Visibility.Visible;

                        ConfirmButton.IsEnabled = false; // Ẩn nút Confirm
                        ConfirmButton.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        MessageBox.Show("Hàng chọn không hợp lệ.", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Error");
                }
                finally
                {
                    // Kích hoạt thay đổi lựa chọn sau khi lưu xong
                    isSelectionChangeAllowed = true;
                }
            }
        }

        // Selection changed event để ngăn chọn hàng khác khi đang chỉnh sửa
        private void ShowDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isSelectionChangeAllowed || (isEditing && currentSelectedItem != null && ShowDataGrid.SelectedItem != currentSelectedItem))
            {
                ShowDataGrid.SelectedItem = currentSelectedItem; // Reset lại hàng được chọn
            }
        }

        // Beginning edit event để ngăn chỉnh sửa hàng không được chọn
        private void ShowDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (!isEditing || e.Row.Item != currentSelectedItem)
            {
                e.Cancel = true; // Hủy chỉnh sửa nếu không phải hàng hiện tại
            }
        }

        private void ConfirmAddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SponsorNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ConfirmAddExistButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
