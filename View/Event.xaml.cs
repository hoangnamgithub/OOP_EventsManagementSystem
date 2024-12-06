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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OOP_EventsManagementSystem.View
{
    public partial class Event : UserControl
    {
        public Event()
        {
            InitializeComponent();

            // Gắn một ViewModel mẫu cho mục đích minh họa
            DataContext = new EventListVM();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chi tiết sự kiện");
        }
    }
}
