using Cicekci.Data;
using Cicekci.Models;
using Cicekci.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            ViewBag.ItemCount = items.Sum(i => i.Quantity);
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

        // --- SİPARİŞ ONAYLA (Checkout) ---

        // Checkout formu
        [HttpGet]
        public IActionResult Checkout()
        {
            var items = _cartService.GetCartItems();
            if (!items.Any())
            {
                TempData["Error"] = "Sepetiniz boş. Önce ürün ekleyin.";
                return RedirectToAction("Index");
            }

            ViewBag.Total = _cartService.GetTotal();
            return View(new Order());
        }

        // Checkout formu POST — siparişi veri tabanına kaydet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(Order model)
        {
            var items = _cartService.GetCartItems();
            if (!items.Any())
            {
                TempData["Error"] = "Sepetiniz boş.";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Total = _cartService.GetTotal();
                return View(model);
            }

            // Siparişi oluştur
            model.TotalAmount = _cartService.GetTotal();
            model.OrderDate = DateTime.Now;
            model.Status = "Hazırlanıyor";

            // Sipariş kalemleri
            model.Items = items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Price = i.Price,
                Quantity = i.Quantity
            }).ToList();

            _db.Orders.Add(model);
            _db.SaveChanges();

            // Sepeti temizle
            _cartService.Clear();

            // Onay sayfasına yönlendir
            return RedirectToAction("OrderConfirmation", new { id = model.Id });
        }

        // Sipariş onay sayfası
        [HttpGet]
        public IActionResult OrderConfirmation(int id)
        {
            var order = _db.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);

            if (order == null) return NotFound();
            return View(order);
        }
    }
}
