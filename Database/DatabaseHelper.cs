using System;
using System.Data.SqlClient;

namespace OOP_EventsManagementSystem.Database
{
    public class DatabaseHelper
    {
        private const string ConnectionString = "Server=.\\sqlexpress;Integrated Security=true;";
        private const string DatabaseName = "EventManagementDB";

        public static void InitializeDatabase()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Check if the database exists and create it if not
                    string checkDbQuery = $"IF DB_ID('{DatabaseName}') IS NULL CREATE DATABASE {DatabaseName};";
                    using (var command = new SqlCommand(checkDbQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine($"Database '{DatabaseName}' verified or created.");

                    // Switch to the newly created or existing database
                    connection.ChangeDatabase(DatabaseName);

                    // Execute the database schema creation logic
                    string[] schemaScripts = GetSchemaCreationScripts();
                    foreach (var script in schemaScripts)
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
                            Console.WriteLine($"Error executing script: {script.Substring(0, Math.Min(script.Length, 100))}...");
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

                // Events Schema
                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Events].[partner_role]') AND type in (N'U'))
                  CREATE TABLE Events.partner_role (
                      partner_role_id INT PRIMARY KEY IDENTITY(1,1),
                      role_name NVARCHAR(100) NOT NULL
                  );",

                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Events].[event_type]') AND type in (N'U'))
                  CREATE TABLE Events.event_type (
                      event_type_id INT PRIMARY KEY IDENTITY(1,1),
                      type_name NVARCHAR(100) NOT NULL
                  );",

                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Events].[partner]') AND type in (N'U'))
                  CREATE TABLE Events.partner (
                      partner_id INT PRIMARY KEY IDENTITY(1,1),
                      partner_name NVARCHAR(200) NOT NULL,
                      partner_description NVARCHAR(MAX) NULL
                  );",

                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Events].[location]') AND type in (N'U'))
                  CREATE TABLE Events.location (
                      location_id INT PRIMARY KEY IDENTITY(1,1),
                      location_name NVARCHAR(200) NOT NULL,
                      address NVARCHAR(MAX) NOT NULL,
                      cost DECIMAL(10, 2) NOT NULL CHECK (cost >= 0)
                  );",

                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Events].[event]') AND type in (N'U'))
                  CREATE TABLE Events.event (
                      event_id INT PRIMARY KEY IDENTITY(1,1),
                      event_name NVARCHAR(200) NOT NULL,
                      event_type_id INT NOT NULL,
                      event_description NVARCHAR(MAX) NULL,
                      location_id INT NULL,
                      start_time DATETIME NOT NULL,
                      end_time DATETIME NOT NULL,
                      FOREIGN KEY (event_type_id) REFERENCES Events.event_type(event_type_id) ON DELETE CASCADE,
                      FOREIGN KEY (location_id) REFERENCES Events.location(location_id) ON DELETE SET NULL
                  );",

                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Events].[is_partner]') AND type in (N'U'))
                  CREATE TABLE Events.is_partner (
                      is_partner_id INT PRIMARY KEY IDENTITY(1,1),
                      event_id INT NOT NULL,
                      partner_id INT NOT NULL,
                      partner_role_id INT NOT NULL,
                      FOREIGN KEY (event_id) REFERENCES Events.event(event_id) ON DELETE CASCADE,
                      FOREIGN KEY (partner_id) REFERENCES Events.partner(partner_id) ON DELETE CASCADE,
                      FOREIGN KEY (partner_role_id) REFERENCES Events.partner_role(partner_role_id)
                  );",

                // Shows Schema
                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Shows].[equipment_type]') AND type in (N'U'))
                  CREATE TABLE Shows.equipment_type (
                      equipment_type_id INT PRIMARY KEY IDENTITY(1,1),
                      equipment_type_name NVARCHAR(100) NOT NULL
                  );",

                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Shows].[equipment]') AND type in (N'U'))
                  CREATE TABLE Shows.equipment (
                     equipment_id INT PRIMARY KEY IDENTITY(1,1),
                     equipment_name NVARCHAR(200) NOT NULL,
                     equipment_type_id INT NOT NULL,
                     available BIT NOT NULL,
                     quantity INT NOT NULL DEFAULT 0,
                     cost DECIMAL(10, 2) NOT NULL CHECK (cost >= 0),
                     FOREIGN KEY (equipment_type_id) REFERENCES Shows.equipment_type(equipment_type_id) ON DELETE CASCADE
                  );",

                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Shows].[show]') AND type in (N'U'))
                  CREATE TABLE Shows.show (
                      show_id INT PRIMARY KEY IDENTITY(1,1),
                      show_name NVARCHAR(200) NOT NULL,
                      show_description NVARCHAR(MAX) NULL,
                      event_id INT NOT NULL,
                      start_time DATETIME NOT NULL,
                      end_time DATETIME NOT NULL,
                      FOREIGN KEY (event_id) REFERENCES Events.event(event_id) ON DELETE CASCADE
                  );",

                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Shows].[required]') AND type in (N'U'))
                  CREATE TABLE Shows.required (
                      required_id INT PRIMARY KEY IDENTITY(1,1),
                      show_id INT NOT NULL,
                      equipment_id INT NOT NULL,
                      quantity INT NOT NULL CHECK (quantity >= 0),
                      FOREIGN KEY (show_id) REFERENCES Shows.show(show_id) ON DELETE CASCADE,
                      FOREIGN KEY (equipment_id) REFERENCES Shows.equipment(equipment_id) ON DELETE CASCADE
                  );",

                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Shows].[performer]') AND type in (N'U'))
                  CREATE TABLE Shows.performer (
                      performer_id INT PRIMARY KEY IDENTITY(1,1),
                      performer_name NVARCHAR(200) NOT NULL,
                      genre NVARCHAR(100) NOT NULL,
                      performer_contact NVARCHAR(MAX) NULL
                  );",

                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Shows].[participate]') AND type in (N'U'))
                  CREATE TABLE Shows.participate (
                      participate_id INT PRIMARY KEY IDENTITY(1,1),
                      show_id INT NOT NULL,
                      performer_id INT NOT NULL,
                      cost DECIMAL(10, 2) NOT NULL CHECK (cost >= 0),
                      FOREIGN KEY (show_id) REFERENCES Shows.show(show_id) ON DELETE CASCADE,
                      FOREIGN KEY (performer_id) REFERENCES Shows.performer(performer_id) ON DELETE CASCADE
                  );",

                // Employees Schema
                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Employees].[role]') AND type in (N'U'))
                  CREATE TABLE Employees.role (
                      role_id INT PRIMARY KEY IDENTITY(1,1),
                      role_name NVARCHAR(100) NOT NULL
                  );",

                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Employees].[employee]') AND type in (N'U'))
                  CREATE TABLE Employees.employee (
                      employee_id INT PRIMARY KEY IDENTITY(1,1),
                      employee_name NVARCHAR(200) NOT NULL,
                      employee_contact NVARCHAR(MAX) NULL
                  );",

                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Employees].[has_role]') AND type in (N'U'))
                  CREATE TABLE Employees.has_role (
                      has_role_id INT PRIMARY KEY IDENTITY(1,1),
                      employee_id INT NOT NULL,
                      role_id INT NOT NULL,
                      FOREIGN KEY (employee_id) REFERENCES Employees.employee(employee_id) ON DELETE CASCADE,
                      FOREIGN KEY (role_id) REFERENCES Employees.role(role_id) ON DELETE CASCADE
                  );",

                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Employees].[engaged]') AND type in (N'U'))
                  CREATE TABLE Employees.engaged (
                      engaged_id INT PRIMARY KEY IDENTITY(1,1),
                      show_id INT NOT NULL,
                      employee_id INT NOT NULL,
                      start_time DATETIME NOT NULL,
                      end_time DATETIME NOT NULL,
                      FOREIGN KEY (show_id) REFERENCES Shows.show(show_id) ON DELETE CASCADE,
                      FOREIGN KEY (employee_id) REFERENCES Employees.employee(employee_id) ON DELETE CASCADE
                  );"
            };
        }
    }
}
