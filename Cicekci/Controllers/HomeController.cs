using Cicekci.Data;
using Cicekci.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cicekci.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // Ana sayfada öne çıkan ürünler (son eklenen 4)
            var featured = _db.Products.OrderByDescending(p => p.CreatedDate).Take(4).ToList();
            ViewBag.Categories = _db.Categories.ToList();
            return View(featured);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(Models.ContactMessage model)
        {
            if (ModelState.IsValid)
            {
                _db.ContactMessages.Add(model);
                _db.SaveChanges();
                TempData["Success"] = "Mesajınız başarıyla gönderildi! En kısa sürede sizinle iletişime geçeceğiz.";
                return RedirectToAction("Contact");
            }
            return View(model);
        }
    }
}
