using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using OOP_EventsManagementSystem.Utilities;

namespace OOP_EventsManagementSystem.ViewModel
{
    public class MainWindowVM : ViewModelBase
    {
        private string _someProperty;

        public string SomeProperty
        {
            get { return _someProperty; }
            set
            {
                if (_someProperty != value)
                {
                    _someProperty = value;
                    OnPropertyChanged(nameof(SomeProperty));
                }
            }
        }
    }
}
