using Microsoft.AspNetCore.Mvc;
using UmbiloTemple.Models;
using UmbiloTemple.Services;

namespace UmbiloTemple.Controllers
{
    public class ContactController : Controller
    {
        private readonly ContactService _contactService;
        private readonly EmailService _emailService;

        public ContactController(ContactService contactService, EmailService emailService)
        {
            _contactService = contactService;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Index(ContactMessage message)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please complete all fields correctly.";
                return View(message);
            }

            // 1️⃣ Save message to Firestore
            await _contactService.SaveMessageAsync(message);

            // 2️⃣ Send email notification to admin
            string subject = $"📩 New Contact Message from {message.Name}";
            string body = $@"
                <h2>New Contact Message</h2>
                <p><strong>Name:</strong> {message.Name}</p>
                <p><strong>Email:</strong> {message.Email}</p>
                <p><strong>Message:</strong><br/>{message.Message}</p>
                <p><em>Received on {DateTime.Now:dddd, dd MMM yyyy HH:mm}</em></p>";

            await _emailService.SendEmailAsync(subject, body);

            TempData["Success"] = "Your message has been sent successfully!";
            return RedirectToAction("Index");
        }
    }
}
