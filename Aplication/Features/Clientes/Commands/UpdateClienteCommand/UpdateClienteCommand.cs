﻿using Aplication.Interfaces;
using Aplication.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplication.Exceptions;

namespace Aplication.Features.Clientes.Commands.UpdateClienteCommand
{
    public class UpdateClienteCommand : IRequest<Response<int>>
    {
        public int Id {get; set;} 
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }



    }

    public class UpdateClienteCommandHandler : IRequestHandler<UpdateClienteCommand,Response<int>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        private readonly IMapper _mapper;

        public UpdateClienteCommandHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<int>>Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = await _repositoryAsync.GetByIdAsync(request.Id);
            if (cliente == null)
            {
                throw new KeyNotFoundException($"REgistro no encontrado en {request.Id}");
            }
            else
            {
                cliente.Nombre = request.Nombre;
                cliente.Apellido = request.Apellido;
                cliente.FechaNacimiento = request.FechaNacimiento;
                cliente.Telefono = request.Telefono;
                cliente.Email = request.Email;
                cliente.Direccion = request.Direccion;
                await _repositoryAsync.UpdateAsync(cliente);
                return new Response<int>(cliente.Id);
            }
        }
    }

}
