using Cicekci.Data;
using Cicekci.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cicekci.Areas.Admin.Controllers
{
    // Anasayfa içeriğini yönetir (Index: görüntüle, Edit: düzenle)
    [Area("Admin")]
    [Authorize]
    public class HomeContentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeContentController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Index: Anasayfa içeriğinin özetini görüntüle
        public IActionResult Index()
        {
            var content = GetOrCreateContent();
            return View(content);
        }

        // Edit GET: düzenleme formunu göster
        public IActionResult Edit()
        {
            var content = GetOrCreateContent();
            var model = new HomeContentViewModel
            {
                Id = content.Id,
                HomeHeroTitle = content.HomeHeroTitle,
                HomeHeroSubtitle = content.HomeHeroSubtitle
            };
            return View(model);
        }

        // Edit POST: sadece Anasayfa alanlarını güncelle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HomeContentViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var content = _db.SiteContents.Find(model.Id);
            if (content == null) return NotFound();

            content.HomeHeroTitle = model.HomeHeroTitle;
            content.HomeHeroSubtitle = model.HomeHeroSubtitle;
            _db.SaveChanges();

            TempData["Success"] = "Anasayfa içeriği güncellendi.";
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
