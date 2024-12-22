using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Styles;
using OOP_EventsManagementSystem.Utilities;
using OOP_EventsManagementSystem.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace OOP_EventsManagementSystem.ViewModel
{

    public class LocationVM : INotifyPropertyChanged
    {
        
        private ObservableCollection<VenueViewModel> _venues;
        public ObservableCollection<VenueViewModel> Venues
        {
            get => _venues;
            set
            {
                _venues = value;
                OnPropertyChanged(nameof(Venues));
            }
        }

        public LocationVM()
        {
            LoadData();
        }

        private void LoadData()

        {           
            using (var context = new EventManagementDbContext())
            {
                Venues = new ObservableCollection<VenueViewModel>(
                    context.Venues.Select(v => new VenueViewModel
                    {
                        VenueName = v.VenueName,
                        Cost = v.Cost,
                        Address = v.Address,
                        Capacity = v.Capacity
                    }).ToList()
                );
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

public class VenueViewModel
    {
        public string VenueName { get; set; }
        public decimal Cost { get; set; }
        public string? Address { get; set; }
        public int Capacity { get; set; }

        // Command để mở LocationDescription
        public ICommand EditCommand { get; set; }

        public VenueViewModel()
        {
            EditCommand = new RelayCommand(OpenLocationDescription);
        }

        private void OpenLocationDescription(object parameter)
        {
            // Tạo cửa sổ LocationDescription
            var locationDescription = new LocationDescription();

            // Gán dữ liệu từ VenueViewModel vào các TextBox
            locationDescription.txtVenueName.Text = VenueName;
            locationDescription.txtAddress.Text = Address;
            locationDescription.txtCost.Text = Cost.ToString();
            locationDescription.txtCapacity.Text = Capacity.ToString();

            // Hiển thị cửa sổ
            locationDescription.ShowDialog();
        }
    }


}
