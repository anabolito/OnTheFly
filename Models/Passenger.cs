using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Passenger
    {
        public string CPF { get; set; }
        public string Name { get; set; }
        public char Gender { get; set; }
        public string? Phone{ get; set; }
        public DateOnly DtBirth { get; set; }
        public DateTime DtRegistry { get; set; }
        public bool? Status { get; set; }
        public Address Address{ get; set; }
    }
}
