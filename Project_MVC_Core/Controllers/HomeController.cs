using Microsoft.AspNetCore.Mvc;

namespace Project_MVC_Core.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
