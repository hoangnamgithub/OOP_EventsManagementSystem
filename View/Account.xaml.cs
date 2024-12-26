using Microsoft.Win32;
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

namespace OOP_EventsManagementSystem.View
{
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : UserControl
    {
        public Account()
        {
            InitializeComponent();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Mở hộp thoại OpenFileDialog để chọn ảnh
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png"; // Chỉ chấp nhận định dạng .jpg và .png

            if (openFileDialog.ShowDialog() == true)
            {
                // Lấy đường dẫn file hình ảnh được chọn
                string selectedFilePath = openFileDialog.FileName;

                try
                {
                    // Cập nhật nguồn hình ảnh
                    imgEvent.Source = new BitmapImage(new Uri(selectedFilePath));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể tải ảnh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        // Phương thức để nhận dữ liệu và cập nhật vào các điều khiển
        public void SetAccountInfo(string email, string password, string permission)
        {
            txtEmail.Text = email;
            txtPassword.Text = password;
            txtPermission.Text = permission;
        }
    }
}
