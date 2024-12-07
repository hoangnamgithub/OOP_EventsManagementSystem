using OOP_EventsManagementSystem.View;
using OOP_EventsManagementSystem.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OOP_EventsManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPasswordVisible = false; // Biến lưu trạng thái ẩn/hiển mật khẩu

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Event());
        }

        private void btn_eye_Click(object sender, RoutedEventArgs e)
        {
            if (isPasswordVisible)
            {
                // Nếu mật khẩu đang hiển thị, chuyển sang ẩn
                txt_box_password_visible.Visibility = Visibility.Collapsed;
                txt_box_password.Visibility = Visibility.Visible;
                txt_box_password.Password = txt_box_password_visible.Text; // Đồng bộ nội dung
                img_eye.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/eye_off.png"));
                isPasswordVisible = false;
            }
            else
            {
                // Nếu mật khẩu đang ẩn, chuyển sang hiển thị
                txt_box_password_visible.Visibility = Visibility.Visible;
                txt_box_password.Visibility = Visibility.Collapsed;
                txt_box_password_visible.Text = txt_box_password.Password; // Đồng bộ nội dung
                img_eye.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/eye_on.png"));
                isPasswordVisible = true;
            }
        }

        private void btn_signup_Click(object sender, RoutedEventArgs e)
        {
            // Tạo đối tượng trang SignUp
            SignUp signUpWindow = new SignUp();

            // Hiển thị trang SignUp
            signUpWindow.Show();

            // Đóng trang Login (MainWindow)
            this.Close();
        }
        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            // Tạo đối tượng Event UserControl
            Event eventControl = new Event();

            // Gắn Event UserControl vào container chính
            MainContainer.Children.Clear(); // Xóa các thành phần cũ nếu cần
            MainContainer.Children.Add(eventControl);

            // Hiển thị MainContainer
            MainContainer.Visibility = Visibility.Visible;

            // Ẩn các thành phần liên quan đến giao diện login
            LoginContainer.Visibility = Visibility.Collapsed; // Đảm bảo LoginContainer được khai báo
        }

        private void PowerOff_Click(object sender, RoutedEventArgs e)
        {
            // Tắt cửa sổ hiện tại
            this.Close();
        }

      

    }
}