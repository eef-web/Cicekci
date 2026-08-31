using Cicekci.Data;
using Cicekci.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cicekci.Areas.Admin.Controllers
{
    // İletişim içeriğini yönetir (Index: görüntüle, Edit: düzenle)
    [Area("Admin")]
    [Authorize]
    public class ContactContentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ContactContentController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Index: İletişim içeriğinin özetini görüntüle
        public IActionResult Index()
        {
            var content = GetOrCreateContent();
            return View(content);
        }

        // Edit GET: düzenleme formunu göster
        public IActionResult Edit()
        {
            var content = GetOrCreateContent();
            var model = new ContactContentViewModel
            {
                Id = content.Id,
                ContactAddress = content.ContactAddress,
                ContactPhone = content.ContactPhone,
                ContactEmail = content.ContactEmail,
                ContactWorkingHours = content.ContactWorkingHours
            };
            return View(model);
        }

        // Edit POST: sadece İletişim alanlarını güncelle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ContactContentViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var content = _db.SiteContents.Find(model.Id);
            if (content == null) return NotFound();

            content.ContactAddress = model.ContactAddress;
            content.ContactPhone = model.ContactPhone;
            content.ContactEmail = model.ContactEmail;
            content.ContactWorkingHours = model.ContactWorkingHours;
            _db.SaveChanges();

            TempData["Success"] = "İletişim içeriği güncellendi.";
            return RedirectToAction("Index");
        }

        private SiteContent GetOrCreateContent()
        {
            var content = _db.SiteContents.FirstOrDefault();
            if (content == null)
            {
                content = new SiteContent();
                _db.SiteContents.Add(content);
                _db.SaveChanges();
            }
            return content;
        }
    }
}
