using DarshanManagementApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DarshanManagementApp.Managers
{
    public class DarshanManager
    {
        private string connectionString;

        public DarshanManager(string connectionString)
        {
            this.connectionString = connectionString;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            string serverConnectionString = "server=localhost;user=root;password=23Cdac@23;";
            using (var connection = new MySqlConnection(serverConnectionString))
            {
                try
                {
                    connection.Open();
                    string createDatabaseQuery = "CREATE DATABASE IF NOT EXISTS darshan_appointments;";
                    using (var command = new MySqlCommand(createDatabaseQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating database: {ex.Message}");
                    throw;
                }
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string createUsersTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Users (
                            UserId INT AUTO_INCREMENT PRIMARY KEY,
                            Username VARCHAR(255) NOT NULL UNIQUE,
                            Password VARCHAR(255) NOT NULL,
                            Email VARCHAR(255),
                            FullName VARCHAR(255),
                            PhoneNumber VARCHAR(20),
                            Address TEXT
                        );
                    ";

                    string createAdminsTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Admins (
                            AdminId INT AUTO_INCREMENT PRIMARY KEY,
                            Username VARCHAR(255) NOT NULL UNIQUE,
                            Password VARCHAR(255) NOT NULL
                        );
                    ";

                    string createAppointmentsTableQuery = @"
                        CREATE TABLE IF NOT EXISTS DarshanAppointments (
                            AppointmentId INT AUTO_INCREMENT PRIMARY KEY,
                            DevoteeName VARCHAR(255) NOT NULL,
                            AppointmentDate DATETIME NOT NULL,
                            UserId INT NOT NULL,
                            FOREIGN KEY (UserId) REFERENCES Users(UserId)
                        );
                    ";

                    using (var command = new MySqlCommand(createUsersTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (var command = new MySqlCommand(createAdminsTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (var command = new MySqlCommand(createAppointmentsTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error initializing database: {ex.Message}");
                    throw;
                }
            }
        }

        public void AddAppointment(DarshanAppointment appointment, int userId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string insertQuery = @"
                        INSERT INTO DarshanAppointments (DevoteeName, AppointmentDate, UserId)
                        VALUES (@DevoteeName, @AppointmentDate, @UserId);
                    ";

                    using (var command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@DevoteeName", appointment.DevoteeName);
                        command.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding appointment: {ex.Message}");
                    throw;
                }
            }
        }

        public void CancelAppointment(int appointmentId, int userId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string deleteQuery = @"
                        DELETE FROM DarshanAppointments
                        WHERE AppointmentId = @AppointmentId AND UserId = @UserId;
                    ";

                    using (var command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@AppointmentId", appointmentId);
                        command.Parameters.AddWithValue("@UserId", userId);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            Console.WriteLine("No appointment found with the specified ID and user ID.");
                        }
                        else
                        {
                            Console.WriteLine("Appointment canceled successfully.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error cancelling appointment: {ex.Message}");
                    throw;
                }
            }
        }

        public void ViewAppointments(int userId)
        {
            List<DarshanAppointment> appointments = new List<DarshanAppointment>();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = @"
                        SELECT AppointmentId, DevoteeName, AppointmentDate
                        FROM DarshanAppointments
                        WHERE UserId = @UserId;
                    ";

                    using (var command = new MySqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DarshanAppointment appointment = new DarshanAppointment
                                {
                                    AppointmentId = reader.GetInt32(0),
                                    DevoteeName = reader.GetString(1),
                                    AppointmentDate = reader.GetDateTime(2)
                                };
                                appointments.Add(appointment);
                            }
                        }
                    }

                    if (appointments.Count == 0)
                    {
                        Console.WriteLine("No appointments found for the specified user.");
                        return;
                    }

                    Console.WriteLine("Appointments:");
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("| ID  |  Devotee Name  |    Appointment Date     |");
                    Console.WriteLine("--------------------------------------------------");
                    foreach (var appointment in appointments)
                    {
                        Console.WriteLine($"| {appointment.AppointmentId,-3} | {appointment.DevoteeName,-14} | {appointment.AppointmentDate,-23} |");
                    }
                    Console.WriteLine("-------------------------------------------------");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error viewing appointments: {ex.Message}");
                    throw;
                }
            }
        }

        public void ViewAppointmentsByDate(DateTime date)
        {
            List<DarshanAppointment> appointments = new List<DarshanAppointment>();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = @"
                        SELECT AppointmentId, DevoteeName, AppointmentDate, UserId
                        FROM DarshanAppointments
                        WHERE DATE(AppointmentDate) = @Date;
                    ";

                    using (var command = new MySqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Date", date);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DarshanAppointment appointment = new DarshanAppointment
                                {
                                    AppointmentId = reader.GetInt32(0),
                                    DevoteeName = reader.GetString(1),
                                    AppointmentDate = reader.GetDateTime(2)
                                };
                                appointments.Add(appointment);
                            }
                        }
                    }

                    if (appointments.Count == 0)
                    {
                        Console.WriteLine($"No appointments found for the date {date.ToString("yyyy-MM-dd")}.");
                        return;
                    }

                    Console.WriteLine($"Appointments on {date.ToString("yyyy-MM-dd")}:");
                    Console.WriteLine("-------------------------------------------------");
                    Console.WriteLine("| ID  | Devotee Name   | Appointment Date       |");
                    Console.WriteLine("-------------------------------------------------");
                    foreach (var appointment in appointments)
                    {
                        Console.WriteLine($"| {appointment.AppointmentId,-3} | {appointment.DevoteeName,-14} | {appointment.AppointmentDate,-22} |");
                    }
                    Console.WriteLine("-------------------------------------------------");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error viewing appointments by date: {ex.Message}");
                    throw;
                }
            }
        }

        public void ViewAllAppointments()
        {
            List<DarshanAppointment> appointments = new List<DarshanAppointment>();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = @"
                        SELECT AppointmentId, DevoteeName, AppointmentDate, UserId
                        FROM DarshanAppointments;
                    ";

                    using (var command = new MySqlCommand(selectQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DarshanAppointment appointment = new DarshanAppointment
                                {
                                    AppointmentId = reader.GetInt32(0),
                                    DevoteeName = reader.GetString(1),
                                    AppointmentDate = reader.GetDateTime(2)
                                };
                                appointments.Add(appointment);
                            }
                        }
                    }

                    if (appointments.Count == 0)
                    {
                        Console.WriteLine("No appointments found.");
                        return;
                    }

                    Console.WriteLine("Appointments:");
                    Console.WriteLine("-------------------------------------------------");
                    Console.WriteLine("| ID  | Devotee Name   | Appointment Date       |");
                    Console.WriteLine("-------------------------------------------------");
                    foreach (var appointment in appointments)
                    {
                        Console.WriteLine($"| {appointment.AppointmentId,-3} | {appointment.DevoteeName,-14} | {appointment.AppointmentDate,-22} |");
                    }
                    Console.WriteLine("-------------------------------------------------");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error viewing all appointments: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
