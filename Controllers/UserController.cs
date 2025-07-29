using Microsoft.AspNetCore.Mvc;

namespace QUickDish.API.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
