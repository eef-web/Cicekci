using Cicekci.Data;
using Cicekci.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cicekci.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Tüm ürünleri listele (kategori filtresi opsiyonel)
        public IActionResult Index(int? categoryId)
        {
            var products = _db.Products
                .Include(p => p.Category)
                .Where(p => !categoryId.HasValue || p.CategoryId == categoryId.Value)
                .OrderByDescending(p => p.CreatedDate)
                .ToList();

            var categories = _db.Categories.OrderBy(c => c.Name).ToList();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = categoryId;
            return View(products);
        }

        // Ürün detayı
        public IActionResult Details(int id)
        {
            var product = _db.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();
            return View(product);
        }
    }
}
