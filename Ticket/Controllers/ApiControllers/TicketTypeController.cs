using BLL.Models.TicketTypeModels;
using BLL.Services;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ticket.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketTypeController : ControllerBase
    {
        private readonly ITicketTypeService _service;
        public TicketTypeController(ITicketTypeService service)
        {
            _service = service;
        }
        [Route("GetAllTicketType")]
        [HttpGet]
        [SwaggerResponse(200, "Success", Type = typeof(List<TicketType>))]
        [SwaggerResponse(400, "Not Found!")]
        [SwaggerOperation(Summary = "Получить все элемент", Description = "Возвращает все элементы как типа List")]
        public async Task<ActionResult<List<TicketTypeResponse>>> GetAllTicketType()
        {
            var result = await _service.GetAllTicketType();
            return result == null ? NotFound("No data found from the database!") : Ok(result);
        }
    }
}
