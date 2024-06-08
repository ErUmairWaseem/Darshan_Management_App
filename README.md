Project Overview: Darshan Management Application
The Darshan Management Application facilitates the booking and management of darshan appointments. It includes functionalities for both users and admins.

Modules and Features:


User Module:
Sign Up: Register with username, password, email, full name, phone number, and address.
Login: Authenticate with username and password.
Book Appointment: Schedule a darshan appointment.
View Appointments: See booked appointments.
Cancel Appointment: Remove an existing appointment.
Update Profile: Modify user details.


Admin Module:
Admin Sign Up: Register with username and password.
Admin Login: Authenticate as admin.
View All Appointments: Display all darshan appointments.
View Appointments by Date: Display appointments for a specific date.
View All Users: List all registered users.
View All Admins: List all admins.
Delete User: Remove a user by ID.


Backend Database:
The application uses a MySQL database with three main tables:


Users:
UserId, Username, Password, Email, FullName, PhoneNumber, Address
Admins:
AdminId, Username, Password
DarshanAppointments:
AppointmentId, DevoteeName, AppointmentDate, UserId (Foreign Key referencing Users)


Summary:
DarshanManager: Manages database and appointments.
UserManager: Manages user and admin accounts.
AdminManager: Handles admin-specific tasks.

Program.cs: Main interface for user/admin operations.

This project streamlines the booking and management of darshan appointments, providing an efficient tool for users and administrators.
