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
using System.Windows.Shapes;

namespace OOP_EventsManagementSystem.Styles
{
    /// <summary>
    /// Interaction logic for LocationDescription.xaml
    /// </summary>
    public partial class LocationDescription : Window
    {
        public string VenueName { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
        public decimal Cost { get; set; }
        public LocationDescription()
        {
            InitializeComponent();
            this.DataContext = new LocationVM();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Bind dữ liệu vào các control trong giao diện
            txtVenueName.Text = VenueName;
            txtAddress.Text = Address;
            txtCapacity.Text = Capacity.ToString();
            txtCost.Text = Cost.ToString();
        }
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) { this.DragMove(); }
        }
    }
}
