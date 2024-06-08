using DarshanManagementApp.Managers;
using DarshanManagementApp.Models;
using System;
using System.Collections.Generic;

namespace DarshanManagementApp
{
    class Program
    {
        static void Main()
        {
            string connectionString = "server=localhost;user=root;password=23Cdac@23;database=darshan_appointments;";
            DarshanManager darshanManager = new DarshanManager(connectionString);
            UserManager userManager = new UserManager(connectionString);

            // Create an admin user
            Admin adminUser = new Admin
            {
                Username = "admin",
                Password = "adminpass"
            };

            // Create the admin user in the database
            userManager.AdminSignup(adminUser.Username, adminUser.Password);

            // In a real-world scenario, this list would be populated from the database.
            List<Admin> admins = new List<Admin> { adminUser };

            AdminManager adminManager = new AdminManager(admins);
            while (true)
            {
                Console.WriteLine("1. Login");
                Console.WriteLine("2. User Sign Up");
                Console.WriteLine("3. Exit");

                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        // Login
                        PerformLogin(userManager, adminManager, darshanManager);
                        break;
                    case 2:
                        // User Sign Up
                        PerformSignUp(userManager);
                        break;
                    case 3:
                        // Exit the application
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void PerformLogin(UserManager userManager, AdminManager adminManager, DarshanManager darshanManager)
        {
            Console.WriteLine("1. Admin Login");
            Console.WriteLine("2. User Login");
            Console.Write("Enter your choice: ");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out int loginChoice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }

            switch (loginChoice)
            {
                case 1:
                    // Admin Login
                    Console.Write("Enter Admin Username: ");
                    string adminUsername = Console.ReadLine();
                    Console.Write("Enter Admin Password: ");
                    string adminPassword = Console.ReadLine();

                    Admin adminLoggedInUser = userManager.AdminLogin(adminUsername, adminPassword);
                    if (adminLoggedInUser != null)
                    {
                        HandleAdminUser(adminManager, darshanManager, userManager);
                    }
                    break;
                case 2:
                    // User Login
                    Console.Write("Enter Username: ");
                    string username = Console.ReadLine();
                    Console.Write("Enter Password: ");
                    string password = Console.ReadLine();

                    User loggedInUser = userManager.Login(username, password);
                    if (loggedInUser != null)
                    {
                        HandleLoggedInUser(darshanManager, userManager, loggedInUser);
                    }
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        static void PerformSignUp(UserManager userManager)
        {
            Console.Write("Enter Username: ");
            string newUsername = Console.ReadLine();
            Console.Write("Enter Password: ");
            string newPassword = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Full Name: ");
            string fullName = Console.ReadLine();
            Console.Write("Enter Phone Number: ");
            string phoneNumber = Console.ReadLine();
            Console.Write("Enter Address: ");
            string address = Console.ReadLine();

            userManager.Signup(newUsername, newPassword, email, fullName, phoneNumber, address);
        }

        static void HandleLoggedInUser(DarshanManager darshanManager, UserManager userManager, User user)
        {
            while (true)
            {
                Console.WriteLine("1. Book Darshan Appointment");
                Console.WriteLine("2. View Darshan Appointments");
                Console.WriteLine("3. Cancel Darshan Appointment");
                Console.WriteLine("4. Update Profile");
                Console.WriteLine("5. Logout");

                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        // Book Darshan Appointment
                        DarshanAppointment newAppointment = new DarshanAppointment();
                        Console.Write("Enter Devotee Name: ");
                        newAppointment.DevoteeName = Console.ReadLine();
                        Console.Write("Enter Appointment Date (yyyy-MM-dd): ");
                        if (!DateTime.TryParse(Console.ReadLine(), out DateTime appointmentDate))
                        {
                            Console.WriteLine("Invalid date format. Please use yyyy-MM-dd.");
                            continue;
                        }
                        newAppointment.AppointmentDate = appointmentDate;

                        darshanManager.AddAppointment(newAppointment, user.UserId);
                        Console.WriteLine("Appointment booked successfully!");
                        break;
                    case 2:
                        // View Darshan Appointments
                        darshanManager.ViewAppointments(user.UserId);
                        break;
                    case 3:
                        // Cancel Darshan Appointment
                        Console.Write("Enter Appointment ID to cancel: ");
                        if (!int.TryParse(Console.ReadLine(), out int appointmentId))
                        {
                            Console.WriteLine("Invalid input. Please enter a number.");
                            continue;
                        }

                        darshanManager.CancelAppointment(appointmentId, user.UserId);
                        break;
                    case 4:
                        // Update Profile
                        Console.Write("Enter new Username: ");
                        string newUsername = Console.ReadLine();
                        Console.Write("Enter new Password: ");
                        string newPassword = Console.ReadLine();
                        Console.Write("Enter new Email: ");
                        string newEmail = Console.ReadLine();
                        Console.Write("Enter new Full Name: ");
                        string newFullName = Console.ReadLine();
                        Console.Write("Enter new Phone Number: ");
                        string newPhoneNumber = Console.ReadLine();
                        Console.Write("Enter new Address: ");
                        string newAddress = Console.ReadLine();

                        userManager.UpdateUser(user.UserId, newUsername, newPassword, newEmail, newFullName, newPhoneNumber, newAddress);
                        break;
                    case 5:
                        // Logout
                        Console.WriteLine("Logged out successfully!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void HandleAdminUser(AdminManager adminManager, DarshanManager darshanManager, UserManager userManager)
        {
            while (true)
            {
                Console.WriteLine("1. View All Appointments");
                Console.WriteLine("2. View All Users");
                Console.WriteLine("3. View All Admins");
                Console.WriteLine("4. View Appointments by Date");
                Console.WriteLine("5. Delete User");
                Console.WriteLine("6. Logout");

                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        // View all appointments
                        adminManager.ViewAllAppointments(darshanManager);
                        break;
                    case 2:
                        // View all users
                        adminManager.ViewAllUsers(userManager);
                        break;
                    case 3:
                        // View all admins
                        userManager.ViewAllAdmins();
                        break;
                    case 4:
                        // View appointments by date
                        Console.Write("Enter date (yyyy-MM-dd): ");
                        if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
                        {
                            Console.WriteLine("Invalid date format. Please use yyyy-MM-dd.");
                            continue;
                        }
                        darshanManager.ViewAppointmentsByDate(date);
                        break;
                    case 5:
                        // Delete user
                        Console.Write("Enter User ID to delete: ");
                        if (!int.TryParse(Console.ReadLine(), out int userId))
                        {
                            Console.WriteLine("Invalid input. Please enter a number.");
                            continue;
                        }
                        adminManager.DeleteUser(userManager, userId);
                        break;
                    case 6:
                        // Logout
                        Console.WriteLine("Logged out successfully!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}
