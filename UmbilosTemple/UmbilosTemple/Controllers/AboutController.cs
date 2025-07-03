using Microsoft.AspNetCore.Mvc;

namespace UmbilosTemple.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
