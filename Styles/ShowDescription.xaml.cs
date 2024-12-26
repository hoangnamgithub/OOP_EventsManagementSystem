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
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.ViewModel;

namespace OOP_EventsManagementSystem.Styles
{
    /// <summary>
    /// Interaction logic for ShowDescription.xaml
    /// </summary>
    public partial class ShowDescription : Window
    {
        private ShowVM _showVM;

        public ShowDescription()
        {
            InitializeComponent();
            _showVM = new ShowVM(); // Khởi tạo ShowViewModel

            LoadGenres();
        }

        private void ShowVM_RefreshShows()
        {
            // Gọi lại phương thức LoadShowsForSelectedPerformer từ ShowViewModel
            _showVM.LoadShowsForSelectedPerformer();
        }

        private void LoadGenres()
        {
            try
            {
                using (var context = new EventManagementDbContext())
                {
                    // Lấy danh sách Genre từ database
                    var genres = context.Genres.ToList();

                    // Gán danh sách Genre vào ComboBox
                    genreComboBox.ItemsSource = genres;
                    genreComboBox.DisplayMemberPath = "Genre1"; // Hiển thị Genre1
                    genreComboBox.SelectedValuePath = "GenreId"; // Lấy giá trị GenreId khi chọn
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading genres: {ex.Message}");
            }
        }

        public void SetPerformerDetails(string name, string contact)
        {
            txtPerformerName.Text = name;
        }

        private void btn_comfirm_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra dữ liệu nhập vào
            string name = txtShowName.Text; // TextBox cho tên Show
            string performerName = txtPerformerName.Text; // TextBox cho tên Performer
            var selectedGenre = genreComboBox.SelectedValue; // ComboBox cho Genre
            string cost = txtCost.Text; // TextBox cho chi phí Show

            // Kiểm tra nếu bất kỳ trường nào bị bỏ trống
            if (
                string.IsNullOrWhiteSpace(name)
                || string.IsNullOrWhiteSpace(performerName)
                || selectedGenre == null
                || string.IsNullOrWhiteSpace(cost)
            )
            {
                MessageBox.Show(
                    "Please fill in all the required fields.",
                    "Incomplete Data",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            // Kiểm tra định dạng số cho chi phí
            if (!decimal.TryParse(cost, out decimal parsedCost))
            {
                MessageBox.Show(
                    "Cost must be a valid number.",
                    "Invalid Input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            try
            {
                using (var context = new EventManagementDbContext())
                {
                    // Kiểm tra xem performer đã tồn tại trong database chưa
                    var performer = context.Performers.FirstOrDefault(p =>
                        p.FullName == performerName
                    );
                    if (performer == null)
                    {
                        // Nếu performer chưa tồn tại, thêm mới
                        performer = new Performer
                        {
                            FullName = performerName,
                            ContactDetail = "N/A", // Nếu cần thêm thông tin liên hệ, có thể cập nhật ở đây
                        };
                        context.Performers.Add(performer);
                        context.SaveChanges();
                    }

                    // Tạo đối tượng Show mới
                    var newShow = new Show
                    {
                        ShowName = name,
                        PerformerId = performer.PerformerId, // ID performer vừa nhập
                        GenreId = (int)selectedGenre, // ID genre đã chọn
                        Cost = parsedCost,
                    };

                    // Thêm và lưu vào database
                    context.Shows.Add(newShow);
                    context.SaveChanges();

                    MessageBox.Show(
                        "New show added successfully!",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                    this.Close(); // Đóng cửa sổ sau khi lưu
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while saving the show: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
