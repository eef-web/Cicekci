using Cicekci.Data;
using Cicekci.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cicekci.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly ApplicationDbContext _db;

        public CartController(CartService cartService, ApplicationDbContext db)
        {
            _cartService = cartService;
            _db = db;
        }

        // Sepeti göster
        public IActionResult Index()
        {
            var items = _cartService.GetCartItems();
            ViewBag.Total = _cartService.GetTotal();
            return View(items);
        }

        // Sepete ürün ekle
        public IActionResult Add(int productId, int quantity = 1)
        {
            var product = _db.Products.Find(productId);
            if (product == null) return NotFound();

            _cartService.AddItem(productId, product.Name, product.Price, quantity, product.ImageUrl);
            TempData["Success"] = $"{product.Name} sepete eklendi.";
            return RedirectToAction("Index");
        }

        // Sepetten ürün çıkar
        public IActionResult Remove(int productId)
        {
            _cartService.RemoveItem(productId);
            return RedirectToAction("Index");
        }

        // Sepetteki adet güncelle
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                _cartService.RemoveItem(productId);
            }
            else
            {
                _cartService.UpdateQuantity(productId, quantity);
            }
            return RedirectToAction("Index");
        }

        // Sepeti boşalt
        public IActionResult Clear()
        {
            _cartService.Clear();
            return RedirectToAction("Index");
        }
    }
}
