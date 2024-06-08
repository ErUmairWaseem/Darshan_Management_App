using DarshanManagementApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DarshanManagementApp.Managers
{
    public class UserManager
    {
        private string connectionString;

        public UserManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool Signup(string username, string password, string email, string fullName, string phoneNumber, string address)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string checkUsernameQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    using (var checkUsernameCommand = new MySqlCommand(checkUsernameQuery, connection))
                    {
                        checkUsernameCommand.Parameters.AddWithValue("@Username", username);
                        int count = Convert.ToInt32(checkUsernameCommand.ExecuteScalar());
                        if (count > 0)
                        {
                            Console.WriteLine("Username already exists. Please choose a different username.");
                            return false;
                        }
                    }

                    string createUserQuery = "INSERT INTO Users (Username, Password, Email, FullName, PhoneNumber, Address) " +
                                             "VALUES (@Username, @Password, @Email, @FullName, @PhoneNumber, @Address)";
                    using (var createUserCommand = new MySqlCommand(createUserQuery, connection))
                    {
                        createUserCommand.Parameters.AddWithValue("@Username", username);
                        createUserCommand.Parameters.AddWithValue("@Password", password);
                        createUserCommand.Parameters.AddWithValue("@Email", email);
                        createUserCommand.Parameters.AddWithValue("@FullName", fullName);
                        createUserCommand.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        createUserCommand.Parameters.AddWithValue("@Address", address);
                        createUserCommand.ExecuteNonQuery();
                    }

                    Console.WriteLine("Signup successful! You can now login.");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error signing up: {ex.Message}");
                    throw;
                }
            }
        }

        public bool AdminSignup(string username, string password)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string checkUsernameQuery = "SELECT COUNT(*) FROM Admins WHERE Username = @Username";
                    using (var checkUsernameCommand = new MySqlCommand(checkUsernameQuery, connection))
                    {
                        checkUsernameCommand.Parameters.AddWithValue("@Username", username);
                        int count = Convert.ToInt32(checkUsernameCommand.ExecuteScalar());
                        if (count > 0)
                        {
                            Console.WriteLine("Admin username already exists. Please choose a different username.");
                            return false;
                        }
                    }

                    string createAdminQuery = "INSERT INTO Admins (Username, Password) " +
                                              "VALUES (@Username, @Password)";
                    using (var createAdminCommand = new MySqlCommand(createAdminQuery, connection))
                    {
                        createAdminCommand.Parameters.AddWithValue("@Username", username);
                        createAdminCommand.Parameters.AddWithValue("@Password", password);
                        createAdminCommand.ExecuteNonQuery();
                    }

                    Console.WriteLine("Admin signup successful! You can now login.");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error signing up admin: {ex.Message}");
                    throw;
                }
            }
        }

        public User Login(string username, string password)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectUserQuery = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
                    using (var selectUserCommand = new MySqlCommand(selectUserQuery, connection))
                    {
                        selectUserCommand.Parameters.AddWithValue("@Username", username);
                        selectUserCommand.Parameters.AddWithValue("@Password", password);

                        using (var reader = selectUserCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                User user = new User
                                {
                                    UserId = reader.GetInt32("UserId"),
                                    Username = reader.GetString("Username"),
                                    Password = reader.GetString("Password"),
                                    Email = reader.GetString("Email"),
                                    FullName = reader.GetString("FullName"),
                                    PhoneNumber = reader.GetString("PhoneNumber"),
                                    Address = reader.GetString("Address")
                                };

                                Console.WriteLine("Login successful!");
                                return user;
                            }
                        }
                    }

                    Console.WriteLine("Invalid username or password.");
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error logging in: {ex.Message}");
                    throw;
                }
            }
        }

        public Admin AdminLogin(string username, string password)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectAdminQuery = "SELECT * FROM Admins WHERE Username = @Username AND Password = @Password";
                    using (var selectAdminCommand = new MySqlCommand(selectAdminQuery, connection))
                    {
                        selectAdminCommand.Parameters.AddWithValue("@Username", username);
                        selectAdminCommand.Parameters.AddWithValue("@Password", password);

                        using (var reader = selectAdminCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Admin admin = new Admin
                                {
                                    AdminId = reader.GetInt32("AdminId"),
                                    Username = reader.GetString("Username"),
                                    Password = reader.GetString("Password")
                                };

                                Console.WriteLine("Admin login successful!");
                                return admin;
                            }
                        }
                    }

                    Console.WriteLine("Invalid admin username or password.");
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error logging in as admin: {ex.Message}");
                    throw;
                }
            }
        }

        public void UpdateUser(int userId, string newUsername, string newPassword, string newEmail, string newFullName, string newPhoneNumber, string newAddress)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string updateQuery = "UPDATE Users SET Username = @Username, Password = @Password, Email = @Email, FullName = @FullName, PhoneNumber = @PhoneNumber, Address = @Address WHERE UserId = @UserId";
                    using (var command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", newUsername);
                        command.Parameters.AddWithValue("@Password", newPassword);
                        command.Parameters.AddWithValue("@Email", newEmail);
                        command.Parameters.AddWithValue("@FullName", newFullName);
                        command.Parameters.AddWithValue("@PhoneNumber", newPhoneNumber);
                        command.Parameters.AddWithValue("@Address", newAddress);
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine("User updated successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating user: {ex.Message}");
                    throw;
                }
            }
        }

        public void DeleteUser(int userId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM Users WHERE UserId = @UserId";
                    using (var command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            Console.WriteLine("No user found with the specified ID.");
                        }
                        else
                        {
                            Console.WriteLine("User deleted successfully.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting user: {ex.Message}");
                    throw;
                }
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = "SELECT UserId, Username, Email, FullName, PhoneNumber, Address FROM Users";
                    using (var command = new MySqlCommand(selectQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    UserId = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    FullName = reader.GetString(3),
                                    PhoneNumber = reader.GetString(4),
                                    Address = reader.GetString(5)
                                };
                                users.Add(user);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving users: {ex.Message}");
                    throw;
                }
            }
            return users;
        }

        public List<Admin> GetAllAdmins()
        {
            List<Admin> admins = new List<Admin>();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = "SELECT AdminId, Username FROM Admins";
                    using (var command = new MySqlCommand(selectQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Admin admin = new Admin
                                {
                                    AdminId = reader.GetInt32(0),
                                    Username = reader.GetString(1)
                                };
                                admins.Add(admin);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving admins: {ex.Message}");
                    throw;
                }
            }
            return admins;
        }

        public void ViewAllUsers()
        {
            List<User> users = GetAllUsers();
            if (users.Count == 0)
            {
                Console.WriteLine("No users found.");
                return;
            }

            Console.WriteLine("Users:");
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine("| ID  | Username      | Email                   | Full Name       |");
            Console.WriteLine("-------------------------------------------------------------------");
            foreach (var user in users)
            {
                Console.WriteLine($"| {user.UserId,-3} | {user.Username,-13} | {user.Email,-17} | {user.FullName,-15} |");
            }
            Console.WriteLine("-------------------------------------------------------------------");
        }

        public void ViewAllAdmins()
        {
            List<Admin> admins = GetAllAdmins();
            if (admins.Count == 0)
            {
                Console.WriteLine("No admins found.");
                return;
            }

            Console.WriteLine("Admins:");
            Console.WriteLine("------------------------");
            Console.WriteLine("| ID  | Username       |");
            Console.WriteLine("------------------------");
            foreach (var admin in admins)
            {
                Console.WriteLine($"| {admin.AdminId,-3} | {admin.Username,-14} |");
            }
            Console.WriteLine("------------------------");
        }
    }
}
