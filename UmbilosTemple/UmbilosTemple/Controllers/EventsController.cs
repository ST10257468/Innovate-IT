using Microsoft.AspNetCore.Mvc;

namespace UmbilosTemple.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
