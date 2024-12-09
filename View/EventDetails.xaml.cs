﻿using System;
using System.Windows;
using System.Windows.Controls;
using OOP_EventsManagementSystem.ViewModel;

namespace OOP_EventsManagementSystem.View
{
    /// <summary>
    /// Interaction logic for EventDetails.xaml
    /// </summary>
    public partial class EventDetails : UserControl
    {
        public EventDetails()
        {
            InitializeComponent();
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



        // Đặt phương thức PowerOff_Click bên trong lớp

        private void PowerOff_Click(object sender, RoutedEventArgs e)
        {
            // Lấy cửa sổ chứa UserControl (MainWindow)
            Window parentWindow = Window.GetWindow(this);

            // Đóng cửa sổ (nếu tồn tại)
            parentWindow?.Close();
        }
    }
}
