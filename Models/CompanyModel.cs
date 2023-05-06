using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CompanyModel
    {
        public string CNPJ { get; set; }
        public string Name { get; set; }
        public string NameOpt { get; set; }
        public DateOnly DtOpen { get; set; }
        public bool? Status { get; set; }
        public Address Address { get; set; }

    }
}
