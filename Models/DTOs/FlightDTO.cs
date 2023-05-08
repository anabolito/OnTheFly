using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class FlightDTO
    {
        public string IataDestiny { get; set; }
        public string IataDparture { get; set; }
        public string RabPlane { get; set; }
        public int Sales { get; set; }
        public DateTime DtDeparture { get; set; }
        public bool Status { get; set; }
    }
}
