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
            var content = _db.SiteContents.FirstOrDefault();
            if (content == null)
            {
                // İçerik henüz elle girilmemişse düzenleme formuna yönlendir
                TempData["Info"] = "İletişim içeriği henüz oluşturulmadı. Aşağıdaki formu doldurarak ekleyebilirsiniz.";
                return RedirectToAction("Edit");
            }
            return View(content);
        }

        // Edit GET: düzenleme formunu göster
        public IActionResult Edit()
        {
            var content = _db.SiteContents.FirstOrDefault();
            var model = new ContactContentViewModel
            {
                Id = content?.Id ?? 0,
                ContactAddress = content?.ContactAddress ?? string.Empty,
                ContactPhone = content?.ContactPhone ?? string.Empty,
                ContactEmail = content?.ContactEmail ?? string.Empty,
                ContactWorkingHours = content?.ContactWorkingHours ?? string.Empty,
                ContactTitle = content?.ContactTitle,
            };
            return View(model);
        }

        // Edit POST: İletişim alanlarını günceller; içerik yoksa elle girilen değerlerle oluşturur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ContactContentViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var content = _db.SiteContents.FirstOrDefault();

            if (content == null)
            {
                content = new SiteContent
                {
                    ContactAddress = model.ContactAddress,
                    ContactPhone = model.ContactPhone,
                    ContactEmail = model.ContactEmail,
                    ContactWorkingHours = model.ContactWorkingHours,
                    ContactTitle = model.ContactTitle,
                };
                _db.SiteContents.Add(content);
            }
            else
            {
                content.ContactAddress = model.ContactAddress;
                content.ContactPhone = model.ContactPhone;
                content.ContactEmail = model.ContactEmail;
                content.ContactWorkingHours = model.ContactWorkingHours;
                content.ContactTitle = model.ContactTitle;
            }

            _db.SaveChanges();

            TempData["Success"] = "İletişim içeriği kaydedildi.";
            return RedirectToAction("Index");
        }
    }
}
