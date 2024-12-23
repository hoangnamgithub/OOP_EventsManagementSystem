﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OOP_EventsManagementSystem.ViewModel;
using OOP_EventsManagementSystem.ViewModel;

namespace OOP_EventsManagementSystem.Styles
{
    /// <summary>
    /// Interaction logic for EventDetails.xaml
    /// </summary>
    public partial class EventDetails : Window
    {
        public EventDetails()
        {
            InitializeComponent();
            DataContext = new EventVM();
        }

        // Xử lý mở rộng Expander
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            // Cuộn xuống cuối trang khi Expander được mở
            MainScrollViewer.ScrollToEnd();
        }

        // Xử lý đóng cửa sổ
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Xử lý tìm kiếm
        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            // Logic tìm kiếm sẽ được thực hiện ở đây
        }

        // Xử lý chỉnh sửa
        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            // Logic chỉnh sửa sẽ được thực hiện ở đây
        }

        // Xử lý xóa
        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            // Logic xóa sẽ được thực hiện ở đây
        }

        // Xử lý di chuyển cửa sổ bằng chuột
        private void EventDescription_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void btn_startEdit_Click(object sender, RoutedEventArgs e)
        {
            if (btn_startEdit.Content.ToString() == "Edit")
            {
                btn_startEdit.Content = "Confirm";
            }
            else
            {
                btn_startEdit.Content = "Edit";
            }
        }
    }
}
