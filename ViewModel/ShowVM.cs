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
using Microsoft.EntityFrameworkCore;
using System.Windows;
using static Bogus.DataSets.Name;

namespace OOP_EventsManagementSystem.ViewModel
{
    class ShowVM : INotifyPropertyChanged
    {
        private readonly EventManagementDbContext _context;
        public ObservableCollection<Genre> Genres { get; set; }
        public ObservableCollection<Performer> Performers { get; set; }
        public ObservableCollection<Show> SelectedPerformerShows { get; set; }

        private int? _selectedPerformerId;
        public int? SelectedPerformerId
        {
            get { return _selectedPerformerId; }
            set
            {
                _selectedPerformerId = value;
                OnPropertyChanged(nameof(SelectedPerformerId));
            }
        }

        public ShowVM()
        {
            _context = new EventManagementDbContext();
            Performers = new ObservableCollection<Performer>();
            SelectedPerformerShows = new ObservableCollection<Show>();
            Genres = new ObservableCollection<Genre>(_context.Genres.ToList());
        }

        public void LoadPerformers()
        {
            var performers = _context.Performers.ToList();
            Performers.Clear();
            foreach (var performer in performers)
            {
                Performers.Add(performer);
            }
        }

        public void LoadShowsForSelectedPerformer()
        {
            if (SelectedPerformerId == null)
            {
                SelectedPerformerShows.Clear();
                return;
            }

            var shows = _context.Shows
                .Include(show => show.Genre) // Nạp thông tin Genre
                .Where(show => show.PerformerId == SelectedPerformerId)
                .ToList();

            SelectedPerformerShows.Clear();
            foreach (var show in shows)
            {
                SelectedPerformerShows.Add(show);
            }
        }

        public void DeleteShow(Show show)
        {
            if (show != null)
            {
                // Lấy danh sách ShowSchedule liên quan
                var relatedSchedules = _context.ShowSchedules
                    .Where(s => s.ShowId == show.ShowId)
                    .ToList();

                // Xóa các bản ghi ShowSchedule chưa diễn ra
                var today = DateOnly.FromDateTime(DateTime.Now);
                var schedulesToDelete = relatedSchedules
                    .Where(s => s.StartDate > today) // Chỉ lấy các bản ghi chưa diễn ra
                    .ToList();

                _context.ShowSchedules.RemoveRange(schedulesToDelete);

                // Kiểm tra nếu không còn lịch trình nào thì xóa show
                if (!relatedSchedules.Any(s => s.StartDate <= today))
                {
                    _context.Shows.Remove(show); // Xóa show nếu không có lịch diễn còn lại
                }


                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();

                // Xóa khỏi danh sách hiển thị
                SelectedPerformerShows.Remove(show);
            }
        }

        public void SaveChanges(Show showToSave)
        {
            try
            {
                // Tìm bản ghi Show trong cơ sở dữ liệu
                var showToUpdate = _context.Shows
                    .Include(show => show.Genre) // Load dữ liệu liên quan nếu cần
                    .FirstOrDefault(show => show.ShowId == showToSave.ShowId);

                if (showToUpdate != null)
                {
                    // Cập nhật thông tin của Show
                    showToUpdate.ShowName = showToSave.ShowName;
                    showToUpdate.Cost = showToSave.Cost;

                    // Đảm bảo Genre không null trước khi cập nhật
                    if (showToSave.Genre != null)
                    {
                        showToUpdate.Genre.Genre1 = showToSave.Genre.Genre1;
                    }

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _context.SaveChanges();

                    // Reload lại danh sách Show sau khi thay đổi
                    LoadShowsForSelectedPerformer();
                    MessageBox.Show("Changes have been saved successfully.", "Success");
                }
                else
                {
                    MessageBox.Show("Show not found in the database.", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving changes: {ex.Message}", "Error");
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
