using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;
using UmbiloTemple.Models;

namespace UmbiloTemple.Controllers
{
    public class AdminGalleryController : Controller
    {
        private readonly FirestoreDb _firestore;
        private readonly IWebHostEnvironment _env;

        public AdminGalleryController(FirestoreDb firestore, IWebHostEnvironment env)
        {
            _firestore = firestore;
            _env = env;
        }

        private bool IsAdmin()
            => HttpContext.Session.GetString("UserRole") == "admin";

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var snapshot = await _firestore.Collection("Gallery").GetSnapshotAsync();
            var images = snapshot.Documents.Select(d => d.ConvertTo<GalleryImage>()).ToList();
            return View(images);
        }

        [HttpGet]
        public IActionResult Upload()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(GalleryImage model, IFormFile? imageFile)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (imageFile != null)
            {
                var uploadDir = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadDir);

                var filePath = Path.Combine(uploadDir, imageFile.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(stream);
                model.ImageUrl = "/uploads/" + imageFile.FileName;
            }

            var docRef = _firestore.Collection("Gallery").Document();
            await docRef.SetAsync(model);

            TempData["Success"] = "Image uploaded successfully.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var query = await _firestore.Collection("Gallery").WhereEqualTo("Id", id).GetSnapshotAsync();
            foreach (var doc in query.Documents)
                await doc.Reference.DeleteAsync();

            TempData["Success"] = "Image deleted.";
            return RedirectToAction("Index");
        }
    }
}
