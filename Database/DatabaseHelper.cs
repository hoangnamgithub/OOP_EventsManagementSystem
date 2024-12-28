using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace OOP_EventsManagementSystem.Database
{
    public class DatabaseHelper
    {
        private static readonly string ConnectionString;

        static DatabaseHelper()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private const string DatabaseName = "eventmanadb";

        public static void InitializeDatabase()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Check if the database exists and create it if not
                    string checkDbQuery =
                        $"IF DB_ID('{DatabaseName}') IS NULL CREATE DATABASE {DatabaseName};";
                    using (var command = new SqlCommand(checkDbQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine($"Database '{DatabaseName}' verified or created.");

                    // Switch to the newly created or existing database
                    connection.ChangeDatabase(DatabaseName);

                    // Create schemas
                    string[] schemaScripts = GetSchemaCreationScripts();
                    foreach (var script in schemaScripts)
                    {
                        using (var command = new SqlCommand(script, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }

                    // Execute the table creation logic
                    string[] tableScripts = GetTableCreationScripts();
                    foreach (var script in tableScripts)
                    {
                        try
                        {
                            using (var command = new SqlCommand(script, connection))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(
                                $"Error executing script: {script.Substring(0, Math.Min(script.Length, 100))}..."
                            );
                            Console.WriteLine($"Error details: {ex.Message}");
                            throw;
                        }
                    }

                    Console.WriteLine($"Database '{DatabaseName}' initialized successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                throw;
            }
        }

        private static string[] GetSchemaCreationScripts()
        {
            return new string[]
            {
                // Create Schemas
                "IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Events') EXEC('CREATE SCHEMA Events');",
                "IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Shows') EXEC('CREATE SCHEMA Shows');",
                "IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Employees') EXEC('CREATE SCHEMA Employees');",
                "IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Equipments') EXEC('CREATE SCHEMA Equipments');",
                "IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Accounts') EXEC('CREATE SCHEMA Accounts');",
            };
        }

        private static string[] GetTableCreationScripts()
        {
            return new string[]
            {
                @"

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='employee_role' AND xtype='U')
CREATE TABLE Employees.employee_role (
    role_id INT PRIMARY KEY IDENTITY(1,1),
    role_name NVARCHAR(100) NOT NULL,
    salary DECIMAL(10, 2) NOT NULL,
    manager_id INT NULL -- Temporary, without FK
);


IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='employee' AND xtype='U')
CREATE TABLE Employees.employee (
    employee_id INT PRIMARY KEY IDENTITY(1,1),
    full_name NVARCHAR(100) NOT NULL,
    contact NVARCHAR(50),
    role_id INT NOT NULL -- Temporary, without FK
);


-- Check and add FK_employee_role constraint
IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
    WHERE CONSTRAINT_NAME = 'FK_employee_role' AND TABLE_NAME = 'employee'
)
BEGIN
    ALTER TABLE Employees.employee
    ADD CONSTRAINT FK_employee_role FOREIGN KEY (role_id) REFERENCES Employees.employee_role(role_id) ON DELETE CASCADE;
END

-- Check and add FK_employee_manager constraint
IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
    WHERE CONSTRAINT_NAME = 'FK_employee_manager' AND TABLE_NAME = 'employee_role'
)
BEGIN
    ALTER TABLE Employees.employee_role
    ADD CONSTRAINT FK_employee_manager FOREIGN KEY (manager_id) REFERENCES Employees.employee(employee_id) ON DELETE NO ACTION;
END


IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='permission' AND xtype='U')
CREATE TABLE Accounts.permission (
    permission_id INT PRIMARY KEY IDENTITY(1,1),
    permission NVARCHAR(100) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='account' AND xtype='U')
CREATE TABLE Accounts.account (
    account_id INT PRIMARY KEY IDENTITY(1,1),
    email NVARCHAR(100) NOT NULL,
    password NVARCHAR(100) NOT NULL,
    permission_id INT NOT NULL,
    employee_id INT ,
    FOREIGN KEY (permission_id) REFERENCES Accounts.permission(permission_id) ON DELETE CASCADE,
    FOREIGN KEY (employee_id) REFERENCES Employees.employee(employee_id) ON DELETE CASCADE
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='sponsor_tier' AND xtype='U')
CREATE TABLE Events.sponsor_tier (
    sponsor_tier_id INT PRIMARY KEY IDENTITY(1,1),
    tier_name NVARCHAR(100) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='sponsors' AND xtype='U')
CREATE TABLE Events.sponsors (
    sponsor_id INT PRIMARY KEY IDENTITY(1,1),
    sponsor_name NVARCHAR(100) NOT NULL,
    sponsor_details NVARCHAR(MAX)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='event_type' AND xtype='U')
CREATE TABLE Events.event_type (
    event_type_id INT PRIMARY KEY IDENTITY(1,1),
    type_name NVARCHAR(100) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='venue' AND xtype='U')
CREATE TABLE Events.venue (
    venue_id INT PRIMARY KEY IDENTITY(1,1),
    venue_name NVARCHAR(100) NOT NULL,
    cost DECIMAL(10, 2) NOT NULL,
    address NVARCHAR(MAX),
    capacity INT NOT NULL
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='event' AND xtype='U')
CREATE TABLE Events.event (
    event_id INT PRIMARY KEY IDENTITY(1,1),
    event_name NVARCHAR(100) NOT NULL,
    event_description NVARCHAR(MAX),
    expted_attendee INT NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    event_type_id INT NOT NULL,
    venue_id INT NOT NULL,
    FOREIGN KEY (event_type_id) REFERENCES Events.event_type(event_type_id) ON DELETE CASCADE,
    FOREIGN KEY (venue_id) REFERENCES Events.venue(venue_id) ON DELETE CASCADE
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='need' AND xtype='U')
CREATE TABLE Employees.need (
    need_id INT PRIMARY KEY IDENTITY(1,1),
    role_id INT NOT NULL,
    event_id INT NOT NULL,
    quantity INT NOT NULL,
    FOREIGN KEY (role_id) REFERENCES Employees.employee_role(role_id) ON DELETE CASCADE,
    FOREIGN KEY (event_id) REFERENCES Events.event(event_id) ON DELETE CASCADE,
    UNIQUE (role_id, event_id)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='is_sponsor' AND xtype='U')
CREATE TABLE Events.is_sponsor (
    is_sponsor_id INT PRIMARY KEY IDENTITY(1,1),
    event_id INT NOT NULL,
    sponsor_id INT NOT NULL,
    sponsor_tier_id INT NOT NULL,
    FOREIGN KEY (event_id) REFERENCES Events.event(event_id) ON DELETE CASCADE,
    FOREIGN KEY (sponsor_id) REFERENCES Events.sponsors(sponsor_id) ON DELETE CASCADE,
    FOREIGN KEY (sponsor_tier_id) REFERENCES Events.sponsor_tier(sponsor_tier_id) ON DELETE CASCADE,
    UNIQUE (event_id, sponsor_id, sponsor_tier_id)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='genre' AND xtype='U')
CREATE TABLE Shows.genre (
    genre_id INT PRIMARY KEY IDENTITY(1,1),
    genre NVARCHAR(100) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='performer' AND xtype='U')
CREATE TABLE Shows.performer (
    performer_id INT PRIMARY KEY IDENTITY(1,1),
    full_name NVARCHAR(100) NOT NULL,
    contact_detail NVARCHAR(50)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='show' AND xtype='U')
CREATE TABLE Shows.show (
    show_id INT PRIMARY KEY IDENTITY(1,1),
    show_name NVARCHAR(100) NOT NULL,
    cost DECIMAL(10, 2) NOT NULL,
    performer_id INT NOT NULL,
    genre_id INT NOT NULL,
    FOREIGN KEY (performer_id) REFERENCES Shows.performer(performer_id) ON DELETE CASCADE,
    FOREIGN KEY (genre_id) REFERENCES Shows.genre(genre_id) ON DELETE CASCADE
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='show_schedule' AND xtype='U')
CREATE TABLE Shows.show_schedule (
    show_time_id INT PRIMARY KEY IDENTITY(1,1),
    start_date DATE,
    est_duration INT ,
    show_id INT NOT NULL,
    event_id INT NOT NULL,
    FOREIGN KEY (show_id) REFERENCES Shows.show(show_id) ON DELETE CASCADE,
    FOREIGN KEY (event_id) REFERENCES Events.event(event_id) ON DELETE CASCADE,
    UNIQUE (show_id, event_id)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='equipment_type' AND xtype='U')
CREATE TABLE Equipments.equipment_type (
    equip_type_id INT PRIMARY KEY IDENTITY(1,1),
    type_name NVARCHAR(100) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='equipment_name' AND xtype='U')
CREATE TABLE Equipments.equipment_name (
    equip_name_id INT PRIMARY KEY IDENTITY(1,1),
    equip_name NVARCHAR(100) NOT NULL,
    equip_cost DECIMAL(10, 2) NOT NULL,
    equip_type_id INT NOT NULL,
    FOREIGN KEY (equip_type_id) REFERENCES Equipments.equipment_type(equip_type_id) ON DELETE CASCADE
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='required' AND xtype='U')
CREATE TABLE Equipments.required (
    required_id INT PRIMARY KEY IDENTITY(1,1),
    quantity INT NOT NULL,
    event_id INT NOT NULL,
    equip_name_id INT NOT NULL,
    FOREIGN KEY (event_id) REFERENCES Events.event(event_id) ON DELETE CASCADE,
    FOREIGN KEY (equip_name_id) REFERENCES Equipments.equipment_name(equip_name_id) ON DELETE CASCADE,
    UNIQUE (event_id, equip_name_id)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='engaged' AND xtype='U')
CREATE TABLE Employees.engaged (
    engaged_id INT PRIMARY KEY IDENTITY(1,1),
    account_id INT NOT NULL,
    event_id INT NOT NULL,
    FOREIGN KEY (account_id) REFERENCES Accounts.account(account_id) ON DELETE CASCADE,
    FOREIGN KEY (event_id) REFERENCES Events.event(event_id) ON DELETE CASCADE,
    UNIQUE (account_id, event_id)
);
        ",
            };
        }
    }
}
