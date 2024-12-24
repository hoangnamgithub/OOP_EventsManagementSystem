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
    /// Interaction logic for PerformerDescription.xaml
    /// </summary>
    public partial class PerformerDescription : Window
    {
        private ShowVM _viewModel;

        public PerformerDescription()
        {
            InitializeComponent();

            // Khởi tạo ViewModel nếu chưa được thiết lập
            _viewModel = new ShowVM();

            // Gán DataContext cho cửa sổ để liên kết View với ViewModel
            this.DataContext = _viewModel;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy dữ liệu từ các TextBox
            string fullName = FullNameTextBox.Text;
            string contact = ContactTextBox.Text;

            // Kiểm tra xem người dùng đã nhập đầy đủ thông tin chưa
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(contact))
            {
                MessageBox.Show("Please fill in all the required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Tạo đối tượng Performer mới
            var performer = new Performer
            {
                FullName = fullName,
                ContactDetail = contact
            };

            // Gọi hàm từ ViewModel để lưu dữ liệu
            _viewModel.SaveChangesForPerformer(performer);
            _viewModel.LoadPerformers();
            this.Close();
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    

    private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { if (e.ButtonState == MouseButtonState.Pressed) { this.DragMove(); } }
    }
}
