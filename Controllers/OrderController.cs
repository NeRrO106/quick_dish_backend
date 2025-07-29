using Microsoft.AspNetCore.Mvc;

namespace QUickDish.API.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
