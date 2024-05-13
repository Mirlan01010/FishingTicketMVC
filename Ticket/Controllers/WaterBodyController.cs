using BLL.Models.WaterBodyModels;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ticket.Controllers
{
    [Authorize(Roles = ("Admin"))]
    public class WaterBodyController : Controller
    {
        private readonly IWaterBodyService _service;
        private readonly IRegionService _regionService;
        public WaterBodyController(IWaterBodyService service, IRegionService regionService)
        {
            _service = service;
            _regionService = regionService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<WaterBodyResponse> objList = await _service.GetAllWaterBody();
            objList = await _service.GetAllWaterBody();
            return View(objList);
        }
        
        public async Task<IActionResult> Create()
        {
            var regobj = await _regionService.GetAllRegion();
            WaterBodyVM waterBodyVM = new WaterBodyVM()
            {
                WaterBodyRequest = new WaterBodyRequest { },
                regionResponses = new List<SelectListItem>()
            };
            foreach(var item in regobj)
            {
                waterBodyVM.regionResponses.Add(new SelectListItem { Text=item.Name,Value=item.Id.ToString()});
            }
            return View(waterBodyVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WaterBodyVM obj)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateWaterBody(obj.WaterBodyRequest);
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
            var obj = await _service.GetSingleWaterBody(id);
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

            var obj = await _service.GetSingleWaterBody(id);
            if (obj.IsActive == true)
            {
                obj.IsActive = false;
            }
            else
            {
                obj.IsActive = true;
            }
            await _service.UpdateWaterBody(obj);

            return RedirectToAction("Active", new { id = id });
        }
        //GET - Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = await _service.GetSingleWaterBody(id);
            if (obj == null)
            {
                return NotFound();
            }
            var regobj = await _regionService.GetAllRegion();
            WaterBodyUpdateVM waterBodyVM = new WaterBodyUpdateVM()
            {
                WaterBodyResponse = obj,
                regionResponses = new List<SelectListItem>()
            };
            foreach (var item in regobj)
            {
                waterBodyVM.regionResponses.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }

            return View(waterBodyVM);
        }
        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WaterBodyUpdateVM obj)
        {
           
                await _service.UpdateWaterBody(obj.WaterBodyResponse);
                return RedirectToAction("Index");
            
            return View(obj);
        }
        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = await _service.GetSingleWaterBody(id);
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
            var obj = await _service.GetSingleWaterBody(id);
            if (obj == null)
            {
                return NotFound();
            }
            await _service.DeleteWaterBody(id);
            return RedirectToAction("Index");
        }
    }
}
