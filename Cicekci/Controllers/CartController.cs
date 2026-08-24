using Cicekci.Models;
using Cicekci.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cicekci.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cart;

        public CartController(CartService cart)
        {
            _cart = cart;
        }

        // Sepet sayfası
        public IActionResult Index()
        {
            var items = _cart.GetCart();
            ViewBag.Total = _cart.GetTotal();
            ViewBag.Count = _cart.GetCount();
            return View(items);
        }

        // Sepetten ürün çıkar
        public IActionResult Remove(int productId)
        {
            _cart.RemoveFromCart(productId);
            return RedirectToAction("Index");
        }

        // Adet güncelle
        [HttpPost]
        public IActionResult Update(int productId, int quantity)
        {
            _cart.UpdateQuantity(productId, quantity);
            return RedirectToAction("Index");
        }

        // Siparişi tamamla (örnek)
        public IActionResult Checkout()
        {
            if (_cart.GetCount() == 0)
            {
                TempData["Error"] = "Sepetiniz boş!";
                return RedirectToAction("Index");
            }

            _cart.ClearCart();
            TempData["Success"] = "Siparişiniz başarıyla alındı! Teşekkür ederiz. 🌸";
            return RedirectToAction("Index");
        }
    }
}
