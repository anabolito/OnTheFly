using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class SaleDTO
    {
        public string IataFlight { get; set; }
        public string RabFlight { get; set; }
        public string DtDepartureFlight { get; set; }
        public List<string> Cpf { get; set; }
        public bool Reserved { get; set; }
        public bool Sold { get; set; }
    }
}
