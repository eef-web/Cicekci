using Cicekci.Models;
using Microsoft.EntityFrameworkCore;

namespace Cicekci.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Kategori → Ürün bire-çok ilişkisi
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    // İlk açılışta örnek veri ekler
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext db)
        {
            if (db.Categories.Any()) return;

            var categories = new List<Category>
            {
                new Category { Name = "Doğum Günü", Description = "Doğum günü için özel çiçek aranjmanları" },
                new Category { Name = "Sevgililer Günü", Description = "Aşkın sembolü kırmızı güller ve daha fazlası" },
                new Category { Name = "Cenaze", Description = "Başsağlığı için uygun çiçekler" },
                new Category { Name = "Ev & Ofis", Description = "Mekanlara renk katak çiçekler" },
            };

            db.Categories.AddRange(categories);
            db.SaveChanges();

            var products = new List<Product>
            {
                new Product { Name = "Kırmızı Gül Buketi", Description = "25 adet kırmızı gülden oluşan şık buket.", Price = 450, ImageUrl = "https://images.unsplash.com/photo-1518895949257-7621c66c12e9?w=600", CategoryId = 2, InStock = true },
                new Product { Name = "Karışık Mevsim Buketi", Description = "Mevsime göre seçilmiş taze çiçeklerle renkli buket.", Price = 350, ImageUrl = "https://images.unsplash.com/photo-1561181286-d3fee7d44314?w=600", CategoryId = 1, InStock = true },
                new Product { Name = "Papatya Buketi", Description = "100 adet taze papatya ile neşeli buket.", Price = 280, ImageUrl = "https://images.unsplash.com/photo-1591886967495-eb1e6b80b4c9?w=600", CategoryId = 1, InStock = true },
                new Product { Name = "Orkide Saksı", Description = "Uzun ömürlü beyaz orkide saksı çiçeği.", Price = 520, ImageUrl = "https://images.unsplash.com/photo-1567748157439-651aca2ff064?w=600", CategoryId = 4, InStock = true },
                new Product { Name = "Karanfil Buketi", Description = "Beyaz ve pembe karanfillerle zarif buket.", Price = 300, ImageUrl = "https://images.unsplash.com/photo-1606146914785-2a4a3b4b1d12?w=600", CategoryId = 3, InStock = true },
                new Product { Name = "Lavanta Buketi", Description = "Mor lavantalarla hoş kokulu buket.", Price = 380, ImageUrl = "https://images.unsplash.com/photo-1490750967868-88aa4481c6a8?w=600", CategoryId = 4, InStock = true },
                new Product { Name = "Yonca Çelenk", Description = "Cenaze törenleri için beyaz yonca çelenk.", Price = 600, ImageUrl = "https://images.unsplash.com/photo-1606041008023-472dfb5e3344?w=600", CategoryId = 3, InStock = true },
                new Product { Name = "Peyzaj Saksı", Description = "Ofis ve ev için bakımı kolay yeşil bitki.", Price = 250, ImageUrl = "https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=600", CategoryId = 4, InStock = true },
                new Product { Name = "Gül & Lale Karışımı", Description = "Kırmızı gül ve beyaz lale karışımı özel buket.", Price = 480, ImageUrl = "https://images.unsplash.com/photo-1520763185298-1b58670fba46?w=600", CategoryId = 2, InStock = true },
                new Product { Name = "Sümbül Saksı", Description = "Bahar müjdesi sümbül saksısı.", Price = 220, ImageUrl = "https://images.unsplash.com/photo-1612544448445-10c0d30e0b89?w=600", CategoryId = 1, InStock = true },
                new Product { Name = "Bambu Şans Bitkisi", Description = "Ofise şans getiren bambu bitkisi.", Price = 190, ImageUrl = "https://images.unsplash.com/photo-1572688484438-313b3ac4146d?w=600", CategoryId = 4, InStock = true },
                new Product { Name = "Pembe Gül Buketi", Description = "20 adet pembe gül ile romantik buket.", Price = 410, ImageUrl = "https://images.unsplash.com/photo-1457089328109-e5d9bd499191?w=600", CategoryId = 2, InStock = true },
            };

            db.Products.AddRange(products);
            db.SaveChanges();
        }
    }
}
