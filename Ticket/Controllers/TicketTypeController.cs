using BLL.Models.TicketTypeModels;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ticket.Controllers
{
    [Authorize(Roles =("User"))]
    public class TicketTypeController : Controller
    {
        private readonly ITicketTypeService _service;
        public TicketTypeController(ITicketTypeService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<TicketTypeResponse> objList = await _service.GetAllTicketType();
            objList = await _service.GetAllTicketType();
            return View(objList);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketTypeRequest obj)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateTicketType(obj);
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public async Task<IActionResult> Active(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = await _service.GetSingleTicketType(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivePost(int? id)
        {

            var obj = await _service.GetSingleTicketType(id);
            if (obj.IsActive == true)
            {
                obj.IsActive = false;
            }
            else
            {
                obj.IsActive = true;
            }
            await _service.UpdateTicketType(obj);

            return RedirectToAction("Active", new { id = id });
        }
        //GET - Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = await _service.GetSingleTicketType(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TicketTypeResponse obj)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateTicketType(obj);
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
            var obj = await _service.GetSingleTicketType(id);
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
            var obj = await _service.GetSingleTicketType(id);
            if (obj == null)
            {
                return NotFound();
            }
            await _service.DeleteTicketType(id);
            return RedirectToAction("Index");
        }
    }
}
