using AIRLINEMANAGEMENTSYSTEM.Helper;
using AIRLINEMANAGEMENTSYSTEM.Model;
using AIRLINEMANAGEMENTSYSTEM.Security;
using AIRLINEMANAGEMENTSYSTEM.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRLINEMANAGEMENTSYSTEM.Administrator
{
    public class AdministratorMenu
    {

        private readonly  IAdministratorService _administratorService;

        public AdministratorMenu(IAdministratorService administratorService)
        {
            _administratorService = administratorService;
        }

        private int ReadValidId(string message, IEnumerable<int> validIds)
        {
            while (true)
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine(), out int id) && validIds.Contains(id))
                    return id;

                ConsoleMessageHelper.WriteError("Invalid selection. Please choose from the list.");
            }
        }

        private DateTime ReadValidDate(string message, string format)
        {
            while (true)
            {
                Console.Write(message);
                if (DateTime.TryParseExact(
                        Console.ReadLine(),
                        format,
                        null,
                        System.Globalization.DateTimeStyles.None,
                        out DateTime date))
                    return date;

                ConsoleMessageHelper.WriteError($"Invalid format. Expected {format}");
            }
        }



        public async Task ShowMenu()
        {
            //Console.WriteLine("");
            //Console.WriteLine(" Login successful....");
            Console.WriteLine("");

            //while loop for Doctor Menu and CRUD Operations
            while (true)
            {
                Console.WriteLine("------- Administrator Menu --------");
                Console.WriteLine("1. List All Scheduled Flights");
                Console.WriteLine("2. Search Scheduled Flight");
                Console.WriteLine("3. Schedule A flight");
                Console.WriteLine("4. Edit a Scheduled flight Details ");
                Console.WriteLine("5. Logout");

                Console.Write("Enter your choice: ");
                if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 5)
                {
                    ConsoleMessageHelper.WriteWarning("Invalid choice...Please try again");
                    continue;
                }
                Console.Clear();

                try
                {
                    switch (choice)
                    {
                        case 1:
                            {
                                // Fetch all scheduled flight details from the service layer
                                var schedules = await _administratorService.GetFlightScheduleDetailsAsync();

                                // If no schedules are available, notify the user and return to menu
                                if (schedules == null || schedules.Count == 0)
                                {
                                    ConsoleMessageHelper.WriteWarning("No flight schedules found.");
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;
                                }

                                // Print the top border of the table
                                Console.WriteLine(new string('-', 180));

                                // Print the table header with fixed-width columns for proper alignment
                                Console.WriteLine(
                                    $"{"Schedule",-10}" +
                                    $"{"Flight",-15}" +
                                    $"{"Departure Airport",-40}" +
                                    $"{"Arrival Airport",-40}" +
                                    $"{"Dep Date",-12}" +
                                    $"{"Dep Time",-10}" +
                                    $"{"Arr Date",-12}" +
                                    $"{"Arr Time",-10}" +
                                    $"{"Created By",-15}"
                                );

                                // Print the header separator line
                                Console.WriteLine(new string('-', 180));

                                // Iterate through each flight schedule and display its details
                                foreach (var fs in schedules)
                                {
                                    Console.WriteLine(
                                        $"{fs.FlightScheduleId,-10}" +
                                        $"{fs.FlightDetails?.FlightName,-15}" +
                                        $"{fs.DepartureAirport?.AirportName,-40}" +
                                        $"{fs.ArrivalAirport?.AirportName,-40}" +
                                        $"{fs.DepartureDate,-12:yyyy-MM-dd}" +
                                        $"{fs.DepartureTime,-10:HH:mm}" +
                                        $"{fs.ArrivalDate,-12:yyyy-MM-dd}" +
                                        $"{fs.ArrivalTime,-10:HH:mm}" +
                                        $"{fs.Administrator?.Username,-15}"
                                    );
                                }

                                // Print the bottom border of the table
                                Console.WriteLine(new string('-', 180));

                                // Exit case 1 and return control to the administrator menu
                                break;
                            }


                        case 2:
                            {
                                // Variable to store the flight schedule ID entered by the user
                                int search;

                                // Keep prompting until a valid numeric schedule code is entered
                                while (true)
                                {
                                    Console.Write("Enter Scheduled Flight Code: ");
                                    if (int.TryParse(Console.ReadLine(), out search))
                                        break;

                                    ConsoleMessageHelper.WriteError("Invalid input. Enter a number.");
                                }

                                // Fetch the flight schedule details for the given schedule ID
                                var fs = await _administratorService.GetFlightScheduleByIdAsync(search);

                                // If no schedule is found, notify the user and return to menu
                                if (fs == null)
                                {
                                    ConsoleMessageHelper.WriteWarning("No schedule found for given ID.");
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;
                                }

                                // Print table separator line
                                Console.WriteLine(new string('-', 150));

                                // Print table header with fixed-width columns
                                Console.WriteLine(
                                    $"{"ID",-5}" +
                                    $"{"Flight",-15}" +
                                    $"{"Departure Airport",-30}" +
                                    $"{"Arrival Airport",-30}" +
                                    $"{"Dep Date",-12}" +
                                    $"{"Dep Time",-10}" +
                                    $"{"Arr Date",-12}" +
                                    $"{"Arr Time",-10}" +
                                    $"{"Created By",-15}"
                                );

                                // Print table separator line
                                Console.WriteLine(new string('-', 150));

                                // Print the flight schedule details in a single row
                                Console.WriteLine(
                                    $"{fs.FlightScheduleId,-5}" +
                                    $"{fs.FlightDetails?.FlightName,-15}" +
                                    $"{fs.DepartureAirport?.AirportName,-30}" +
                                    $"{fs.ArrivalAirport?.AirportName,-30}" +
                                    $"{fs.DepartureDate,-12:yyyy-MM-dd}" +
                                    $"{fs.DepartureTime,-10:HH:mm}" +
                                    $"{fs.ArrivalDate,-12:yyyy-MM-dd}" +
                                    $"{fs.ArrivalTime,-10:HH:mm}" +
                                    $"{fs.Administrator?.Username,-15}"
                                );

                                // Print closing separator line
                                Console.WriteLine(new string('-', 150));

                                // Pause so the user can read the result before returning to menu
                                Console.WriteLine("Press any key to return...");
                                Console.ReadKey();
                                Console.Clear();

                                break;
                            }


                        case 3:
                            {
                                // Display header for scheduling a new flight
                                Console.WriteLine("---- Schedule a Flight ----");

                                // Fetch all available flight details from the system
                                var flights = await _administratorService.GetAllFlightDetailsAsync();

                                // Display the list of available flights
                                Console.WriteLine("\nAvailable Flights:");
                                foreach (var f in flights)
                                    Console.WriteLine($"{f.FlightDetailsId} - {f.FlightName}");

                                // Prompt the administrator to select a valid flight ID
                                int flightId = ReadValidId(
                                    "\nEnter Flight ID: ",
                                    flights.Select(f => f.FlightDetailsId)
                                );

                                // Fetch all available airport details from the system
                                var airports = await _administratorService.GetAllAirportDetailsAsync();

                                // Display the list of available airports
                                Console.WriteLine("\nAvailable Airports:");
                                foreach (var a in airports)
                                    Console.WriteLine($"{a.AirportDetailsId} - {a.AirportName}");

                                // Prompt the administrator to select a valid departure airport ID
                                int departureAirportId = ReadValidId(
                                    "\nEnter Departure Airport ID: ",
                                    airports.Select(a => a.AirportDetailsId)
                                );

                                // Prompt the administrator to select a valid arrival airport ID
                                // Ensure arrival airport is not the same as departure airport
                                int arrivalAirportId;
                                while (true)
                                {
                                    arrivalAirportId = ReadValidId(
                                        "\nEnter Arrival Airport ID: ",
                                        airports.Select(a => a.AirportDetailsId)
                                    );

                                    if (arrivalAirportId != departureAirportId)
                                        break;

                                    ConsoleMessageHelper.WriteError(
                                        "Arrival airport cannot be same as departure airport."
                                    );
                                }

                                // Prompt for departure date with validation
                                DateTime depDate = ReadValidDate(
                                    "\nEnter Departure Date (yyyy-MM-dd): ",
                                    "yyyy-MM-dd"
                                );

                                // Prompt for departure time with validation
                                DateTime depTime = ReadValidDate(
                                    "Enter Departure Time (HH:mm): ",
                                    "HH:mm"
                                );

                                // Prompt for arrival date with validation
                                DateTime arrDate = ReadValidDate(
                                    "Enter Arrival Date (yyyy-MM-dd): ",
                                    "yyyy-MM-dd"
                                );

                                // Prompt for arrival time with validation
                                DateTime arrTime = ReadValidDate(
                                    "Enter Arrival Time (HH:mm): ",
                                    "HH:mm"
                                );

                                // Combine date and time to validate chronological correctness
                                DateTime departureDateTime = depDate.Date + depTime.TimeOfDay;
                                DateTime arrivalDateTime = arrDate.Date + arrTime.TimeOfDay;

                                // Ensure arrival time is after departure time
                                if (arrivalDateTime <= departureDateTime)
                                {
                                    ConsoleMessageHelper.WriteError(
                                        "Arrival time must be after departure time."
                                    );
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;
                                }

                                // Create a FlightSchedule object with all validated inputs
                                FlightSchedule schedule = new FlightSchedule
                                {
                                    FlightDetails = new FlightDetails
                                    {
                                        FlightDetailsId = flightId
                                    },
                                    DepartureAirport = new AirportDetails
                                    {
                                        AirportDetailsId = departureAirportId
                                    },
                                    ArrivalAirport = new AirportDetails
                                    {
                                        AirportDetailsId = arrivalAirportId
                                    },
                                    DepartureDate = depDate,
                                    DepartureTime = depTime,
                                    ArrivalDate = arrDate,
                                    ArrivalTime = arrTime,

                                    // Associate the logged-in administrator with this schedule
                                    Administrator = new Administrators
                                    {
                                        AdministratorId = LoggedInContext.AdministratorId
                                    }
                                };

                                // Call the service layer to add the new flight schedule
                                bool isAdded = await _administratorService.AddFlightScheduleAsync(schedule);

                                // Display result of the scheduling operation
                                if (isAdded)
                                    ConsoleMessageHelper.WriteSuccess("Flight scheduled successfully!");
                                else
                                    ConsoleMessageHelper.WriteError("Failed to schedule flight.");

                                // Small delay for user readability before clearing the screen
                                Thread.Sleep(1000);
                                Console.Clear();
                                break;
                            }

                        case 4:
                            {
                                // Display edit schedule header
                                Console.WriteLine("---- Edit Scheduled Flight ----");

                                // Ask the administrator to enter the schedule ID to edit
                                int scheduleId;
                                while (true)
                                {
                                    Console.Write("Enter Scheduled Flight ID: ");
                                    if (int.TryParse(Console.ReadLine(), out scheduleId))
                                        break;

                                    ConsoleMessageHelper.WriteError("Invalid Schedule ID");
                                }

                                // Fetch the existing flight schedule using the provided ID
                                var existing = await _administratorService.GetFlightScheduleByIdAsync(scheduleId);

                                // If no schedule is found, inform the user and return to menu
                                if (existing == null)
                                {
                                    ConsoleMessageHelper.WriteWarning("Scheduled flight not found.");
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;
                                }

                                // Display the current details of the selected schedule
                                ConsoleMessageHelper.WriteSuccess("Existing Schedule Found");
                                Console.WriteLine(
                                    $"Flight: {existing.FlightDetails.FlightName}\n" +
                                    $"From: {existing.DepartureAirport.AirportName}\n" +
                                    $"To: {existing.ArrivalAirport.AirportName}\n" +
                                    $"Departure: {existing.DepartureDate:yyyy-MM-dd} {existing.DepartureTime:HH:mm}\n" +
                                    $"Arrival: {existing.ArrivalDate:yyyy-MM-dd} {existing.ArrivalTime:HH:mm}"
                                );

                                // Load all available flight details for selection
                                var flights = await _administratorService.GetAllFlightDetailsAsync();

                                // Load all available airport details for selection
                                var airports = await _administratorService.GetAllAirportDetailsAsync();

                                // Display available flights
                                Console.WriteLine("\nAvailable Flights:");
                                foreach (var f in flights)
                                    Console.WriteLine($"{f.FlightDetailsId} - {f.FlightName}");

                                // Prompt user to select a new flight ID from the available list
                                int flightId = ReadValidId(
                                    "\nEnter New Flight ID: ",
                                    flights.Select(f => f.FlightDetailsId)
                                );

                                // Display available airports
                                Console.WriteLine("\nAvailable Airports:");
                                foreach (var a in airports)
                                    Console.WriteLine($"{a.AirportDetailsId} - {a.AirportName}");

                                // Prompt user to select a new departure airport ID
                                int depAirport = ReadValidId(
                                    "\nEnter New Departure Airport ID: ",
                                    airports.Select(a => a.AirportDetailsId)
                                );

                                // Prompt user to select a new arrival airport ID
                                // Ensure that arrival airport is not the same as departure airport
                                int arrAirport;
                                while (true)
                                {
                                    arrAirport = ReadValidId(
                                        "\nEnter New Arrival Airport ID: ",
                                        airports.Select(a => a.AirportDetailsId)
                                    );

                                    if (arrAirport != depAirport)
                                        break;

                                    ConsoleMessageHelper.WriteError("Arrival airport cannot be same as departure airport.");
                                }

                                // Prompt user to enter the new departure date
                                DateTime depDate = ReadValidDate(
                                    "\nEnter New Departure Date (yyyy-MM-dd): ",
                                    "yyyy-MM-dd"
                                );

                                // Prompt user to enter the new departure time
                                DateTime depTime = ReadValidDate(
                                    "Enter New Departure Time (HH:mm): ",
                                    "HH:mm"
                                );

                                // Prompt user to enter the new arrival date
                                DateTime arrDate = ReadValidDate(
                                    "Enter New Arrival Date (yyyy-MM-dd): ",
                                    "yyyy-MM-dd"
                                );

                                // Prompt user to enter the new arrival time
                                DateTime arrTime = ReadValidDate(
                                    "Enter New Arrival Time (HH:mm): ",
                                    "HH:mm"
                                );

                                // Combine date and time values to validate schedule timing
                                DateTime depDT = depDate.Date + depTime.TimeOfDay;
                                DateTime arrDT = arrDate.Date + arrTime.TimeOfDay;

                                // Validate that arrival time is after departure time
                                if (arrDT <= depDT)
                                {
                                    ConsoleMessageHelper.WriteError("Arrival must be after departure.");
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;
                                }

                                // Update the existing schedule object with new values
                                existing.FlightDetails.FlightDetailsId = flightId;
                                existing.DepartureAirport.AirportDetailsId = depAirport;
                                existing.ArrivalAirport.AirportDetailsId = arrAirport;
                                existing.DepartureDate = depDate;
                                existing.DepartureTime = depTime;
                                existing.ArrivalDate = arrDate;
                                existing.ArrivalTime = arrTime;

                                // Call the service layer to persist the updated schedule
                                bool updated = await _administratorService.UpdateFlightScheduleAsync(existing);

                                // Display result of the update operation
                                if (updated)
                                    ConsoleMessageHelper.WriteSuccess("Scheduled flight updated successfully!");
                                else
                                    ConsoleMessageHelper.WriteError("Update failed.");

                                break;
                            }


                        case 5:
                            Console.WriteLine("Logging out..");
                            Thread.Sleep(1000);
                            Console.Clear();
                            return;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleMessageHelper.WriteError($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
