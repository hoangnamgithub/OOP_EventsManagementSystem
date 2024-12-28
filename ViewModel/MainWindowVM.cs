using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.View;
using System.ComponentModel;
using System.Windows.Input;

public class MainWindowVM : INotifyPropertyChanged
{
    private object _currentView;
    private int _permissionId; // Thêm thuộc tính PermissionId
    private readonly EventManagementDbContext _context; // Khai báo DbContext

    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged(nameof(CurrentView));
        }
    }

    public int PermissionId
    {
        get => _permissionId;
        set
        {
            _permissionId = value;
            OnPropertyChanged(nameof(PermissionId));
            UpdateCommandsAvailability(); // Cập nhật quyền truy cập cho các lệnh khi PermissionId thay đổi
        }
    }

    public ICommand HomeCommand { get; }
    public ICommand EventCommand { get; }
    public ICommand ShowCommand { get; }
    public ICommand PartnerCommand { get; }
    public ICommand EmployeeCommand { get; }
    public ICommand EquipmentCommand { get; }
    public ICommand LocationCommand { get; }

    // Constructor nhận PermissionId
    public MainWindowVM(int permissionId) // Thêm tham số PermissionId vào constructor
    {
        _permissionId = permissionId;

        HomeCommand = new RelayCommand(ExecuteHomeCommand, CanExecuteHomeCommand);
        EventCommand = new RelayCommand(ExecuteEventCommand, CanExecuteEventCommand);
        ShowCommand = new RelayCommand(ExecuteShowCommand, CanExecuteShowCommand);
        PartnerCommand = new RelayCommand(ExecutePartnerCommand, CanExecutePartnerCommand);
        EmployeeCommand = new RelayCommand(ExecuteEmployeeCommand, CanExecuteEmployeeCommand);
        EquipmentCommand = new RelayCommand(ExecuteEquipmentCommand, CanExecuteEquipmentCommand);
        LocationCommand = new RelayCommand(ExecuteLocationCommand, CanExecuteLocationCommand);
        if (permissionId == 1)
        {
            CurrentView = new Home();
        }
        else if ( permissionId ==2)
        {
            CurrentView = new OOP_EventsManagementSystem.View.Employee();
        }
        else if (permissionId == 3)
        {
            CurrentView = new OOP_EventsManagementSystem.View.Event();
        }



    }

    private void ExecuteHomeCommand(object parameter)
    {
        CurrentView = new Home();
    }

    private void ExecuteEventCommand(object parameter)
    {
        CurrentView = new OOP_EventsManagementSystem.View.Event();
    }

    private void ExecuteShowCommand(object parameter)
    {
        CurrentView = new OOP_EventsManagementSystem.View.Show();
    }

    private void ExecutePartnerCommand(object parameter)
    {
        CurrentView = new Partner();
    }

    private void ExecuteEmployeeCommand(object parameter)
    {
        CurrentView = new OOP_EventsManagementSystem.View.Employee();
    }

    private void ExecuteEquipmentCommand(object parameter)
    {
        CurrentView = new OOP_EventsManagementSystem.View.Equipment();
    }

    private void ExecuteLocationCommand(object parameter)
    {
        CurrentView = new Location();
    }

    // Các phương thức CanExecute kiểm tra quyền của người dùng dựa trên PermissionId
    private bool CanExecuteHomeCommand(object parameter) => PermissionId == 1; // Nút Home luôn có thể thực thi
    private bool CanExecuteEventCommand(object parameter) => PermissionId != 2; // Nút Event luôn có thể thực thi

    // Các nút khác chỉ có thể thực thi nếu PermissionId khác 3
    private bool CanExecuteShowCommand(object parameter) => PermissionId == 1;
    private bool CanExecutePartnerCommand(object parameter) => PermissionId == 1;
    private bool CanExecuteEmployeeCommand(object parameter) => PermissionId != 3;
    private bool CanExecuteEquipmentCommand(object parameter) => PermissionId == 1;
    private bool CanExecuteLocationCommand(object parameter) => PermissionId == 1;

    // Cập nhật quyền truy cập của các lệnh khi PermissionId thay đổi
    private void UpdateCommandsAvailability()
    {
        // Nếu PermissionId = 3, vô hiệu hóa các lệnh ngoài Home và Event
        ((RelayCommand)ShowCommand).RaiseCanExecuteChanged();
        ((RelayCommand)PartnerCommand).RaiseCanExecuteChanged();
        ((RelayCommand)EmployeeCommand).RaiseCanExecuteChanged();
        ((RelayCommand)EquipmentCommand).RaiseCanExecuteChanged();
        ((RelayCommand)LocationCommand).RaiseCanExecuteChanged();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
