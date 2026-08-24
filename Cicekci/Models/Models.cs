using System.ComponentModel.DataAnnotations;

namespace Cicekci.Models
{
    // Çiçek kategorisi (Doğum Günü, Sevgililer Günü, Cenaze vb.)
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // İlişki: bu kategorideki ürünler
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }

    // Çiçek ürünü
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0, 100000)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public bool InStock { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    // Sepetteki ürün
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
    }

    // İletişim sayfasından gelen mesajlar (veri tabanına kaydedilir)
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        [Required]
        [StringLength(1000)]
        public string Message { get; set; } = string.Empty;

        public DateTime SentDate { get; set; } = DateTime.Now;
    }
}
