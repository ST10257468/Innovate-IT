using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;
using UmbiloTemple.Models;

namespace UmbiloTemple.Controllers
{
    public class AdminEventsController : Controller
    {
        private readonly FirestoreDb _firestore;
        private readonly IWebHostEnvironment _env;

        public AdminEventsController(FirestoreDb firestore, IWebHostEnvironment env)
        {
            _firestore = firestore;
            _env = env;
        }

        private bool IsAdmin()
            => HttpContext.Session.GetString("UserRole") == "admin";

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var snapshot = await _firestore.Collection("Events").GetSnapshotAsync();
            var events = snapshot.Documents.Select(d => d.ConvertTo<Event>()).ToList();
            return View(events);
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Event model, IFormFile? imageFile)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (imageFile != null)
            {
                var path = Path.Combine(_env.WebRootPath, "uploads", imageFile.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                await imageFile.CopyToAsync(stream);
                model.ImagePath = "/uploads/" + imageFile.FileName;
            }

            var docRef = _firestore.Collection("Events").Document();
            await docRef.SetAsync(model);

            TempData["Success"] = "Event added successfully.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var query = await _firestore.Collection("Events").WhereEqualTo("Id", id).GetSnapshotAsync();
            foreach (var doc in query.Documents)
                await doc.Reference.DeleteAsync();

            TempData["Success"] = "Event deleted.";
            return RedirectToAction("Index");
        }
    }
}
