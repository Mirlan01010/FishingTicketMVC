using BLL.Models.AuthModels;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ticket.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthService  _authService;
        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel user)
        {
            var response = await _authService.Login(user);
            if (response.JwtToken == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Response.Cookies.Append("jwtToken", response.JwtToken, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    HttpOnly = true, // Токен доступен только серверу
                    Secure = true, // Требуется HTTPS для отправки токена
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict // Уровень SameSite
                });
                return Redirect("/");
            }
        }
        
    }
}
