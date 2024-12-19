using OOP_EventsManagementSystem.Styles;
using OOP_EventsManagementSystem.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OOP_EventsManagementSystem.ViewModel
{
    class ShowVM : INotifyPropertyChanged
    {
        
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        

        public ShowVM() 
        {
            
            EditCommand = new RelayCommand(EditShowDetails);
            DeleteCommand = new RelayCommand(DeleteShowDetails);
            
        }

        
        private void EditShowDetails(object obj)
        {
            
        }
        private void DeleteShowDetails(object obj)
        {
            
        }
       

        // Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
