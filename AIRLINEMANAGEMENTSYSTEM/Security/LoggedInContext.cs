using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRLINEMANAGEMENTSYSTEM.Security
{
    public static class LoggedInContext
    {
        // Stores currently logged-in administrator ID
        public static int AdministratorId { get; private set; }

        // Indicates whether someone is logged in
        public static bool IsLoggedIn => AdministratorId > 0;

        // Call this after successful login
        public static void SetAdministrator(int administratorId)
        {
            AdministratorId = administratorId;
        }

        // Call this on logout
        public static void Clear()
        {
            AdministratorId = 0;
        }
    }
}
