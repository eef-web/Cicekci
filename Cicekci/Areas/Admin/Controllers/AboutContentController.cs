using Cicekci.Data;
using Cicekci.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cicekci.Areas.Admin.Controllers
{
    // Hakkımızda içeriğini yönetir (Index: görüntüle, Edit: düzenle)
    [Area("Admin")]
    [Authorize]
    public class AboutContentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AboutContentController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Index: Hakkımızda içeriğinin özetini görüntüle
        public IActionResult Index()
        {
            var content = GetOrCreateContent();
            return View(content);
        }

        // Edit GET: düzenleme formunu göster
        public IActionResult Edit()
        {
            var content = GetOrCreateContent();
            var model = new AboutContentViewModel
            {
                Id = content.Id,
                AboutTitle = content.AboutTitle,
                AboutMainText = content.AboutMainText,
                AboutDetailText = content.AboutDetailText,
                StatYears = content.StatYears,
                StatCustomers = content.StatCustomers,
                StatProducts = content.StatProducts
            };
            return View(model);
        }

        // Edit POST: sadece Hakkımızda alanlarını güncelle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AboutContentViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var content = _db.SiteContents.Find(model.Id);
            if (content == null) return NotFound();

            content.AboutTitle = model.AboutTitle;
            content.AboutMainText = model.AboutMainText;
            content.AboutDetailText = model.AboutDetailText;
            content.StatYears = model.StatYears;
            content.StatCustomers = model.StatCustomers;
            content.StatProducts = model.StatProducts;
            _db.SaveChanges();

            TempData["Success"] = "Hakkımızda içeriği güncellendi.";
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
