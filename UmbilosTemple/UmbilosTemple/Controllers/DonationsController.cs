using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UmbilosTemple.Controllers
{
    public class DonationsController : Controller
    {
        // GET: DonationsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: DonationsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DonationsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DonationsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DonationsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DonationsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DonationsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DonationsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
