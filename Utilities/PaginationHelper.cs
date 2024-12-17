using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace OOP_EventsManagementSystem.Utilities
{
    public class PaginationHelper<T> : INotifyPropertyChanged
    {
        private int _currentPage = 1;
        private int _itemsPerPage;

        public ObservableCollection<T> FullCollection { get; set; }
        public ObservableCollection<T> PagedCollection { get; set; }

        
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));
                    UpdatePagedCollection();
                }
            }
        }

        public int TotalPages => (FullCollection.Count + _itemsPerPage - 1) / _itemsPerPage;

        public PaginationHelper(IEnumerable<T> collection, int itemsPerPage = 45)
        {
            _itemsPerPage = itemsPerPage;
            FullCollection = new ObservableCollection<T>(collection);
            PagedCollection = new ObservableCollection<T>();
            UpdatePagedCollection();
        }

        public void UpdatePagedCollection()
        {
            var startIndex = (_currentPage - 1) * _itemsPerPage;
            var pagedData = FullCollection.Skip(startIndex).Take(_itemsPerPage).ToList();
            PagedCollection.Clear();
            foreach (var item in pagedData)
                PagedCollection.Add(item);

            OnPropertyChanged(nameof(PagedCollection));
            OnPropertyChanged(nameof(TotalPages));
        }

        public void NextPage()
        {
            if (CurrentPage < TotalPages)
                CurrentPage++;
        }

        public void PreviousPage()
        {
            if (CurrentPage > 1)
                CurrentPage--;
        }

        public bool CanGoNext() => CurrentPage < TotalPages;
        public bool CanGoPrevious() => CurrentPage > 1;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
