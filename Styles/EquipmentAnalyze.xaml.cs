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
using OOP_EventsManagementSystem.ViewModel;

namespace OOP_EventsManagementSystem.Styles
{
    /// <summary>
    /// Interaction logic for EquipmentAnalyze.xaml
    /// </summary>
    public partial class EquipmentAnalyze : UserControl
    {
        public EquipmentAnalyze()
        {
            InitializeComponent();
            this.DataContext = new HomeVM();
        }
    }
}
