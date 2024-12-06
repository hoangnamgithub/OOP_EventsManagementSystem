﻿using System.Configuration;
using System.Data;
using System.Windows;
using OOP_EventsManagementSystem.Database;

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
            SeedData.SeedEventTypes(); // Populate fake data for event types
        }
    }
}
