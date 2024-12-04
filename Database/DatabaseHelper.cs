using System;
using System.Data.SqlClient;
using Faker;  // Assuming you are using Faker.Net
using System.IO;

public static class DatabaseHelper
{
    private static string connectionString = "Server=.\\SQLEXPRESS;Database=master;Trusted_Connection=True;";  // Set your connection string

    public static void CreateDatabaseIfNotExists()
    {
        try
        {
            // Check if the database exists
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Query to check if the database exists
                string checkDbQuery = "SELECT database_id FROM sys.databases WHERE Name = 'EventManagementDB'";
                using (SqlCommand command = new SqlCommand(checkDbQuery, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result == null)  // Database does not exist, create it
                    {
                        CreateDatabase(connection);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void CreateDatabase(SqlConnection connection)
    {
        try
        {
            // Create database
            string createDbQuery = "CREATE DATABASE EventManagementDB";
            using (SqlCommand command = new SqlCommand(createDbQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Database 'EventManagementDB' created successfully.");

            // Now create the tables after database is created
            CreateTables();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while creating the database: {ex.Message}");
        }
    }

    private static void CreateTables()
    {
        try
        {
            // Set the connection string for the newly created database
            string dbConnectionString = @"Server=.\\SQLEXPRESS;Database=EventManagementDB;Integrated Security=True;";  // Update with your connection details
            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                string script = File.ReadAllText("CreateTablesScript.sql");  // Load the SQL script from a file
                using (SqlCommand command = new SqlCommand(script, connection))
                {
                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Tables created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while creating tables: {ex.Message}");
        }
    }
}


/*
public class DatabaseHelper
{
    private string connectionString = "Server=.\\SQLEXPRESS;Database=EventManagementDB;Trusted_Connection=True;";  // Modify as needed

    // This method checks if the database is empty, and inserts fake data if it is
    public void InsertFakeDataIfDatabaseEmpty()
    {
        if (IsDatabaseEmpty())
        {
            InsertFakeData();
        }
    }

    // Method to check if the database is empty
    private bool IsDatabaseEmpty()
    {
        bool isEmpty = false;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT COUNT(*) FROM Events.event";  // You can change the table here
            SqlCommand command = new SqlCommand(query, connection);
            int count = (int)command.ExecuteScalar();
            isEmpty = count == 0;
        }
        return isEmpty;
    }

    // Method to insert fake data into the database
    private void InsertFakeData()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Insert fake data into Events table
            InsertEventData(connection);
            InsertPartnerData(connection);
            InsertRoleData(connection);
            InsertEmployeeData(connection);
            InsertShowData(connection);
            InsertPerformerData(connection);
            InsertEquipmentData(connection);
            InsertRequiredData(connection);
            InsertEngagedData(connection);
        }
    }

    private void InsertEventData(SqlConnection connection)
    {
        string eventName = Company.Name() + " Event";
        string eventDescription = "A fun and exciting event hosted by " + Company.Name() + ".";
        int eventTypeId = new Random().Next(1, 5);  // Random event type ID
        DateTime eventStartTime = DateTime.Now.AddDays(10);
        DateTime eventEndTime = eventStartTime.AddDays(2);

        string query = "INSERT INTO Events (event_name, event_type_id, event_description, start_time, end_time) VALUES (@eventName, @eventTypeId, @eventDescription, @eventStartTime, @eventEndTime)";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@eventName", eventName);
            command.Parameters.AddWithValue("@eventTypeId", eventTypeId);
            command.Parameters.AddWithValue("@eventDescription", eventDescription);
            command.Parameters.AddWithValue("@eventStartTime", eventStartTime);
            command.Parameters.AddWithValue("@eventEndTime", eventEndTime);
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Fake event data inserted successfully.");
    }

    private void InsertPartnerData(SqlConnection connection)
    {
        string partnerName = Company.Name();
        string partnerDetails = "A valuable partner for this event.";
        string query = "INSERT INTO Partners (partner_name, partner_details) VALUES (@partnerName, @partnerDetails)";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@partnerName", partnerName);
            command.Parameters.AddWithValue("@partnerDetails", partnerDetails);
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Fake partner data inserted successfully.");
    }

    private void InsertRoleData(SqlConnection connection)
    {
        string roleName = "Manager";
        string query = "INSERT INTO Roles (role_name) VALUES (@roleName)";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@roleName", roleName);
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Fake role data inserted successfully.");
    }

    private void InsertEmployeeData(SqlConnection connection)
    {
        string employeeName = Name.FullName();
        string employeeContact = Phone.Number();
        string query = "INSERT INTO Employees (employee_name, employee_contact) VALUES (@employeeName, @employeeContact)";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@employeeName", employeeName);
            command.Parameters.AddWithValue("@employeeContact", employeeContact);
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Fake employee data inserted successfully.");
    }

    private void InsertShowData(SqlConnection connection)
    {
        string showName = "Show " + Company.Name();
        string showDescription = "A thrilling show hosted by " + Company.Name() + ".";
        DateTime showStartTime = DateTime.Now.AddDays(5);
        DateTime showEndTime = showStartTime.AddHours(3);
        string query = "INSERT INTO Shows (show_name, show_description, event_id, start_time, end_time) VALUES (@showName, @showDescription, 1, @showStartTime, @showEndTime)";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@showName", showName);
            command.Parameters.AddWithValue("@showDescription", showDescription);
            command.Parameters.AddWithValue("@showStartTime", showStartTime);
            command.Parameters.AddWithValue("@showEndTime", showEndTime);
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Fake show data inserted successfully.");
    }

    private void InsertPerformerData(SqlConnection connection)
    {
        string performerName = Name.FullName();
        string performerGenre = "Music";
        string query = "INSERT INTO Performers (performer_name, genre, contact_detail) VALUES (@performerName, @performerGenre, @performerContact)";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@performerName", performerName);
            command.Parameters.AddWithValue("@performerGenre", performerGenre);
            command.Parameters.AddWithValue("@performerContact", Phone.Number());
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Fake performer data inserted successfully.");
    }

    private void InsertEquipmentData(SqlConnection connection)
    {
        string equipmentName = "Sound System";
        string query = "INSERT INTO Equipment (equipment_name, equipment_type_id, available) VALUES (@equipmentName, 1, 10)";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@equipmentName", equipmentName);
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Fake equipment data inserted successfully.");
    }

    private void InsertRequiredData(SqlConnection connection)
    {
        string query = "INSERT INTO Required (show_id, equipment_id, quantity, cost) VALUES (1, 1, 2, 200)";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Equipment linked to show successfully.");
    }

    private void InsertEngagedData(SqlConnection connection)
    {
        string query = "INSERT INTO Engaged (show_id, start_time, end_time, cost) VALUES (1, @showStartTime, @showEndTime, 500)";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@showStartTime", DateTime.Now.AddDays(5));
            command.Parameters.AddWithValue("@showEndTime", DateTime.Now.AddDays(5).AddHours(3));
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Employee engaged with show successfully.");
    }
}
*/