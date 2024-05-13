using BLL.Models.WaterBodyModels;
using BLL.Services;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ticket.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaterBodyController : ControllerBase
    {
        private readonly IWaterBodyService _service;
        public WaterBodyController(IWaterBodyService service)
        {
            _service = service;
        }
        [Route("GetAllWaterBodyByRegionId")]
        [HttpGet]
        [SwaggerResponse(200, "Success", Type = typeof(List<WaterBody>))]
        [SwaggerResponse(400, "Not Found!")]
        [SwaggerOperation(Summary = "Получить все элемент по id области", Description = "Возвращает все элементы как типа List")]
        public async Task<ActionResult<List<WaterBodyResponse>>> GetAllWaterBodyByRegionId([FromQuery] int regionId)
        {
            var result = await _service.GetWaterBodyByRegionId(regionId);
            return result == null ? NotFound("No data found from the database!") : Ok(result);
        }
    }
}
