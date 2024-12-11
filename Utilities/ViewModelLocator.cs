using OOP_EventsManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EventsManagementSystem.Utilities
{
    public class ViewModelLocator
    {
        // Property để cung cấp MainWindowViewModel
        public MainWindowVM Main => new MainWindowVM();
        public EventVM EventVM => new EventVM();
    }
}

