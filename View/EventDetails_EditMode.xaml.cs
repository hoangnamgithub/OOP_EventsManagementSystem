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
    /// Interaction logic for EventDetails_EditMode.xaml
    /// </summary>
    public partial class EventDetails_EditMode : UserControl
    {
        public EventDetails_EditMode()
        {
            InitializeComponent();

            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                // Ngắt sự kiện trước khi đồng bộ trạng thái
                btn_switch_mode.Checked -= btnSwitchMode_Checked;
                btn_switch_mode.Unchecked -= btnSwitchMode_Unchecked;

                btn_switch_mode.IsChecked = mainWindow.IsEditMode; // Đồng bộ trạng thái

                // Kết nối lại sự kiện sau khi đồng bộ
                btn_switch_mode.Checked += btnSwitchMode_Unchecked;
                btn_switch_mode.Unchecked += btnSwitchMode_Checked;
            }
        }


        private void btnSwitchMode_Checked(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new EventDetails_EditMode());
                mainWindow.IsEditMode = true; // Cập nhật trạng thái edit
            }
        }

        private void btnSwitchMode_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new EventDetails());
                mainWindow.IsEditMode = false; // Cập nhật trạng thái normal
            }
        }




        private void PowerOff_Click(object sender, RoutedEventArgs e)
        {
            // Lấy cửa sổ chứa UserControl (MainWindow)
            Window parentWindow = Window.GetWindow(this);

            // Đóng cửa sổ (nếu tồn tại)
            parentWindow?.Close();
        }
    }
}
