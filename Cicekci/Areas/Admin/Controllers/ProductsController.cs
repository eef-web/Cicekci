using Cicekci.Data;
using Cicekci.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Cicekci.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Index: tüm ürünleri listele
        public IActionResult Index()
        {
            var products = _db.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedDate)
                .ToList();
            return View(products);
        }

        // Create GET
        public IActionResult Create()
        {
            ViewBag.Categories = GetCategorySelectList();
            return View(new Product());
        }

        // Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = GetCategorySelectList();
                return View(model);
            }

            model.CreatedDate = DateTime.Now;
            _db.Products.Add(model);
            _db.SaveChanges();
            TempData["Success"] = "Ürün başarıyla oluşturuldu.";
            return RedirectToAction("Index");
        }

        // Edit GET
        public IActionResult Edit(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null) return NotFound();
            ViewBag.Categories = GetCategorySelectList();
            return View(product);
        }

        // Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = GetCategorySelectList();
                return View(model);
            }

            var product = _db.Products.Find(id);
            if (product == null) return NotFound();

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.ImageUrl = model.ImageUrl;
            product.CategoryId = model.CategoryId;
            product.InStock = model.InStock;
            _db.SaveChanges();

            TempData["Success"] = "Ürün başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        // Delete GET (onay sayfası)
        public IActionResult Delete(int id)
        {
            var product = _db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        // Delete POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null) return NotFound();

            _db.Products.Remove(product);
            _db.SaveChanges();
            TempData["Success"] = "Ürün silindi.";
            return RedirectToAction("Index");
        }

        private List<SelectListItem> GetCategorySelectList()
        {
            return _db.Categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();
        }
    }
}
