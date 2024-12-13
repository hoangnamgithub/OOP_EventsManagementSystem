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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OOP_EventsManagementSystem.Styles
{
    /// <summary>
    /// Interaction logic for SearchBar.xaml
    /// </summary>
    public partial class SearchBar : UserControl
    {
        public SearchBar()
        {
            InitializeComponent();
        }
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e) { if (SearchBox.Text == "Search......") { SearchBox.Text = ""; SearchBox.CaretBrush = System.Windows.Media.Brushes.Black; } }
        private void SearchBox_LostFocus(object sender, RoutedEventArgs e) { if (string.IsNullOrWhiteSpace(SearchBox.Text)) { SearchBox.Text = "Search......"; SearchBox.CaretBrush = System.Windows.Media.Brushes.Transparent; } }
        private void btn_search_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}