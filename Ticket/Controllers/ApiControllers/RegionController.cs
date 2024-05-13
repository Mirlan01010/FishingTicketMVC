using BLL.Models.RegionModels;
using BLL.Services;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ticket.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService _service;
        public RegionController(IRegionService service)
        {
            _service = service;
        }
        [Route("GetAllRegion")]
        [HttpGet]
        [SwaggerResponse(200, "Success", Type = typeof(List<Region>))]
        [SwaggerResponse(400, "Not Found!")]
        [SwaggerOperation(Summary = "Получить все элемент", Description = "Возвращает все элементы как типа List")]
        public async Task<ActionResult<List<RegionResponse>>> GetAllRegion()
        {
            var result = await _service.GetAllRegion();
            return result == null ? NotFound("No data found from the database!") : Ok(result);
        }
    }
}
