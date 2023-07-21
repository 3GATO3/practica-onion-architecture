using Aplication.Interfaces;
using Aplication.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Features.Clientes.Commands.CreateClienteCommand
{
    public class CreateClienteCommand: IRequest<Response<int>>
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }

        public class CreateClienteCommandHandler : IRequestHandler<CreateClienteCommand, Response<int>>
        {
            private readonly IRepositoryAsync<Cliente> _repositoryAsync;
            private readonly IMapper _mapper;
            public CreateClienteCommandHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
            {
                _repositoryAsync = repositoryAsync;
                _mapper = mapper;
            }
            public async Task<Response<int>> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
            {


                var nuevoRegistro = _mapper.Map<Cliente>(request);
                nuevoRegistro.Edad = CalculateAge(request.FechaNacimiento, 0);
                var data = await _repositoryAsync.AddAsync(nuevoRegistro);
                await _repositoryAsync.SaveChangesAsync(cancellationToken);

                return new Response<int> ( data.Id );
            }
        }
        public static int CalculateAge(DateTime birthDate, int currentAge)
        {
            if (currentAge >= 0)
            {
                currentAge = new DateTime(DateTime.Now.Subtract(birthDate).Ticks).Year - 1;
            }
            return currentAge;
        }
    }
}
