using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OOP_EventsManagementSystem.ViewModel;
using OOP_EventsManagementSystem.ViewModel;

namespace OOP_EventsManagementSystem.Styles
{
    /// <summary>
    /// Interaction logic for EventDetails.xaml
    /// </summary>
    public partial class EventDetails : Window
    {
        public EventDetails()
        {
            InitializeComponent();
            DataContext = new EventVM();
        }

        private void txtbox_expectedAttendee_PreviewTextInput(
            object sender,
            TextCompositionEventArgs e
        )
        {
            e.Handled = !IsTextNumeric(e.Text);
        }

        private void dtpckr_startDate_SelectedDateChanged(
            object sender,
            SelectionChangedEventArgs e
        )
        {
            if (dtpckr_startDate.SelectedDate.HasValue)
            {
                DateTime startDate = dtpckr_startDate.SelectedDate.Value;
                if (
                    dtpckr_endDate.SelectedDate.HasValue
                    && dtpckr_endDate.SelectedDate.Value < startDate
                )
                {
                    MessageBox.Show(
                        "Start date cannot be after the end date.",
                        "Validation Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    dtpckr_startDate.SelectedDate = dtpckr_endDate.SelectedDate.Value.AddDays(-1);
                }
                else if (
                    !dtpckr_endDate.SelectedDate.HasValue
                    || dtpckr_endDate.SelectedDate.Value < startDate
                )
                {
                    dtpckr_endDate.SelectedDate = startDate.AddDays(1);
                }
            }
        }

        private void dtpckr_endDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtpckr_startDate.SelectedDate.HasValue && dtpckr_endDate.SelectedDate.HasValue)
            {
                DateTime startDate = dtpckr_startDate.SelectedDate.Value;
                DateTime endDate = dtpckr_endDate.SelectedDate.Value;
                if (endDate < startDate)
                {
                    MessageBox.Show(
                        "End date cannot be before the start date.",
                        "Validation Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    dtpckr_endDate.SelectedDate = startDate.AddDays(1);
                }
            }
        }

        private static bool IsTextNumeric(string text)
        {
            Regex regex = new Regex("[^0-9]+"); // Regex to match non-numeric text
            return !regex.IsMatch(text);
        }

        // Xử lý mở rộng Expander
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            // Cuộn xuống cuối trang khi Expander được mở
            MainScrollViewer.ScrollToEnd();
        }

        // Xử lý đóng cửa sổ
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Xử lý tìm kiếm
        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            // Logic tìm kiếm sẽ được thực hiện ở đây
        }

        // Xử lý chỉnh sửa
        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            // Logic chỉnh sửa sẽ được thực hiện ở đây
        }

        // Xử lý xóa
        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            // Logic xóa sẽ được thực hiện ở đây
        }

        // Xử lý di chuyển cửa sổ bằng chuột
        private void EventDescription_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
