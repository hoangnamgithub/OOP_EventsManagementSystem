using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using OOP_EventsManagementSystem.ViewModel;

namespace OOP_EventsManagementSystem.View
{
    /// <summary>
    /// Interaction logic for Equipment.xaml
    /// </summary>
    public partial class Equipment : UserControl
    {
        public Equipment()
        {
            InitializeComponent();
            DataContext = new EquipmentVM();
        }
    }
}
