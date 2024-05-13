using BLL.Models.AuthModels;
using BLL.Models.TicketTypeModels;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace Ticket.Controllers
{
    [Authorize(Roles = ("Admin"))]
    public class UserController : Controller
    {
        private readonly IAuthService _authService;
        public UserController(IAuthService authService)
        {
            _authService = authService;
        }
        public async  Task<IActionResult> Index(string role)
        {
            var users = await _authService.GetUsersByRole(role);
            return View(users);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterUser obj)
        {
            if (ModelState.IsValid)
            {
                await _authService.RegisterUser(obj);
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //GET - DELETE
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null )
            {
                return NotFound();
            }
            var obj = await _authService.GetUserById(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(string id)
        {
            var obj = await _authService.GetUserById(id);
            if (obj == null)
            {
                return NotFound();
            }
            await _authService.DeleteUser(id);
            return RedirectToAction("Index");
        }


    }
}
