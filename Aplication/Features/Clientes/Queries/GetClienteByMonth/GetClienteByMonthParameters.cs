using Aplication.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Features.Clientes.Queries.GetClienteByMonth
{
    public class GetClienteByMonthParameters: RequestParameter
    {
     
        public string? BirthMonth { get; set; }
    }
}
