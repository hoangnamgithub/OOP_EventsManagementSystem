using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP_EventsManagementSystem.Model;

namespace OOP_EventsManagementSystem.ViewModel
{
    internal class AddEventVM : INotifyPropertyChanged
    {
        private ObservableCollection<Show> _shows;
        public ObservableCollection<Show> Shows
        {
            get => _shows;
            set
            {
                _shows = value;
                OnPropertyChanged(nameof(Shows));
            }
        }

        public AddEventVM()
        {
            _context = new EventManagementDbContext();
            LoadShows();
        }

        private void LoadShows()
        {
            Shows = new ObservableCollection<Show>(
                _context.Shows.Include(s => s.Performer).Include(s => s.Genre).ToList()
            );
        }

        private readonly EventManagementDbContext _context;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
