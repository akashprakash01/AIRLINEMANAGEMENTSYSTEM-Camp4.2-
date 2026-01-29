using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRLINEMANAGEMENTSYSTEM.Model
{
    public class FlightSchedule
    {
        public int FlightScheduleId { get; set; }

        public FlightDetails FlightDetails { get; set; }

        public AirportDetails DepartureAirport { get; set; }
        public AirportDetails ArrivalAirport { get; set; }

        public DateTime DepartureDate { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime ArrivalTime { get; set; }

        public Administrators Administrator { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

