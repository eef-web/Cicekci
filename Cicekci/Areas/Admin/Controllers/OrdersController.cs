using Cicekci.Data;
using Cicekci.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cicekci.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Tüm siparişleri listele
        public IActionResult Index(string? status = null)
        {
            var query = _db.Orders
                .Include(o => o.Items)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.Status == status);
                ViewBag.CurrentFilter = status;
            }

            var orders = query.OrderByDescending(o => o.OrderDate).ToList();

            // Filtre seçenekleri
            ViewBag.StatusCounts = new Dictionary<string, int>
            {
                { "Hazırlanıyor", _db.Orders.Count(o => o.Status == "Hazırlanıyor") },
                { "Gönderildi", _db.Orders.Count(o => o.Status == "Gönderildi") },
                { "Teslim Edildi", _db.Orders.Count(o => o.Status == "Teslim Edildi") },
                { "İptal", _db.Orders.Count(o => o.Status == "İptal") }
            };

            return View(orders);
        }

        // Sipariş detayı
        public IActionResult Details(int id)
        {
            var order = _db.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // Sipariş durumunu güncelle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string status)
        {
            var order = _db.Orders.Find(id);
            if (order == null) return NotFound();

            order.Status = status;
            _db.SaveChanges();

            TempData["Success"] = $"Sipariş #{id} durumu \"{status}\" olarak güncellendi.";
            return RedirectToAction("Details", new { id = id });
        }

        // Siparişi sil
        public IActionResult Delete(int id)
        {
            var order = _db.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var order = _db.Orders.Find(id);
            if (order == null) return NotFound();

            _db.Orders.Remove(order);
            _db.SaveChanges();

            TempData["Success"] = $"Sipariş #{id} silindi.";
            return RedirectToAction("Index");
        }
    }
}
