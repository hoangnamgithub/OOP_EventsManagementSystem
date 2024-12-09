using OOP_EventsManagementSystem.Database;
using System.Configuration;
using System.Data;
using System.Windows;

namespace OOP_EventsManagementSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Ensure the database and tables exist before running the app
            DatabaseHelper.InitializeDatabase();
        }
    }

}
