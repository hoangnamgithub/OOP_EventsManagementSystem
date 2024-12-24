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
    /// Interaction logic for ShowOrPerformer.xaml
    /// </summary>
    public partial class ShowOrPerformer : Window
    {
        public ShowOrPerformer()
        {
            InitializeComponent();
        }

        private void btn_chooseShow_click(object sender, RoutedEventArgs e)
        {
        
        }

        private void btn_choosePerformer_click(object sender, RoutedEventArgs e)
        {
        var performerDescription = new PerformerDescription();
            performerDescription.Show();
            this.Close();
        }
    }
}
