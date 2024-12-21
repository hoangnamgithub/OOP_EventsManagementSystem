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
    /// Interaction logic for PartnerDescription.xaml
    /// </summary>
    public partial class PartnerDescription : Window
    {
        private readonly EventManagementDbContext _context;
        public PartnerDescription()
        {
            InitializeComponent();
            _context = new EventManagementDbContext();
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
        this.Close();
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { if (e.ButtonState == MouseButtonState.Pressed) { this.DragMove(); } }
        // Khi cửa sổ được hiển thị, dữ liệu sẽ được gán từ DataContext
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
    var selectedSponsor = (SponsorModel)this.DataContext;

            // Kiểm tra và xử lý dữ liệu, ví dụ hiển thị thông tin chi tiết
            Console.WriteLine(selectedSponsor.SponsorName);
        }
    }
}
