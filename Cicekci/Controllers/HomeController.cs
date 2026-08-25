using Cicekci.Data;
using Cicekci.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cicekci.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext db, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // Anasayfa: veri tabanından ürünler ve site içerikleri çekilir
        public IActionResult Index()
        {
            var content = _db.SiteContents.FirstOrDefault() ?? new SiteContent();
            var products = _db.Products
                .Include(p => p.Category)
                .Where(p => p.InStock)
                .OrderByDescending(p => p.CreatedDate)
                .Take(6)
                .ToList();

            ViewBag.Content = content;
            ViewBag.Products = products;
            return View();
        }

        // Hakkımızda sayfası — içerik veri tabanından gelir
        public IActionResult About()
        {
            var content = _db.SiteContents.FirstOrDefault() ?? new SiteContent();
            return View(content);
        }

        // İletişim sayfası — POST'ta mesajı veri tabanına kaydet
        [HttpGet]
        public IActionResult Contact()
        {
            var content = _db.SiteContents.FirstOrDefault() ?? new SiteContent();
            ViewBag.Content = content;
            return View(new ContactMessage());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactMessage model)
        {
            var content = _db.SiteContents.FirstOrDefault() ?? new SiteContent();
            ViewBag.Content = content;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.SentDate = DateTime.Now;
            _db.ContactMessages.Add(model);
            _db.SaveChanges();

            TempData["Success"] = "Mesajınız başarıyla iletildi! En kısa sürede size dönüş yapacağız.";
            return RedirectToAction("Contact");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
