using AIRLINEMANAGEMENTSYSTEM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRLINEMANAGEMENTSYSTEM.Service
{
    public interface IAdministratorService
    {
        Task<List<FlightSchedule>> GetFlightScheduleDetailsAsync();
        Task<FlightSchedule?> GetFlightScheduleByIdAsync(int flightScheduleId);

        Task<List<FlightDetails>> GetAllFlightDetailsAsync();
        Task<List<AirportDetails>> GetAllAirportDetailsAsync();

        Task<bool> AddFlightScheduleAsync(FlightSchedule flightSchedule);
        Task<bool> UpdateFlightScheduleAsync(FlightSchedule flightSchedule);
    }

}
