﻿using Aplication.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Features.Clientes.Queries.GetAllClientes
{
    public class GetAllClientesParameters: RequestParameter
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Birth { get; set; }
    }
}
