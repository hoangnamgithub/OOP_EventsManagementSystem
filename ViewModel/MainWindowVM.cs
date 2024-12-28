using System.ComponentModel;
using System.Windows.Input;
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.View;

public class MainWindowVM : INotifyPropertyChanged
{
    private object _currentView;
    private int _permissionId; // Thêm thuộc tính PermissionId
    private readonly EventManagementDbContext _context; // Khai báo DbContext

    private Lazy<Home> _homeView;
    private Lazy<Partner> _partnerView;
    private Lazy<Location> _locationView;
    private Lazy<OOP_EventsManagementSystem.View.Event> _eventView;
    private Lazy<OOP_EventsManagementSystem.View.Show> _showView;
    private Lazy<OOP_EventsManagementSystem.View.Employee> _employeeView;
    private Lazy<OOP_EventsManagementSystem.View.Equipment> _equipmentView;
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
            // Đặt view mặc định là Home
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



        _homeView = new Lazy<Home>(() => new Home());
        _eventView = new Lazy<OOP_EventsManagementSystem.View.Event>(
            () => new OOP_EventsManagementSystem.View.Event()
        );
        _showView = new Lazy<OOP_EventsManagementSystem.View.Show>(
            () => new OOP_EventsManagementSystem.View.Show()
        );
        _partnerView = new Lazy<Partner>(() => new Partner());
        _employeeView = new Lazy<OOP_EventsManagementSystem.View.Employee>(
            () => new OOP_EventsManagementSystem.View.Employee()
        );
        _equipmentView = new Lazy<OOP_EventsManagementSystem.View.Equipment>(
            () => new OOP_EventsManagementSystem.View.Equipment()
        );
        _locationView = new Lazy<Location>(() => new Location());
        
    }

    private void ExecuteHomeCommand(object parameter)
    {
        CurrentView = _homeView.Value;
    }

    private void ExecuteEventCommand(object parameter)
    {
        CurrentView = _eventView.Value;
    }

    private void ExecuteShowCommand(object parameter)
    {
        CurrentView = _showView.Value;
    }

    private void ExecutePartnerCommand(object parameter)
    {
        CurrentView = _partnerView.Value;
    }

    private void ExecuteEmployeeCommand(object parameter)
    {
        CurrentView = _employeeView.Value;
    }

    private void ExecuteEquipmentCommand(object parameter)
    {
        CurrentView = _equipmentView.Value;
    }

    private void ExecuteLocationCommand(object parameter)
    {
        CurrentView = _locationView.Value;
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
