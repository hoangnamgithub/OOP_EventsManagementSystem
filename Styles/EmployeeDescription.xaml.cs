using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OOP_EventsManagementSystem.Model;
using OOP_EventsManagementSystem.ViewModel;

namespace OOP_EventsManagementSystem.Styles
{
    /// <summary>
    /// Interaction logic for EmployeeDescription.xaml
    /// </summary>
    public partial class EmployeeDescription : Window
    {
        private bool isTextChanging;

        public EmployeeDescription()
        {
            InitializeComponent();
            DataContext = new EmployeeVM(new EventManagementDbContext());
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (
                e.ButtonState == MouseButtonState.Pressed
                && this.WindowState != WindowState.Maximized
            )
            {
                this.DragMove();
            }
        }

        private void txtbox_empName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is EmployeeVM viewModel)
            {
                try
                {
                    if (viewModel.GeneratePasswordCommand?.CanExecute(null) == true)
                    {
                        viewModel.GeneratePasswordCommand.Execute(null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"An error occurred: {ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private void Txtbox_empContact_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void Txtbox_empContact_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isTextChanging)
                return;

            var textBox = sender as TextBox;
            if (textBox == null)
                return;

            var text = new string(textBox.Text.Where(char.IsDigit).ToArray());

            if (text.Length > 10)
            {
                text = text.Substring(0, 10);
            }

            isTextChanging = true;
            textBox.Text = FormatAsPhoneNumber(text);
            textBox.SelectionStart = textBox.Text.Length;
            isTextChanging = false;
        }

        private static string FormatAsPhoneNumber(string text)
        {
            if (string.IsNullOrEmpty(text)) // Avoid formatting if text is empty
                return string.Empty;

            if (text.Length <= 3)
                return $"({text}";
            if (text.Length <= 6)
                return $"({text.Substring(0, 3)}) {text.Substring(3)}";
            return $"({text.Substring(0, 3)}) {text.Substring(3, 3)}-{text.Substring(6)}";
        }

        private static bool IsTextAllowed(string text)
        {
            return text.All(char.IsDigit);
        }
    }
}
