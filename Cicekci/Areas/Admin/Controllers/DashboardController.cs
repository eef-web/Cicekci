using Cicekci.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cicekci.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            ViewBag.ProductCount = _db.Products.Count();
            ViewBag.CategoryCount = _db.Categories.Count();
            ViewBag.MessageCount = _db.ContactMessages.Count();
            ViewBag.UnreadMessageCount = _db.ContactMessages.Count(m => !m.IsRead);
            ViewBag.OutOfStockCount = _db.Products.Count(p => !p.InStock);

            // Sipariş istatistikleri
            ViewBag.OrderCount = _db.Orders.Count();
            ViewBag.PendingOrderCount = _db.Orders.Count(o => o.Status == "Hazırlanıyor");

            // SQLite decimal Sum desteklemez — önce veriyi çek, sonra client-side topla
            ViewBag.Revenue = _db.Orders
                .Where(o => o.Status != "İptal")
                .Select(o => o.TotalAmount)
                .AsEnumerable()
                .Sum();

            ViewBag.RecentMessages = _db.ContactMessages
                .OrderByDescending(m => m.SentDate)
                .Take(5)
                .ToList();

            ViewBag.RecentProducts = _db.Products
                .OrderByDescending(p => p.CreatedDate)
                .Take(5)
                .ToList();

            ViewBag.RecentOrders = _db.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToList();

            return View();
        }
    }
}
