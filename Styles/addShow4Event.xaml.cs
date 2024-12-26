using OOP_EventsManagementSystem.Model;
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
using OOP_EventsManagementSystem.ViewModel;


namespace OOP_EventsManagementSystem.Styles
{
    /// <summary>
    /// Interaction logic for addShow4Event.xaml
    /// </summary>
    public partial class addShow4Event : Window
    {
        public int SelectedEventId { get; set; }  // Thuộc tính lưu trữ EventId đã chọn

        public addShow4Event(int selectedEventId)
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
                    // Lấy danh sách Genre từ database
                    var genres = context.Genres.ToList();
                    genreComboBox.ItemsSource = genres;
                    genreComboBox.DisplayMemberPath = "Genre1";
                    genreComboBox.SelectedValuePath = "GenreId";

                    // Lấy danh sách Performer từ database
                    var performers = context.Performers.ToList();
                    performerComboBox.ItemsSource = performers;
                    performerComboBox.DisplayMemberPath = "FullName";
                    performerComboBox.SelectedValuePath = "PerformerId";

                    // Lấy danh sách Show từ database
                    var shows = context.Shows.ToList();
                    showComboBox.ItemsSource = shows;
                    showComboBox.DisplayMemberPath = "ShowName";
                    showComboBox.SelectedValuePath = "ShowId";

                    // Khi người dùng chọn Performer hoặc Genre, lọc lại danh sách Show
                    performerComboBox.SelectionChanged += (sender, e) => FilterShows();
                    genreComboBox.SelectionChanged += (sender, e) => FilterShows();

                    // Khi người dùng chọn Show, tự động gán Performer, Genre và Cost
                    showComboBox.SelectionChanged += (sender, e) =>
                    {
                        var selectedShow = showComboBox.SelectedItem as Show;
                        if (selectedShow != null)
                        {
                            performerComboBox.SelectedValue = selectedShow.PerformerId;
                            txtCost.Text = selectedShow.Cost.ToString();
                            genreComboBox.SelectedValue = selectedShow.GenreId;
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading genres, performers, and shows: {ex.Message}");
            }
        }
    
    // Hàm để lọc lại danh sách Show khi Performer hoặc Genre thay đổi
    private void FilterShows()
        {
            try
            {
                using (var context = new EventManagementDbContext())
                {
                    // Lưu giá trị hiện tại của showComboBox trước khi lọc
                    var currentShowId = showComboBox.SelectedValue as int?;

                    var selectedPerformerId = performerComboBox.SelectedValue as int?;
                    var selectedGenreId = genreComboBox.SelectedValue as int?;

                    var filteredShows = context.Shows.AsQueryable();

                    // Lọc theo Performer nếu có
                    if (selectedPerformerId.HasValue)
                    {
                        filteredShows = filteredShows.Where(show => show.PerformerId == selectedPerformerId.Value);
                    }

                    // Lọc theo Genre nếu có
                    if (selectedGenreId.HasValue)
                    {
                        filteredShows = filteredShows.Where(show => show.GenreId == selectedGenreId.Value);
                    }

                    // Lấy danh sách Show đã lọc
                    var filteredShowsList = filteredShows.ToList();

                    // Cập nhật lại ItemsSource cho ComboBox
                    showComboBox.ItemsSource = filteredShowsList;

                    // Nếu giá trị hiện tại của showComboBox vẫn còn trong danh sách đã lọc, khôi phục lại nó
                    if (currentShowId.HasValue && filteredShowsList.Any(show => show.ShowId == currentShowId.Value))
                    {
                        showComboBox.SelectedValue = currentShowId.Value;
                    }
                    else
                    {
                        // Nếu giá trị không còn trong danh sách lọc, có thể chọn giá trị mặc định hoặc để trống
                        showComboBox.SelectedValue = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering shows: {ex.Message}");
            }
        }


        private void btn_comfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new EventManagementDbContext())
                {
                    // Kiểm tra xem Show có được chọn hay không
                    var selectedShowId = showComboBox.SelectedValue as int?;
                    if (!selectedShowId.HasValue)
                    {
                        MessageBox.Show("Please select a show.");
                        return; // Ngừng thực thi nếu không có show được chọn
                    }

                    int showId = selectedShowId.Value; // Lấy giá trị int từ selectedShowId

                    // Kiểm tra xem sự kiện đã được chọn chưa
                    var selectedEventId = SelectedEventId; // Bạn cần đảm bảo rằng SelectedEventId có giá trị hợp lệ
                    if (selectedEventId == 0)
                    {
                        MessageBox.Show("Please select an event.");
                        return; // Ngừng thực thi nếu không có sự kiện được chọn
                    }

                    // Kiểm tra xem Show đã tồn tại trong Event chưa
                    var existingShow = context.ShowSchedules
                        .FirstOrDefault(ss => ss.EventId == selectedEventId && ss.ShowId == showId);

                    if (existingShow == null)
                    {
                        // Thêm Show vào Event nếu chưa tồn tại
                        var newShowSchedule = new ShowSchedule
                        {
                            EventId = selectedEventId,
                            ShowId = showId
                        };

                        context.ShowSchedules.Add(newShowSchedule);
                        context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                        MessageBox.Show("Show added successfully!");

                        // Cập nhật lại danh sách Shows trong DataGrid của cửa sổ EventDetails
                        var eventDetailsWindow = Application.Current.Windows.OfType<EventDetails>().FirstOrDefault();
                        if (eventDetailsWindow != null)
                        {
                            var viewModel = eventDetailsWindow.DataContext as EventVM;
                            if (viewModel != null)
                            {
                                viewModel.LoadShowsForEvent(selectedEventId);
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

                        // Đóng cửa sổ addShow4Event sau khi lưu
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("This Show is already added to the Event.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding Show: {ex.Message}");
            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { if (e.ButtonState == MouseButtonState.Pressed) { this.DragMove(); } }


    }
}
