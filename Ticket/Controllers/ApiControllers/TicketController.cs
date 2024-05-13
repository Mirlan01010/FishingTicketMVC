using BLL.Models.Responses;
using BLL.Models.TicketModels;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ticket.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _service;
        public TicketController(ITicketService service)
        {
            _service = service;
        }

        [Authorize]
        [Route("CreateTicket")]
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", Type = typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request", Type = typeof(ApiResponse))]
        [SwaggerOperation(Summary = "Создать Элемент", Description = "Создает объект типа Ticket на основе переданных данных")]

        public async Task<ActionResult<ApiResponse>> CreateTicket([FromBody] TicketRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                return Unauthorized("UserID Not found");
            }
            if (model == null) { return NoContent(); }
            var result = await _service.CreateTicket(model, currentUserId);
            if (result.Success) { return Ok(result); }
            return BadRequest(result);
        }
    }
}
