using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRLINEMANAGEMENTSYSTEM.Model
{
    public class AirportDetails
    {
        public int AirportDetailsId { get; set; }
        public string AirportName { get; set; }
        public City City { get; set; }
        public Country Country { get; set; }

    }
}
