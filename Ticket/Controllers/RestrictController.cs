using BLL.Models.RestrictModels;
using BLL.Models.WaterBodyModels;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ticket.Controllers
{
    public class RestrictController : Controller
    {
        
        private readonly IWaterBodyService _waterBodyService;
        private readonly IRestrictService _restrictService;
        private readonly ITicketTypeService _ticketTypeService;

        public RestrictController(IWaterBodyService waterBodyService, IRestrictService restrictService, ITicketTypeService ticketTypeService)
        {
            _waterBodyService = waterBodyService;
            _restrictService = restrictService;
            _ticketTypeService = ticketTypeService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetSelectData()
        {
            // Здесь получаем данные для заполнения селекта, например, из базы данных или другого источника
            var data = await _waterBodyService.GetAllWaterBody(); // Функция GetSelectDataFromDatabase возвращает список объектов

            return Json(data); // Возвращаем данные в формате JSON
        }
        [HttpGet]
        public async Task<IActionResult> FindDataById([FromQuery]int id)
        {
            // Здесь получаем данные для заполнения селекта, например, из базы данных или другого источника
            var foundObjects = await _restrictService.GetRestrictByWaterBodyId(id); // Функция GetSelectDataFromDatabase возвращает список объектов

            return Json(foundObjects); // Возвращаем данные в формате JSON
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRestricts()
        {
            // Здесь получаем данные для заполнения селекта, например, из базы данных или другого источника
            var all = await _restrictService.GetAllRestrict(); // Функция GetSelectDataFromDatabase возвращает список объектов

            return Json(all); // Возвращаем данные в формате JSON
        }
        public async Task<IActionResult> Create()
        {
            var water = await _waterBodyService.GetAllWaterBody();
            var ticket = await _ticketTypeService.GetAllTicketType();
            RestrictVM restrictVM = new RestrictVM()
            {
                restrictRequest = new RestrictRequest(),
                TicketTypeResponse = new List<SelectListItem>(),
                WaterbodyResponse = new List<SelectListItem>(),
            };
            foreach (var item in ticket)
            {
                restrictVM.TicketTypeResponse.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            foreach (var item in water)
            {
                restrictVM.WaterbodyResponse.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return View(restrictVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RestrictVM obj)
        {
            if (ModelState.IsValid)
            {
                await _restrictService.CreateRestrict(obj.restrictRequest);
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = await _restrictService.GetSingleRestrict(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var obj = await _restrictService.GetSingleRestrict(id);
            if (obj == null)
            {
                return NotFound();
            }
            await _restrictService.DeleteRestrict(id);
            return RedirectToAction("Index");
        }

    }
}
