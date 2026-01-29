using AIRLINEMANAGEMENTSYSTEM.Model;
using AIRLINEMANAGEMENTSYSTEM.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRLINEMANAGEMENTSYSTEM.Service
{
    public class AdministratorServiceImpl : IAdministratorService
    {
        private readonly IAdministratorRepository _administratorRepository;

        public AdministratorServiceImpl(IAdministratorRepository administratorRepository)
        {
            _administratorRepository = administratorRepository;
        }

        public async Task<List<FlightSchedule>> GetFlightScheduleDetailsAsync()
        {
           return await _administratorRepository.GetAllFlightSchedulesRepositoryAsync();

        }

        public async Task<FlightSchedule?> GetFlightScheduleByIdAsync(int flightScheduleId)
        {
            return  await _administratorRepository.GetFlightScheduleByIdRepositoryAsync(flightScheduleId);

        }

        public Task<List<FlightDetails>> GetAllFlightDetailsAsync()
            => _administratorRepository.GetAllFlightDetailsAsync();

        public Task<List<AirportDetails>> GetAllAirportDetailsAsync()
            => _administratorRepository.GetAllAirportDetailsAsync();

        public Task<bool> AddFlightScheduleAsync(FlightSchedule flightSchedule)
            => _administratorRepository.AddFlightScheduleRepositoryAsync(flightSchedule);

        public Task<bool> UpdateFlightScheduleAsync(FlightSchedule flightSchedule)
            => _administratorRepository.UpdateFlightScheduleRepositoryAsync(flightSchedule);
    }

}
