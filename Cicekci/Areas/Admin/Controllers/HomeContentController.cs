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
            var content = _db.SiteContents.FirstOrDefault();
            if (content == null)
            {
                // İçerik henüz elle girilmemişse düzenleme formuna yönlendir
                TempData["Info"] = "Anasayfa içeriği henüz oluşturulmadı. Aşağıdaki formu doldurarak ekleyebilirsiniz.";
                return RedirectToAction("Edit");
            }
            return View(content);
        }

        // Edit GET: düzenleme formunu göster
        public IActionResult Edit()
        {
            var content = _db.SiteContents.FirstOrDefault();
            var model = new HomeContentViewModel
            {
                Id = content?.Id ?? 0,
                HomeHeroTitle = content?.HomeHeroTitle ?? string.Empty,
                HomeHeroSubtitle = content?.HomeHeroSubtitle ?? string.Empty
            };
            return View(model);
        }

        // Edit POST: Anasayfa alanlarını günceller; içerik yoksa elle girilen değerlerle oluşturur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HomeContentViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var content = model.Id > 0 ? _db.SiteContents.Find(model.Id) : null;

            if (content == null)
            {
                content = new SiteContent
                {
                    HomeHeroTitle = model.HomeHeroTitle,
                    HomeHeroSubtitle = model.HomeHeroSubtitle
                };
                _db.SiteContents.Add(content);
            }
            else
            {
                content.HomeHeroTitle = model.HomeHeroTitle;
                content.HomeHeroSubtitle = model.HomeHeroSubtitle;
            }

            _db.SaveChanges();

            TempData["Success"] = "Anasayfa içeriği kaydedildi.";
            return RedirectToAction("Index");
        }
    }
}
