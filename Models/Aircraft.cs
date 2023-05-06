using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Aircraft
    {
        public string RAB { get; set; }
        public int Capacity { get; set; }
        public DateOnly DtLastFlight { get; set; }
        public DateTime DtRegistry { get; set; }
        public Company Company { get; set; }
    }
}
