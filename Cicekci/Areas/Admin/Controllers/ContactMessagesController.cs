using Cicekci.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cicekci.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ContactMessagesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ContactMessagesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Index: tüm mesajları listele
        public IActionResult Index()
        {
            var messages = _db.ContactMessages
                .OrderByDescending(m => m.SentDate)
                .ToList();
            return View(messages);
        }

        // Details: tek mesajı görüntüle, okundu olarak işaretle
        public IActionResult Details(int id)
        {
            var message = _db.ContactMessages.Find(id);
            if (message == null) return NotFound();

            if (!message.IsRead)
            {
                message.IsRead = true;
                _db.SaveChanges();
            }

            return View(message);
        }

        // Delete GET
        public IActionResult Delete(int id)
        {
            var message = _db.ContactMessages.Find(id);
            if (message == null) return NotFound();
            return View(message);
        }

        // Delete POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var message = _db.ContactMessages.Find(id);
            if (message == null) return NotFound();

            _db.ContactMessages.Remove(message);
            _db.SaveChanges();
            TempData["Success"] = "Mesaj silindi.";
            return RedirectToAction("Index");
        }
    }
}
