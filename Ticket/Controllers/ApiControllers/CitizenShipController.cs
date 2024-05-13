using BLL.Models.CitizenShipModels;
using BLL.Services;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ticket.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitizenShipController : ControllerBase
    {
        private readonly ICitizenShipService _service;
        public CitizenShipController(ICitizenShipService service)
        {
            _service = service;
        }
        [Route("GetAllCitizenShip")]
        [HttpGet]
        [SwaggerResponse(200, "Success", Type = typeof(List<CitizenShip>))]
        [SwaggerResponse(400, "Not Found!")]
        [SwaggerOperation(Summary = "Получить все элемент", Description = "Возвращает все элементы как типа List")]
        public async Task<ActionResult<List<CitizenShipResponse>>> GetAllCitizenShip()
        {
            var result = await _service.GetAllCitizenShip();
            return result == null ? NotFound("No data found from the database!") : Ok(result);
        }
    }
}
