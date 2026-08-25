using Cicekci.Data;
using Cicekci.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cicekci.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Index: tüm kategorileri listele
        public IActionResult Index()
        {
            var categories = _db.Categories
                .Include(c => c.Products)
                .OrderBy(c => c.Name)
                .ToList();
            return View(categories);
        }

        // Create GET
        public IActionResult Create()
        {
            return View(new Category());
        }

        // Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            if (!ModelState.IsValid) return View(model);

            _db.Categories.Add(model);
            _db.SaveChanges();
            TempData["Success"] = "Kategori başarıyla oluşturuldu.";
            return RedirectToAction("Index");
        }

        // Edit GET
        public IActionResult Edit(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var category = _db.Categories.Find(id);
            if (category == null) return NotFound();

            category.Name = model.Name;
            category.Description = model.Description;
            _db.SaveChanges();

            TempData["Success"] = "Kategori başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        // Delete GET (onay sayfası)
        public IActionResult Delete(int id)
        {
            var category = _db.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }

        // Delete POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null) return NotFound();

            var hasProducts = _db.Products.Any(p => p.CategoryId == id);
            if (hasProducts)
            {
                TempData["Error"] = "Bu kategoriye ait ürünler var. Önce ürünleri silin veya başka kategoriye taşıyın.";
                return RedirectToAction("Index");
            }

            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["Success"] = "Kategori silindi.";
            return RedirectToAction("Index");
        }
    }
}
