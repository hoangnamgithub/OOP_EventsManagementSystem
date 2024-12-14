using OOP_EventsManagementSystem.Utilities;
using OOP_EventsManagementSystem.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using OOP_EventsManagementSystem.Model;
using Microsoft.Identity.Client;


namespace OOP_EventsManagementSystem.ViewModel
{
    internal class EventVM : INotifyPropertyChanged
    {
        private readonly EventManagementDbContext _context;

        public ObservableCollection<Model.Event> Events { get; set; }

        public EventVM()
        {
            _context = new EventManagementDbContext();
            LoadData();
        }

        private void LoadData()
        {
            Events = new ObservableCollection<Model.Event>(_context.Events.Include(e => e.Venue).ToList()); 
            OnPropertyChanged(nameof(Events));
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
