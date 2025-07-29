using Microsoft.AspNetCore.Mvc;

namespace QUickDish.API.Controllers
{
    public class Order_ItemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
