using OOP_EventsManagementSystem.Styles;
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
    /// Interaction logic for Partner.xaml
    /// </summary>
    public partial class Partner : UserControl
    {
        public Partner()
        {
            InitializeComponent();
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            // Vô hiệu hóa nút Add khi cửa sổ được mở
            btn_add.IsEnabled = false;
            var ownerWindow = Window.GetWindow(this); // Lấy cửa sổ cha
            var partnerDescription = new PartnerDescription
            {
                Owner = ownerWindow
            };
            partnerDescription.ShowDialog();
            partnerDescription.Closed += partnerDescription_Closed;
        }
        private void partnerDescription_Closed(object? sender, EventArgs e)
        {
            // Kích hoạt lại nút Add sau khi cửa sổ được đóng
            btn_add.IsEnabled = true;

            // Hủy đăng ký sự kiện Closed để tránh lỗi nếu cửa sổ được tạo lại
            if (sender is Window window)
            {
                window.Closed -= partnerDescription_Closed;
            }
        }
    }
}
