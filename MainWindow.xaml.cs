﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OOP_EventsManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_signup_Click(object sender, RoutedEventArgs e)
        {
            // Tạo đối tượng trang SignUp
            SignUp signUpWindow = new SignUp();

            // Hiển thị trang SignUp
            signUpWindow.Show();

            // Đóng trang Login (MainWindow)
            this.Close();
        }
        private void PowerOff_Click(object sender, RoutedEventArgs e)
        {
            // Tắt cửa sổ hiện tại
            this.Close();
        }

    }
}