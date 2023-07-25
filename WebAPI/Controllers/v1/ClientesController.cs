using Aplication.Features.Clientes.Commands.CreateClienteCommand;
using Aplication.Features.Clientes.Commands.DeleteClienteCommand;
using Aplication.Features.Clientes.Commands.UpdateClienteCommand;
using Aplication.Features.Clientes.Queries.GetClienteById;
using Aplication.Features.Clientes.Queries.GetAllClientes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ClientesController : BaseApiController

    {
        [HttpGet]
        public async Task <IActionResult> Get([FromQuery]GetAllClientesParameters filter)
        {
            return Ok(await Mediator.Send(new GetAllClientesQuery { PageNumber = filter.PageNumber
                , PageSize = filter.PageSize, Nombre=filter.Nombre, Apellido= filter.Apellido}));
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> Get(int id) 
        {
            return Ok(await Mediator.Send(new GetClienteByIdQuery { Id = id }));
        }

        //POST api/<controller>
      
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CreateClienteCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("Update/{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, UpdateClienteCommand command)
        {
            if (id != command.Id) return BadRequest();
          
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, DeleteClienteCommand command)
        {

            return Ok(await Mediator.Send(new DeleteClienteCommand { Id=id}));
        }
    }
}
