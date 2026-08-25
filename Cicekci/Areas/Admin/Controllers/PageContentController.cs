using Cicekci.Data;
using Cicekci.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cicekci.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PageContentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PageContentController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Edit: tek satırlık SiteContent tablosunu düzenle (Id=1)
        public IActionResult Edit()
        {
            var content = _db.SiteContents.FirstOrDefault();
            if (content == null)
            {
                content = new SiteContent();
                _db.SiteContents.Add(content);
                _db.SaveChanges();
            }
            return View(content);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, SiteContent model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var content = _db.SiteContents.Find(id);
            if (content == null) return NotFound();

            // Anasayfa
            content.HomeHeroTitle = model.HomeHeroTitle;
            content.HomeHeroSubtitle = model.HomeHeroSubtitle;

            // Hakkımızda
            content.AboutTitle = model.AboutTitle;
            content.AboutMainText = model.AboutMainText;
            content.AboutDetailText = model.AboutDetailText;
            content.StatYears = model.StatYears;
            content.StatCustomers = model.StatCustomers;
            content.StatProducts = model.StatProducts;

            // İletişim
            content.ContactAddress = model.ContactAddress;
            content.ContactPhone = model.ContactPhone;
            content.ContactEmail = model.ContactEmail;
            content.ContactWorkingHours = model.ContactWorkingHours;

            _db.SaveChanges();
            TempData["Success"] = "Site içerikleri güncellendi.";
            return RedirectToAction("Edit");
        }
    }
}
