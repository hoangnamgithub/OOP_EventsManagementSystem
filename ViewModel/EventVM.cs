using OOP_EventsManagementSystem.View;
using System.Windows.Controls;
using System.Windows.Input;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class EventVM
    {
        public string ImagePath { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Weekday { get; set; }
        public string Name { get; set; }

        public ICommand ItemSelectedCommand { get; set; }

        public EventVM()
        {
            // Khởi tạo lệnh, khi người dùng nhấn vào sự kiện, hàm OnItemSelected sẽ được gọi
            ItemSelectedCommand = new RelayCommand(OnItemSelected);
        }

        private void OnItemSelected(object parameter)
        {
            if (parameter is EventVM eventVM)
            {
                // Log thông tin sự kiện đã chọn
                Console.WriteLine($"{eventVM.Name} đã được chọn!");

                // Tìm Frame trong cửa sổ chính
                var frame = Application.Current.MainWindow.FindName("MainFrame") as Frame;
                if (frame != null)
                {
                    // Tạo đối tượng trang chi tiết sự kiện
                    var eventDetails = new EventDetails
                    {
                        DataContext = new EventDetailsVM(
                            eventVM.Name,
                            eventVM.Day,
                            eventVM.Month,
                            eventVM.Weekday,
                            eventVM.ImagePath)
                    };

                    // Điều hướng đến trang chi tiết sự kiện
                    frame.Navigate(eventDetails);
                }
            }
        }


    }
}
