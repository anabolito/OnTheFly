using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Flight
    {
        public Airport Destiny{ get; set; }
        public Airport Departure{ get; set; }
        public Aircraft Plane { get; set; }
        public int Sales { get; set; }
        public DateTime DtDeparture { get; set; }
        public bool Status { get; set; }
    }
}
