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
using OOP_EventsManagementSystem.Styles;
using OOP_EventsManagementSystem.ViewModel;

namespace OOP_EventsManagementSystem.View
{
    /// <summary>
    /// Interaction logic for Show.xaml
    /// </summary>
    public partial class Show : UserControl
    {
        public Show()
        {
            InitializeComponent();
            this.DataContext = new ShowVM();
            btn_add.IsEnabled = true;
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            // Vô hiệu hóa nút Add khi cửa sổ được mở
            btn_add.IsEnabled = false;           
            var ownerWindow = Window.GetWindow(this); // Lấy cửa sổ cha
            var choose = new ShowOrPerformer
            {
                Owner = ownerWindow
            };
            choose.ShowDialog();
            choose.Closed += choose_Closed;
        }
        private void choose_Closed(object? sender, EventArgs e)
        {
            // Kích hoạt lại nút Add sau khi cửa sổ được đóng
            btn_add.IsEnabled = true;

            // Hủy đăng ký sự kiện Closed để tránh lỗi nếu cửa sổ được tạo lại
            if (sender is Window window)
            {
                window.Closed -= choose_Closed;
            }
        }
    }
}
