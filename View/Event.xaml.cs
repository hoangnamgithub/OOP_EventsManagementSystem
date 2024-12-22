
﻿using OOP_EventsManagementSystem.Styles;
﻿using OOP_EventsManagementSystem.ViewModel;

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
    /// Interaction logic for Event.xaml
    /// </summary>
    public partial class Event : UserControl
    {

        public Event()
        {
            InitializeComponent();
            this.DataContext = new EventVM();
            AddButton.IsEnabled = true;
            CurrentDateTextBlock.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
            AddButton.IsEnabled = false;

            
            var eventDescriptionWindow = new EventDetails();

            
            eventDescriptionWindow.Closed += EventDescriptionWindow_Closed;

            // Hiển thị cửa sổ
            eventDescriptionWindow.Show();
        }

        private void EventDescriptionWindow_Closed(object? sender, EventArgs e)
        {
            // Kích hoạt lại nút Add sau khi cửa sổ được đóng
            AddButton.IsEnabled = true;

            // Hủy đăng ký sự kiện Closed để tránh lỗi nếu cửa sổ được tạo lại
            if (sender is Window window)
            {
                window.Closed -= EventDescriptionWindow_Closed;
            }
        }       
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e) { if (SearchBox.Text == "Search......") { SearchBox.Text = ""; SearchBox.CaretBrush = System.Windows.Media.Brushes.Black; } }
        private void SearchBox_LostFocus(object sender, RoutedEventArgs e) { if (string.IsNullOrWhiteSpace(SearchBox.Text)) { SearchBox.Text = "Search......"; SearchBox.CaretBrush = System.Windows.Media.Brushes.Transparent; } }
        private void btn_search_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
