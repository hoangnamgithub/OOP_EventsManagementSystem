using System;
using System.Windows;
using System.Windows.Controls;

namespace OOP_EventsManagementSystem.Styles
{
    public partial class EventDetailCard_v2 : UserControl
    {
        public EventDetailCard_v2()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty EventNameProperty =
            DependencyProperty.Register("EventName", typeof(string), typeof(EventDetailCard_v2), new PropertyMetadata(string.Empty));

        public string EventName
        {
            get => (string)GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }

        public static readonly DependencyProperty VenueNameProperty =
            DependencyProperty.Register("VenueName", typeof(string), typeof(EventDetailCard_v2), new PropertyMetadata(string.Empty));

        public string VenueName
        {
            get => (string)GetValue(VenueNameProperty);
            set => SetValue(VenueNameProperty, value);
        }

        public static readonly DependencyProperty StartDateProperty =
            DependencyProperty.Register("StartDate", typeof(DateTime), typeof(EventDetailCard_v2), new PropertyMetadata(default(DateTime)));

        public DateTime StartDate
        {
            get => (DateTime)GetValue(StartDateProperty);
            set => SetValue(StartDateProperty, value);
        }

        public static readonly DependencyProperty EndDateProperty =
            DependencyProperty.Register("EndDate", typeof(DateTime), typeof(EventDetailCard_v2), new PropertyMetadata(default(DateTime)));

        public DateTime EndDate
        {
            get => (DateTime)GetValue(EndDateProperty);
            set => SetValue(EndDateProperty, value);
        }
    }
}
