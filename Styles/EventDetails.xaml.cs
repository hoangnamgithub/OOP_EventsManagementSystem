﻿using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Utilities;
using OOP_EventsManagementSystem.ViewModel;
using OOP_EventsManagementSystem.ViewModel;

namespace OOP_EventsManagementSystem.Styles
{
    /// <summary>
    /// Interaction logic for EventDetails.xaml
    /// </summary>
    public partial class EventDetails : Window
    {
        public int SelectedEventId { get; set; }  // Thuộc tính để lưu EventId đã chọn

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
            // Check if IsEditing is true
            if (DataContext is EventVM eventVM && eventVM.IsEditing)
            {
                // Ask the user if they want to discard their changes
                var result = MessageBox.Show(
                    "You have unsaved changes. Are you sure you want to close without confirming?",
                    "Unsaved Changes",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                eventVM.IsEditing = false; // Ensure the window remains in edit mode

                // If the user clicks "No", switch back to edit mode
                if (result == MessageBoxResult.No)
                {
                    
                    return; // This prevents the close action from happening
                }
            }

            // If user clicks Yes or IsEditing is false, proceed to close the window
            this.Close();
        }


        // Xử lý tìm kiếm
        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            // Logic tìm kiếm sẽ được thực hiện ở đây
        }

        // Xử lý chỉnh sửa

        // Xử lý di chuyển cửa sổ bằng chuột
        private void EventDescription_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void btn_addShow_Click(object sender, RoutedEventArgs e)
        {
            var eventViewModel = DataContext as EventVM;
            if (eventViewModel != null)
            {
                var addShow = new addShow4Event(eventViewModel.SelectedEventId);
                addShow.ShowDialog();
            }
        }

        private void btn_addSponsor_Click(object sender, RoutedEventArgs e)
        {
            var eventViewModel = DataContext as EventVM;
            if (eventViewModel != null)
            {
                var addSponsor = new PartnerDescription(eventViewModel.SelectedEventId);
                addSponsor.Show();
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Lấy ViewModel từ DataContext
            var viewModel = (EventVM)this.DataContext;

            // Truy cập SelectedItems của DataGrid
            var selectedItems = (sender as DataGrid).SelectedItems.Cast<FilteredSponsor>().ToList();

            // Cập nhật danh sách SelectedSponsors trong ViewModel thông qua DataContext
            viewModel.SelectedSponsors = new ObservableCollection<FilteredSponsor>(selectedItems);
        }

        private void SearchShows(object sender, RoutedEventArgs e)
        {
            var eventVM = DataContext as EventVM;

            if (eventVM != null)
            {
                string searchText = SearchBox_show.Text.ToLower(); // Lấy văn bản tìm kiếm từ TextBox

                var filteredShows = eventVM.AllShows
                    .Where(show => show.ShowName.ToLower().Contains(searchText) ||
                                   show.Cost.ToString().ToLower().Contains(searchText) ||
                                   show.Performer.FullName.ToLower().Contains(searchText) ||
                                   show.Genre.Genre1.ToLower().Contains(searchText))
                    .ToList();

                if (filteredShows.Count == 0)
                {
                    MessageBox.Show("No shows found matching the search criteria.");
                }

                // Cập nhật lại PagedCollection cho Shows
                eventVM.ShowsPagination.PagedCollection.Clear();
                foreach (var show in filteredShows)
                {
                    eventVM.ShowsPagination.PagedCollection.Add(show);
                }

                eventVM.OnPropertyChanged(nameof(eventVM.ShowsPagination.PagedCollection));
            }
        }

        private void SearchSponsors(object sender, RoutedEventArgs e)
        {
            var eventVM = DataContext as EventVM;

            if (eventVM != null)
            {
                string searchText = SearchBox_sponsor.Text.ToLower(); // Lấy văn bản tìm kiếm từ TextBox

                var filteredSponsors = eventVM.SponsorsPagination.PagedCollection
                    .Where(sponsor => sponsor.SponsorName.ToLower().Contains(searchText) ||
                                      sponsor.TierName.ToLower().Contains(searchText))
                    .ToList();

                // Kiểm tra tính toàn vẹn của dữ liệu sau khi lọc
                MessageBox.Show($"Filtered sponsors count: {filteredSponsors.Count}");

                if (filteredSponsors.Count == 0)
                {
                    MessageBox.Show("No sponsors found matching the search criteria.");
                }

                // Cập nhật lại PagedCollection với dữ liệu đã lọc
                eventVM.SponsorsPagination.PagedCollection.Clear(); // Xóa các phần tử cũ
                foreach (var sponsor in filteredSponsors)
                {
                    eventVM.SponsorsPagination.PagedCollection.Add(sponsor); // Thêm các phần tử đã lọc vào
                }

                // Nếu cần, cập nhật lại PaginationHelper để tái phân trang
                eventVM.SponsorsPagination = new PaginationHelper<FilteredSponsor>(filteredSponsors, 9);

                // Thông báo cho giao diện biết rằng dữ liệu đã thay đổi
                eventVM.OnPropertyChanged(nameof(eventVM.SponsorsPagination.PagedCollection));
            }
        }

    }
}
