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

namespace OOP_EventsManagementSystem
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            // Tạo đối tượng trang SignUp
            MainWindow mainWindow = new MainWindow();

            // Hiển thị trang SignUp
            mainWindow.Show();

            // Đóng trang Login (MainWindow)
            this.Close();
        }
        private void PowerOff_Click(object sender, RoutedEventArgs e)
        {
            // Tắt cửa sổ hiện tại
            this.Close();
        }
    }
}

