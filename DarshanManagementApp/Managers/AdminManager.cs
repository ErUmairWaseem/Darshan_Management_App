using DarshanManagementApp.Models;
using System;
using System.Collections.Generic;

namespace DarshanManagementApp.Managers
{
    public class AdminManager
    {
        private List<Admin> admins;

        public AdminManager(List<Admin> adminList)
        {
            admins = adminList;
        }

        public bool IsAdmin(Admin admin)
        {
            return admin != null;
        }

        public void ViewAllAppointments(DarshanManager darshanManager)
        {
            darshanManager.ViewAllAppointments();
        }

        public void ViewAllUsers(UserManager userManager)
        {
            userManager.ViewAllUsers();
        }

        public void DeleteUser(UserManager userManager, int userId)
        {
            userManager.DeleteUser(userId);
        }
    }
}
