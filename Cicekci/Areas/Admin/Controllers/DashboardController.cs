using Cicekci.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        // Dinamik dashboard: veriler veri tabanından toplanır ve sayfada gösterilir
        public IActionResult Index()
        {
            ViewBag.ProductCount = _db.Products.Count();
            ViewBag.CategoryCount = _db.Categories.Count();
            ViewBag.MessageCount = _db.ContactMessages.Count();
            ViewBag.UnreadMessageCount = _db.ContactMessages.Count(m => !m.IsRead);
            ViewBag.OutOfStockCount = _db.Products.Count(p => !p.InStock);

            ViewBag.RecentMessages = _db.ContactMessages
                .OrderByDescending(m => m.SentDate)
                .Take(5)
                .ToList();

            ViewBag.RecentProducts = _db.Products
                .OrderByDescending(p => p.CreatedDate)
                .Take(5)
                .ToList();

            return View();
        }
    }
}
