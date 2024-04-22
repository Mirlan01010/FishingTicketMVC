using BLL.Models.CitizenShipModels;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ticket.Controllers
{
    public class CitizenShipController : Controller
    {
        private readonly ICitizenShipService _service;
        public CitizenShipController(ICitizenShipService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<CitizenShipResponse> objList = await _service.GetAllCitizenShip();
            objList = await _service.GetAllCitizenShip();
            return View(objList);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CitizenShipRequest obj)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateCitizenShip(obj);
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
            var obj = await _service.GetSingleCitizenShip(id);
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

            var obj = await _service.GetSingleCitizenShip(id);
            if (obj.IsActive == true)
            {
                obj.IsActive = false;
            }
            else
            {
                obj.IsActive = true;
            }
            await _service.UpdateCitizenShip(obj);

            return RedirectToAction("Active", new { id = id });
        }
        //GET - Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = await _service.GetSingleCitizenShip(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CitizenShipResponse obj)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateCitizenShip(obj);
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
            var obj = await _service.GetSingleCitizenShip(id);
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
            var obj = await _service.GetSingleCitizenShip(id);
            if (obj == null)
            {
                return NotFound();
            }
            await _service.DeleteCitizenShip(id);
            return RedirectToAction("Index");
        }
    }
}
