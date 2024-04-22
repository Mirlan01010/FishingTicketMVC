using BLL.Models.RegionModels;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ticket.Controllers
{
    [Authorize(Roles =("Admin"))]
    public class RegionController : Controller
    {
        private readonly IRegionService _service;
        public RegionController(IRegionService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<RegionResponse> objList = await _service.GetAllRegion();
            objList = await _service.GetAllRegion();
            return View(objList);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegionRequest obj)
        {
            if (ModelState.IsValid)
            {
                 await _service.CreateRegion(obj);
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
            var obj = await _service.GetSingleRegion(id);
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
            
            var obj = await _service.GetSingleRegion(id);
            if (obj.IsActive == true)
            {
                obj.IsActive = false;
            }
            else
            {
                obj.IsActive = true;
            }
            await _service.UpdateRegion(obj);
           
            return RedirectToAction("Active", new {id=id});
        }
        //GET - Edit
        public async  Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = await _service.GetSingleRegion(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RegionResponse obj)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateRegion(obj);
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
            var obj = await _service.GetSingleRegion(id);
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
            var obj = await _service.GetSingleRegion(id);
            if (obj == null)
            {
                return NotFound();
            }
            await _service.DeleteRegion(id);
            return RedirectToAction("Index");
        }

    }
}
