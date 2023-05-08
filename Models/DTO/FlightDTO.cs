using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class FlightDTO
    {
        public string IataDestiny { get; set; }
        public string IataDparture { get; set; }
        public int RabPlane { get; set; }
        public int Sales { get; set; }
        public DateTime DtDeparture { get; set; }
        public bool Status { get; set; }
    }
}
