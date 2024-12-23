using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.Styles;
using OOP_EventsManagementSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OOP_EventsManagementSystem.ViewModel
{
    class ShowVM : INotifyPropertyChanged
    {
        private ObservableCollection<Performer> _performers;
        public ObservableCollection<Performer> Performers
        {
            get { return _performers; }
            set
            {
                _performers = value;
                OnPropertyChanged(nameof(Performers)); // Tạo phương thức OnPropertyChanged trong BaseViewModel để thông báo thay đổi
            }
        }

        private readonly EventManagementDbContext _context;

        public ShowVM()
        {
            _context = new EventManagementDbContext();
            Performers = new ObservableCollection<Performer>();
        }

        // Sử dụng ToList thay vì ToListAsync
        public void LoadPerformers()
        {
            var performersFromDb = _context.Performers.ToList(); // Sử dụng ToList() thay vì ToListAsync()
            Performers.Clear();
            foreach (var performer in performersFromDb)
            {
                Performers.Add(performer);
            }
        }
        // Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
