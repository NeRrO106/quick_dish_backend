using Microsoft.AspNetCore.Mvc;

namespace QUickDish.API.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
