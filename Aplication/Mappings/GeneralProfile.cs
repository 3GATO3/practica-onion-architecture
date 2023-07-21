using Aplication.DTOs;
using Aplication.Features.Clientes.Commands.CreateClienteCommand;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Mappings
{
    public class GeneralProfile: Profile
    {
        public GeneralProfile()
        {
            #region Dtos
            CreateMap<Cliente, ClienteDto>();
            #endregion

            #region Commands
            CreateMap<CreateClienteCommand, Cliente>();

            #endregion
        }
    }
}
