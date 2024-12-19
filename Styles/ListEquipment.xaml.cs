using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace OOP_EventsManagementSystem.Styles
{
    public partial class ListEquipment : UserControl
    {
        public ListEquipment()
        {
            InitializeComponent();
            LoadChartData(); // Load the data and render the chart

            var series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Available",
                    Values = new ChartValues<int> { 70 }, // 70% available
                    DataLabels = true,
                    LabelPoint = chartPoint => $"{chartPoint.Y}%"
                },
                new PieSeries
                {
                    Title = "Being Used",
                    Values = new ChartValues<int> { 30 }, // 30% being used
                    DataLabels = true,
                    LabelPoint = chartPoint => $"{chartPoint.Y}%"
                }
            };

            // Bind the series to the chart
            this.DataContext = new { SeriesCollection = series, EquipmentTypes = GetEquipmentData() };
        }

        private List<EquipmentType> GetEquipmentData()
        {
            return new List<EquipmentType>
            {
                new EquipmentType { TypeName = "Camera", Quantity = 120, Color = "#FF5733" },
                new EquipmentType { TypeName = "Microphone", Quantity = 80, Color = "#33FF57" },
                new EquipmentType { TypeName = "Lighting", Quantity = 60, Color = "#3357FF" },
                new EquipmentType { TypeName = "Tripod", Quantity = 40, Color = "#FF33A1" },
                new EquipmentType { TypeName = "Monitor", Quantity = 30, Color = "#FF8333" }
            };
        }

        private void LoadChartData()
        {
            // Existing chart data handling code...
        }

        public class EquipmentType
        {
            public string TypeName { get; set; }
            public int Quantity { get; set; }
            public string Color { get; set; }
        }

        public class EquipmentStatus
        {
            public string Status { get; set; }
            public int Quantity { get; set; }
            public string Color { get; set; }
        }

        private void TimePeriodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Your logic for handling selection changes
        }

        private void btn_edit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var equipmentDescription = new EquipmentDescription();
            equipmentDescription.Show();
        }

        private void btn_add_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var equipmentDescription = new EquipmentDescription();
            equipmentDescription.Show();
        }
    }
}
