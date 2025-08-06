using Microsoft.AspNetCore.Mvc;

namespace QUickDish.API.Controllers
{
    public class OrderItemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
