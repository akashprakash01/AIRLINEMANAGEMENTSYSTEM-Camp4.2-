using AIRLINEMANAGEMENTSYSTEM.Model;
using ClassLibraryDatabaseConnection;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRLINEMANAGEMENTSYSTEM.Repository
{
    public class AdministratorRepositoryImpl : IAdministratorRepository
    {
        private readonly string _connectionString;

        public AdministratorRepositoryImpl(string connectionString)
        {
            //assigning connection data to private variable
            _connectionString = connectionString;
        }

        #region AddFlightScheduleRepositoryAsync
        public async Task<bool> AddFlightScheduleRepositoryAsync(FlightSchedule flightSchedule)
        {
            // Open SQL connection using the configured connection string
            using SqlConnection conn =
                ConnectionManager.OpenConnection(_connectionString);

            // Prepare SQL command to execute the insert stored procedure
            using SqlCommand cmd =
                new SqlCommand("sp_AddFlightSchedule", conn);

            // Specify that the command is a stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add flight ID parameter (foreign key to FlightDetails table)
            cmd.Parameters.AddWithValue(
                "@flightDetailsId",
                flightSchedule.FlightDetails.FlightDetailsId
            );

            // Add departure airport ID (foreign key to AirportDetails table)
            cmd.Parameters.AddWithValue(
                "@departureAirport",
                flightSchedule.DepartureAirport.AirportDetailsId
            );

            // Add arrival airport ID (foreign key to AirportDetails table)
            cmd.Parameters.AddWithValue(
                "@arrivalAirport",
                flightSchedule.ArrivalAirport.AirportDetailsId
            );

            // Add departure date
            cmd.Parameters.AddWithValue(
                "@departureDate",
                flightSchedule.DepartureDate
            );

            // Add departure time
            cmd.Parameters.AddWithValue(
                "@departureTime",
                flightSchedule.DepartureTime
            );

            // Add arrival date
            cmd.Parameters.AddWithValue(
                "@arrivalDate",
                flightSchedule.ArrivalDate
            );

            // Add arrival time
            cmd.Parameters.AddWithValue(
                "@arrivalTime",
                flightSchedule.ArrivalTime
            );

            // Add administrator ID who scheduled the flight
            cmd.Parameters.AddWithValue(
                "@administratorId",
                flightSchedule.Administrator.AdministratorId
            );

            // Execute the insert operation and get affected row count
            int rows = await cmd.ExecuteNonQueryAsync();

            // Return true if at least one row was inserted successfully
            return rows > 0;
        }
        #endregion

        #region GetAllFlightSchedulesRepositoryAsync
        public async Task<List<FlightSchedule>> GetAllFlightSchedulesRepositoryAsync()
        {
            // Create a list to store all flight schedule records fetched from the database
            List<FlightSchedule> flightSchedules = new List<FlightSchedule>();

            // Open a SQL connection using the configured connection string
            using SqlConnection conn = ConnectionManager.OpenConnection(_connectionString);

            // Create a SQL command to execute the stored procedure that lists all flight schedules
            using SqlCommand cmd = new SqlCommand("sp_ListAllFlightSchedules", conn);

            // Specify that the command type is a stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Execute the stored procedure and obtain a data reader
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // Read each record returned by the stored procedure
            while (await reader.ReadAsync())
            {
                // Map the current database row to a FlightSchedule domain object
                FlightSchedule schedule = new FlightSchedule
                {
                    // Read the primary key of the flight schedule
                    FlightScheduleId = reader.GetInt32(reader.GetOrdinal("flightScheduleId")),

                    // Populate flight details (only name is required for display here)
                    FlightDetails = new FlightDetails
                    {
                        FlightName = reader.GetString(reader.GetOrdinal("flightName"))
                    },

                    // Populate departure airport information
                    DepartureAirport = new AirportDetails
                    {
                        AirportName = reader.GetString(reader.GetOrdinal("DepartureAirport"))
                    },

                    // Populate arrival airport information
                    ArrivalAirport = new AirportDetails
                    {
                        AirportName = reader.GetString(reader.GetOrdinal("ArrivalAirport"))
                    },

                    // Read departure date from the database
                    DepartureDate = reader.GetDateTime(reader.GetOrdinal("departureDate")),

                    // Read departure time from the database
                    DepartureTime = reader.GetDateTime(reader.GetOrdinal("departureTime")),

                    // Read arrival date from the database
                    ArrivalDate = reader.GetDateTime(reader.GetOrdinal("arrivalDate")),

                    // Read arrival time from the database
                    ArrivalTime = reader.GetDateTime(reader.GetOrdinal("arrivalTime")),

                    // Populate administrator information who created the schedule
                    Administrator = new Administrators
                    {
                        Username = reader.GetString(reader.GetOrdinal("CreatedBy"))
                    },

                    // Read audit information (record creation timestamp)
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("createdAt")),

                    // Read audit information (record last updated timestamp)
                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updatedAt"))
                };

                // Add the mapped flight schedule object to the result list
                flightSchedules.Add(schedule);
            }

            // Return the complete list of flight schedules to the caller
            return flightSchedules;
        }
        #endregion


        #region Get Flight Schedule By ID Repository Method

        public async Task<FlightSchedule?> GetFlightScheduleByIdRepositoryAsync(int flightScheduleId)
        {
            // Initialize the FlightSchedule object as null.
            // It will remain null if no record is found for the given ID.
            FlightSchedule? schedule = null;

            // Open a SQL connection using the configured connection string
            using SqlConnection conn = ConnectionManager.OpenConnection(_connectionString);

            // Create a SQL command to execute the stored procedure
            // responsible for fetching a flight schedule by its ID
            using SqlCommand cmd = new SqlCommand("sp_GetFlightScheduleById", conn);

            // Specify that the command type is a stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add the flight schedule ID as a parameter to prevent SQL injection
            cmd.Parameters.AddWithValue("@flightScheduleId", flightScheduleId);

            // Execute the stored procedure and obtain a data reader
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // Check if a record is returned
            if (await reader.ReadAsync())
            {
                // Map the database record to a FlightSchedule domain object
                schedule = new FlightSchedule
                {
                    // Read the primary key of the flight schedule
                    FlightScheduleId = reader.GetInt32(reader.GetOrdinal("flightScheduleId")),

                    // Populate flight details information
                    FlightDetails = new FlightDetails
                    {
                        FlightName = reader.GetString(reader.GetOrdinal("flightName"))
                    },

                    // Populate departure airport details
                    DepartureAirport = new AirportDetails
                    {
                        AirportName = reader.GetString(reader.GetOrdinal("DepartureAirport"))
                    },

                    // Populate arrival airport details
                    ArrivalAirport = new AirportDetails
                    {
                        AirportName = reader.GetString(reader.GetOrdinal("ArrivalAirport"))
                    },

                    // Read departure date from the database
                    DepartureDate = reader.GetDateTime(reader.GetOrdinal("departureDate")),

                    // Read departure time from the database
                    DepartureTime = reader.GetDateTime(reader.GetOrdinal("departureTime")),

                    // Read arrival date from the database
                    ArrivalDate = reader.GetDateTime(reader.GetOrdinal("arrivalDate")),

                    // Read arrival time from the database
                    ArrivalTime = reader.GetDateTime(reader.GetOrdinal("arrivalTime")),

                    // Populate administrator details who created the schedule
                    Administrator = new Administrators
                    {
                        Username = reader.GetString(reader.GetOrdinal("CreatedBy"))
                    },

                    // Read audit information for record creation time
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("createdAt")),

                    // Read audit information for record last update time
                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updatedAt"))
                };
            }

            // Return the populated FlightSchedule object if found; otherwise null
            return schedule;
        }

        #endregion

        #region Get All Flight Details Repository Method

        public async Task<List<FlightDetails>> GetAllFlightDetailsAsync()
        {
            // Create a list to hold all flight detail records
            List<FlightDetails> flights = new();

            // Open a SQL connection using the configured connection string
            using SqlConnection conn = ConnectionManager.OpenConnection(_connectionString);

            // Create a SQL command to execute the stored procedure
            // that retrieves all flight details
            using SqlCommand cmd = new SqlCommand("sp_ListAllFlightDetails", conn);

            // Specify that the command is a stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Execute the stored procedure and obtain a data reader
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // Iterate through each row returned by the stored procedure
            while (await reader.ReadAsync())
            {
                // Map the current database row to a FlightDetails object
                flights.Add(new FlightDetails
                {
                    // Read the unique flight identifier
                    FlightDetailsId = reader.GetInt32(reader.GetOrdinal("flightDetailsId")),

                    // Read the flight name
                    FlightName = reader.GetString(reader.GetOrdinal("flightName"))
                });
            }

            // Return the complete list of flight details
            return flights;
        }

        #endregion

        #region Update Flight Schedule Repository Method

        public async Task<bool> UpdateFlightScheduleRepositoryAsync(FlightSchedule flightSchedule)
        {
            // Open a SQL connection using the configured connection string
            using SqlConnection conn = ConnectionManager.OpenConnection(_connectionString);

            // Create a SQL command to execute the stored procedure
            // responsible for updating an existing flight schedule
            using SqlCommand cmd = new SqlCommand("sp_UpdateFlightSchedule", conn);

            // Specify that the command is a stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add the flight schedule ID to identify the record to be updated
            cmd.Parameters.AddWithValue(
                "@flightScheduleId",
                flightSchedule.FlightScheduleId
            );

            // Add the updated flight details ID
            cmd.Parameters.AddWithValue(
                "@flightDetailsId",
                flightSchedule.FlightDetails.FlightDetailsId
            );

            // Add the updated departure airport ID
            cmd.Parameters.AddWithValue(
                "@departureAirport",
                flightSchedule.DepartureAirport.AirportDetailsId
            );

            // Add the updated arrival airport ID
            cmd.Parameters.AddWithValue(
                "@arrivalAirport",
                flightSchedule.ArrivalAirport.AirportDetailsId
            );

            // Add the updated departure date
            cmd.Parameters.AddWithValue(
                "@departureDate",
                flightSchedule.DepartureDate
            );

            // Add the updated departure time
            cmd.Parameters.AddWithValue(
                "@departureTime",
                flightSchedule.DepartureTime
            );

            // Add the updated arrival date
            cmd.Parameters.AddWithValue(
                "@arrivalDate",
                flightSchedule.ArrivalDate
            );

            // Add the updated arrival time
            cmd.Parameters.AddWithValue(
                "@arrivalTime",
                flightSchedule.ArrivalTime
            );

            // Execute the update operation and get the number of affected rows
            int rows = await cmd.ExecuteNonQueryAsync();

            // Return true if at least one record was updated successfully
            return rows > 0;
        }

        #endregion

        #region Get All Airport Details Repository Method

        public async Task<List<AirportDetails>> GetAllAirportDetailsAsync()
        {
            // Create a list to store all airport detail records
            List<AirportDetails> airports = new();

            // Open a SQL connection using the configured connection string
            using SqlConnection conn = ConnectionManager.OpenConnection(_connectionString);

            // Create a SQL command to execute the stored procedure
            // that retrieves all airport details
            using SqlCommand cmd = new SqlCommand("sp_ListAllAirportDetails", conn);

            // Specify that the command is a stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Execute the stored procedure and obtain a data reader
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // Iterate through each record returned by the stored procedure
            while (await reader.ReadAsync())
            {
                // Map the current database row to an AirportDetails object
                airports.Add(new AirportDetails
                {
                    // Read the unique airport identifier
                    AirportDetailsId = reader.GetInt32(reader.GetOrdinal("airportDetailsId")),

                    // Read the airport name
                    AirportName = reader.GetString(reader.GetOrdinal("airportName"))
                });
            }

            // Return the complete list of airport details
            return airports;
        }

        #endregion


    }
}
