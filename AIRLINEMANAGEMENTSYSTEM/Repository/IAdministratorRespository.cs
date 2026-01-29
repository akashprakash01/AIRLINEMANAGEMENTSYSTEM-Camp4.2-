using AIRLINEMANAGEMENTSYSTEM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRLINEMANAGEMENTSYSTEM.Repository
{
    public interface IAdministratorRepository
    {
        Task<List<FlightSchedule>> GetAllFlightSchedulesRepositoryAsync();

        Task<List<FlightDetails>> GetAllFlightDetailsAsync();

        Task<List<AirportDetails>> GetAllAirportDetailsAsync();

        Task<FlightSchedule?> GetFlightScheduleByIdRepositoryAsync(int flightScheduleId);

        Task<bool> AddFlightScheduleRepositoryAsync(FlightSchedule flightSchedule);

        Task<bool> UpdateFlightScheduleRepositoryAsync(FlightSchedule flightSchedule);
    }
}
