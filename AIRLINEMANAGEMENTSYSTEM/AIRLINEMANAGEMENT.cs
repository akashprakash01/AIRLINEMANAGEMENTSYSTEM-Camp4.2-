using AIRLINEMANAGEMENTSYSTEM.Administrator;
using AIRLINEMANAGEMENTSYSTEM.Helper;
using AIRLINEMANAGEMENTSYSTEM.Repository;
using AIRLINEMANAGEMENTSYSTEM.Security;
using AIRLINEMANAGEMENTSYSTEM.Service;

namespace AIRLINEMANAGEMENTSYSTEM
{
    internal class AIRLINEMANAGEMENT
    {
        static void Main(string[] args)
        {
            LoginProcess().GetAwaiter().GetResult();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

        }

        #region Login process
        public static async Task LoginProcess()
        {
            // Display application welcome message
            Console.WriteLine("Welcome to AIRLINE MANAGEMENT SYSTEM");

            // Keep prompting until a valid login occurs
            while (true)
            {
                try
                {
                    Console.WriteLine("--- Login ---");

                    // Read username from console
                    Console.Write("Enter Your Username: ");
                    string username = Console.ReadLine();

                    // Read password (masked input)
                    Console.Write("Enter Your Password: ");
                    string password = ReadPassword();

                    // Retrieve database connection string 
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CsWin"].ConnectionString;

                    // Create login repository using connection string
                    ILoginRepository loginRepo = new LoginRepository(connectionString);

                    // Validate username and password against database
                    var result = await loginRepo.LoginAsync(username, password);

                    // If authentication fails, show error and retry
                    if (!result.isSuccess)
                    {
                        ConsoleMessageHelper.WriteError("Invalid Username or Password. Try again.");
                        continue;
                    }

                    // Store the currently logged-in administrator ID
                    LoggedInContext.SetAdministrator(result.administratorId);

                    // Create repository for administrator operations
                    IAdministratorRepository administratorRepo = new AdministratorRepositoryImpl(connectionString);

                    // Create service layer using repository dependency
                    IAdministratorService administratorServ = new AdministratorServiceImpl(administratorRepo);

                    // Launch administrator menu
                    AdministratorMenu menu = new AdministratorMenu(administratorServ);

                    // Clear login screen and show admin menu
                    Console.Clear();
                    await menu.ShowMenu();

                    // Exit login loop after successful menu execution
                    break;
                }
                catch (Exception ex)
                {
                    // Catch and display any unexpected runtime errors
                    ConsoleMessageHelper.WriteError(
                        $"An error occurred: {ex.Message}"
                    );
                }
            }
        }
        #endregion


        #region
        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Ignore special keys like Shift, Ctrl, etc.
                if (char.IsControl(key.KeyChar))
                {
                    // Handle Backspace
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        Console.Write("\b \b"); // remove last *
                    }
                }
                else
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }

            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine(); // move to next line
            return password;
        }
        #endregion
    }
}
