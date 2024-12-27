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

        private void PerformerDataGrid_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
        )
        {
            var selectedPerformer = ((DataGrid)sender).SelectedItem as Performer;

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

        private void Add_Show(object sender, RoutedEventArgs e)
        {
            // Lấy hàng được chọn từ DataGrid (giả sử DataGrid của bạn có tên là dgPerformers)
            var selectedPerformer = PerformerDataGrid.SelectedItem as Performer;

            // Tạo cửa sổ ShowDescription
            var showDes = new ShowDescription();

            // Nếu có performer được chọn, truyền dữ liệu trực tiếp
            if (selectedPerformer != null)
            {
                showDes.SetPerformerDetails(
                    selectedPerformer.FullName,
                    selectedPerformer.ContactDetail
                );
            }

            // Hiển thị cửa sổ
            showDes.Show();
        }

        private void ReloadShows()
        {
            _viewModel.LoadShowsForSelectedPerformer();
        }

        private void Add_Performer(object sender, RoutedEventArgs e)
        {
            var PerformerDes = new PerformerDescription();
            PerformerDes.Show();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem có show nào được chọn trong DataGrid Show không
            var selectedShows = ShowDataGrid
                .SelectedItems.Cast<OOP_EventsManagementSystem.Model.Show>()
                .ToList();
            if (selectedShows.Any())
            {
                // Xác nhận xóa nhiều show
                var result = MessageBox.Show(
                    $"Are you sure you want to delete {selectedShows.Count} show(s)?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo
                );

                if (result == MessageBoxResult.Yes)
                {
                    var performerVM = (ShowVM)DataContext;
                    foreach (var show in selectedShows)
                    {
                        performerVM.DeleteShow(show); // Gọi hàm xóa cho từng show
                    }
                }
            }
            else
            {
                // Nếu không có show, kiểm tra Performer
                var selectedPerformers = PerformerDataGrid
                    .SelectedItems.Cast<OOP_EventsManagementSystem.Model.Performer>()
                    .ToList();
                if (selectedPerformers.Any())
                {
                    // Xác nhận xóa nhiều performer
                    var result = MessageBox.Show(
                        $"Are you sure you want to delete {selectedPerformers.Count} performer(s)?",
                        "Confirm Delete",
                        MessageBoxButton.YesNo
                    );

                    if (result == MessageBoxResult.Yes)
                    {
                        var performerVM = (ShowVM)DataContext;
                        foreach (var performer in selectedPerformers)
                        {
                            performerVM.DeletePerformer(performer); // Gọi hàm xóa cho từng performer
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select one or more shows or performers to delete.");
                }
            }
        }

        private bool isEditing = false; // Flag to track editing state
        private object currentSelectedItem = null; // The item currently being edited
        private bool isSelectionChangeAllowed = true; // Flag to control selection change

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShowDataGrid.SelectedItem != null) // Nếu bảng Show có show được chọn
            {
                currentSelectedItem = ShowDataGrid.SelectedItem;
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
            else if (PerformerDataGrid.SelectedItem != null) // Nếu bảng Performer có performer được chọn
            {
                currentSelectedItem = PerformerDataGrid.SelectedItem; // Lưu lại hàng đang được chọn
                foreach (var column in PerformerDataGrid.Columns)
                {
                    if (column is DataGridTextColumn textColumn)
                    {
                        textColumn.IsReadOnly = false; // Khóa tất cả các cột
                    }
                }

                // Chỉ cho phép chỉnh sửa trên hàng đã chọn
                PerformerDataGrid.IsReadOnly = false; // Kích hoạt chỉnh sửa trên bảng
                isEditing = true; // Đánh dấu trạng thái chỉnh sửa

                // Ẩn nút Edit, hiển thị nút Confirm
                EditButton.IsEnabled = false;
                EditButton.Visibility = Visibility.Collapsed;
                ConfirmButton.IsEnabled = true;
                ConfirmButton.Visibility = Visibility.Visible;

                // Khóa tất cả các hàng không phải hàng được chọn
                foreach (var row in PerformerDataGrid.Items)
                {
                    var rowAsDataRow = row as Performer; // Kiểm tra kiểu dữ liệu
                    if (rowAsDataRow != null && rowAsDataRow != currentSelectedItem)
                    {
                        // Dùng DataGridTemplateColumn để chỉ cho phép chỉnh sửa trên hàng được chọn
                        int rowIndex = PerformerDataGrid.Items.IndexOf(row);
                        DataGridRow dataGridRow = (DataGridRow)
                            PerformerDataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
                        if (dataGridRow != null)
                        {
                            dataGridRow.IsEnabled = false; // Vô hiệu hóa các hàng không phải hàng được chọn
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a show or performer to edit.");
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

                    // Kiểm tra xem item đang chỉnh sửa là Show hay Performer
                    if (currentSelectedItem is OOP_EventsManagementSystem.Model.Show showToSave)
                    {
                        var showVM = (ShowVM)DataContext;
                        showVM.SaveChanges(showToSave); // Lưu thay đổi cho show

                        // Sau khi lưu, khóa bảng và đặt lại trạng thái UI
                        LockEditingStateForShow();
                    }
                    else if (
                        currentSelectedItem
                        is OOP_EventsManagementSystem.Model.Performer performerToSave
                    )
                    {
                        var showVM = (ShowVM)DataContext;
                        showVM.SaveChangesForPerformer(performerToSave); // Lưu thay đổi cho performer

                        // Sau khi lưu, khóa bảng và đặt lại trạng thái UI
                        LockEditingStateForPerformer();
                    }
                    else
                    {
                        MessageBox.Show("Invalid item selected for editing.", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error");
                }
                finally
                {
                    // Kích hoạt thay đổi lựa chọn sau khi lưu xong
                    isSelectionChangeAllowed = true;
                }
            }
        }

        private void LockEditingStateForShow()
        {
            foreach (var column in ShowDataGrid.Columns)
            {
                if (column is DataGridTextColumn textColumn)
                {
                    textColumn.IsReadOnly = true; // Khóa chỉnh sửa
                }
            }

            ShowDataGrid.IsReadOnly = true; // Khóa toàn bộ bảng Show
            isEditing = false;
            currentSelectedItem = null;
            EditButton.IsEnabled = true;
            EditButton.Visibility = Visibility.Visible;
            ConfirmButton.IsEnabled = false;
            ConfirmButton.Visibility = Visibility.Collapsed;
        }

        private void LockEditingStateForPerformer()
        {
            foreach (var column in PerformerDataGrid.Columns)
            {
                if (column is DataGridTextColumn textColumn)
                {
                    textColumn.IsReadOnly = true; // Khóa chỉnh sửa
                }
            }

            PerformerDataGrid.IsReadOnly = true; // Khóa toàn bộ bảng Performer
            isEditing = false;
            currentSelectedItem = null;
            EditButton.IsEnabled = true;
            EditButton.Visibility = Visibility.Visible;
            ConfirmButton.IsEnabled = false;
            ConfirmButton.Visibility = Visibility.Collapsed;
        }

        // Selection changed event để ngăn chọn hàng khác khi đang chỉnh sửa
        private void ShowDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (
                !isSelectionChangeAllowed
                || (
                    isEditing
                    && currentSelectedItem != null
                    && ShowDataGrid.SelectedItem != currentSelectedItem
                )
            )
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

        private void ConfirmAddButton_Click(object sender, RoutedEventArgs e) { }

        private void CancelButton_Click(object sender, RoutedEventArgs e) { }

        private void SponsorNameComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e
        ) { }

        private void ConfirmAddExistButton_Click(object sender, RoutedEventArgs e) { }
    }
}
