using System;
using System.Windows.Input;

namespace OOP_EventsManagementSystem.ViewModel
{
    // Lớp RelayCommand giúp thực thi ICommand
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;        // Thực thi hành động khi lệnh được gọi
        private readonly Func<object, bool> _canExecute; // Kiểm tra nếu lệnh có thể được thực thi hay không

        // Constructor nhận vào hành động thực thi
        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        // Constructor nhận vào hành động và điều kiện kiểm tra lệnh
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));  // Bảo đảm không phải null
            _canExecute = canExecute;
        }

        // Kiểm tra điều kiện lệnh có thể thực thi hay không
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;  // Nếu không có điều kiện, mặc định cho phép thực thi
        }

        // Thực thi hành động khi lệnh được gọi
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        // Event để thông báo khi điều kiện lệnh thay đổi
        public event EventHandler CanExecuteChanged;
    }
}
