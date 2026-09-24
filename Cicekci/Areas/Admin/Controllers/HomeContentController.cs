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
                HomeHeroSubtitle = content?.HomeHeroSubtitle ?? string.Empty,
                HomeHeroImageUrl = content?.HomeHeroImageUrl,
                FeaturedTitle = content?.FeaturedTitle,
                WhyTitle = content?.WhyTitle,
                Benefit1Title = content?.Benefit1Title,
                Benefit1Description = content?.Benefit1Description,
                Benefit2Title = content?.Benefit2Title,
                Benefit2Description = content?.Benefit2Description,
                Benefit3Title = content?.Benefit3Title,
                Benefit3Description = content?.Benefit3Description,
                Benefit4Title = content?.Benefit4Title,
                Benefit4Description = content?.Benefit4Description,
            };
            return View(model);
        }

        // Edit POST: Anasayfa alanlarını günceller; içerik yoksa elle girilen değerlerle oluşturur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HomeContentViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var content = _db.SiteContents.FirstOrDefault();

            if (content == null)
            {
                content = new SiteContent
                {
                    HomeHeroTitle = model.HomeHeroTitle,
                    HomeHeroSubtitle = model.HomeHeroSubtitle,
                    HomeHeroImageUrl = model.HomeHeroImageUrl,
                    FeaturedTitle = model.FeaturedTitle,
                    WhyTitle = model.WhyTitle,
                    Benefit1Title = model.Benefit1Title,
                    Benefit1Description = model.Benefit1Description,
                    Benefit2Title = model.Benefit2Title,
                    Benefit2Description = model.Benefit2Description,
                    Benefit3Title = model.Benefit3Title,
                    Benefit3Description = model.Benefit3Description,
                    Benefit4Title = model.Benefit4Title,
                    Benefit4Description = model.Benefit4Description,
                };
                _db.SiteContents.Add(content);
            }
            else
            {
                content.HomeHeroTitle = model.HomeHeroTitle;
                content.HomeHeroSubtitle = model.HomeHeroSubtitle;
                content.HomeHeroImageUrl = model.HomeHeroImageUrl;
                content.FeaturedTitle = model.FeaturedTitle;
                content.WhyTitle = model.WhyTitle;
                content.Benefit1Title = model.Benefit1Title;
                content.Benefit1Description = model.Benefit1Description;
                content.Benefit2Title = model.Benefit2Title;
                content.Benefit2Description = model.Benefit2Description;
                content.Benefit3Title = model.Benefit3Title;
                content.Benefit3Description = model.Benefit3Description;
                content.Benefit4Title = model.Benefit4Title;
                content.Benefit4Description = model.Benefit4Description;
            }

            _db.SaveChanges();

            TempData["Success"] = "Anasayfa içeriği kaydedildi.";
            return RedirectToAction("Index");
        }
    }
}
