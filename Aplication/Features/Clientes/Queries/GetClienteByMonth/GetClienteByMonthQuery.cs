using Aplication.DTOs;
using Aplication.Interfaces;
using Aplication.specifications;
using Aplication.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Features.Clientes.Queries.GetClienteByMonth
{
    public class GetClienteByMonthQuery: IRequest<PagedResponse<List<ClienteDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Birth { get; set; }
        


        public class GetClienteByMonthQueryHandler : IRequestHandler<GetClienteByMonthQuery, PagedResponse<List<ClienteDto>>>
        {
            private readonly IRepositoryAsync<Cliente> _repositoryAsync;
            private readonly IMapper _mapper;
            private readonly IDistributedCache _distributedCache;
            public GetClienteByMonthQueryHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper, IDistributedCache distributedCache)
            {
                _repositoryAsync = repositoryAsync;
                _mapper = mapper;
                _distributedCache = distributedCache;
            }

            public async Task<PagedResponse<List<ClienteDto>>> Handle(GetClienteByMonthQuery request, CancellationToken cancellationToken)
            {
                var clientes = await _repositoryAsync.ListAsync(new PagedClientesBirthSpecification(request.PageSize, request.PageNumber, request.Birth));

               
                var clientesDto = _mapper.Map<List<ClienteDto>>(clientes);
                return new PagedResponse<List<ClienteDto>>(clientesDto, request.PageNumber, request.PageSize);



            }
        }
    }
}
