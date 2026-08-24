using Cicekci.Data;
using Cicekci.Models;
using Cicekci.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cicekci.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly CartService _cart;

        public ProductsController(ApplicationDbContext db, CartService cart)
        {
            _db = db;
            _cart = cart;
        }

        // Tüm ürünler (kategori filtreleme opsiyonel)
        public IActionResult Index(int? categoryId, string? search)
        {
            var products = _db.Products.Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId);
                ViewBag.SelectedCategory = _db.Categories.Find(categoryId.Value)?.Name;
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                products = products.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
                ViewBag.Search = search;
            }

            ViewBag.Categories = _db.Categories.ToList();
            ViewBag.CartCount = _cart.GetCount();
            return View(products.ToList());
        }

        // Ürün detay sayfası
        public IActionResult Details(int id)
        {
            var product = _db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        // Sepete ekleme (AJAX veya form)
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _db.Products.Find(productId);
            if (product == null) return NotFound();

            _cart.AddToCart(new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = quantity,
                ImageUrl = product.ImageUrl
            });

            TempData["Success"] = $"'{product.Name}' sepete eklendi!";
            return RedirectToAction("Index");
        }
    }
}
