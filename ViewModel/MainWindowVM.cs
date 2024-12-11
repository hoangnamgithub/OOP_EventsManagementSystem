using OOP_EventsManagementSystem.Utilities;
using System.Windows.Input;
using OOP_EventsManagementSystem.View;

public class MainWindowVM : ViewModelBase
{
    private object _currentView;
    public object CurrentView
    {
        get { return _currentView; }
        set
        {
            _currentView = value;
            OnPropertyChanged(nameof(CurrentView));
        }
    }

    public ICommand NavigateToEventCommand { get; set; }

    public MainWindowVM()
    {
        NavigateToEventCommand = new RelayCommand(OnNavigateToEvent); // Chỗ này đã sửa
    }

    // Phương thức OnNavigateToEvent nhận tham số kiểu object
    private void OnNavigateToEvent(object parameter)
    {
        // Thiết lập CurrentView để hiển thị UserControl Event
        CurrentView = new Event();  // Điều này sẽ hiển thị UserControl Event.xaml
    }
}
