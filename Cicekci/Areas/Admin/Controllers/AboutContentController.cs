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
            var content = _db.SiteContents.FirstOrDefault();
            if (content == null)
            {
                // İçerik henüz elle girilmemişse düzenleme formuna yönlendir
                TempData["Info"] = "Hakkımızda içeriği henüz oluşturulmadı. Aşağıdaki formu doldurarak ekleyebilirsiniz.";
                return RedirectToAction("Edit");
            }
            return View(content);
        }

        // Edit GET: düzenleme formunu göster
        public IActionResult Edit()
        {
            var content = _db.SiteContents.FirstOrDefault();
            var model = new AboutContentViewModel
            {
                Id = content?.Id ?? 0,
                AboutTitle = content?.AboutTitle ?? string.Empty,
                AboutMainText = content?.AboutMainText ?? string.Empty,
                AboutDetailText = content?.AboutDetailText ?? string.Empty,
                StatYears = content?.StatYears ?? string.Empty,
                StatCustomers = content?.StatCustomers ?? string.Empty,
                StatProducts = content?.StatProducts ?? string.Empty
            };
            return View(model);
        }

        // Edit POST: Hakkımızda alanlarını günceller; içerik yoksa elle girilen değerlerle oluşturur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AboutContentViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var content = model.Id > 0 ? _db.SiteContents.Find(model.Id) : null;

            if (content == null)
            {
                content = new SiteContent
                {
                    AboutTitle = model.AboutTitle,
                    AboutMainText = model.AboutMainText,
                    AboutDetailText = model.AboutDetailText,
                    StatYears = model.StatYears,
                    StatCustomers = model.StatCustomers,
                    StatProducts = model.StatProducts
                };
                _db.SiteContents.Add(content);
            }
            else
            {
                content.AboutTitle = model.AboutTitle;
                content.AboutMainText = model.AboutMainText;
                content.AboutDetailText = model.AboutDetailText;
                content.StatYears = model.StatYears;
                content.StatCustomers = model.StatCustomers;
                content.StatProducts = model.StatProducts;
            }

            _db.SaveChanges();

            TempData["Success"] = "Hakkımızda içeriği kaydedildi.";
            return RedirectToAction("Index");
        }
    }
}
