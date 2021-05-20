using Microsoft.AspNetCore.Mvc;

namespace HomeTreatment
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
