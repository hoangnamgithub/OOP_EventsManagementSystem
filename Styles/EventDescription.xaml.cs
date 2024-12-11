﻿using System;
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
    /// Interaction logic for EventDescription.xaml
    /// </summary>
    public partial class EventDescription : Window
    {
        public EventDescription()
        {
            InitializeComponent();
        }
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {

        }
        // Sự kiện kéo cửa sổ
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Kích hoạt tính năng kéo của Window
                this.DragMove();
            }
        }
    }
}
