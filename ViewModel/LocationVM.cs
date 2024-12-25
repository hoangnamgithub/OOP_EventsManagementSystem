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

        public void LoadData()
        {
            using (var context = new EventManagementDbContext())
            {
                Venues = new ObservableCollection<VenueViewModel>(
                    context.Venues.Select(v => new VenueViewModel
                    {
                        VenueId = v.VenueId,
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
        public int VenueId { get; set; } // Thêm VenueId
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
            // Tạo cửa sổ LocationDescription và truyền VenueViewModel cùng Action để reload
            var locationDescription = new LocationDescription(this, ReloadData);

            // Hiển thị cửa sổ
            locationDescription.ShowDialog();
        }

        private void ReloadData()
        {
            // Reload dữ liệu từ cơ sở dữ liệu
            var locationVM = Application.Current.MainWindow.DataContext as LocationVM;
            locationVM?.LoadData();
        }
    }


}
