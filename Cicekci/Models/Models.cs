using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cicekci.Models
{
    // Çiçek kategorisi (Doğum Günü, Sevgililer Günü, Cenaze vb.)
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Kategori adı 2-50 karakter olmalıdır.")]
        [Display(Name = "Kategori Adı")]
        public string Name { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Açıklama en fazla 300 karakter olabilir.")]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        // İlişki: bu kategorideki ürünler
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }

    // Çiçek ürünü
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ürün adı 2-100 karakter olmalıdır.")]
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Açıklama 10-500 karakter olmalıdır.")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0.01, 100000, ErrorMessage = "Fiyat 0.01 ile 100.000 arasında olmalıdır.")]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Fiyat (TL)")]
        public decimal Price { get; set; }

        [Url(ErrorMessage = "Geçerli bir URL giriniz.")]
        [Display(Name = "Görsel URL")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }

        [Display(Name = "Stokta Var mı?")]
        public bool InStock { get; set; } = true;

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    // Sepetteki ürün (session'da tutulur, veri tabanı tablosu değildir)
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

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ad Soyad 2-100 karakter olmalıdır.")]
        [Display(Name = "Ad Soyad")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [StringLength(100)]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [StringLength(20)]
        [Display(Name = "Telefon")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Mesaj zorunludur.")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Mesaj 5-1000 karakter olmalıdır.")]
        [Display(Name = "Mesaj")]
        public string Message { get; set; } = string.Empty;

        [Display(Name = "Gönderilme Tarihi")]
        public DateTime SentDate { get; set; } = DateTime.Now;

        [Display(Name = "Okundu mu?")]
        public bool IsRead { get; set; } = false;
    }

    // Sitenin dinamik içerikleri (Anasayfa, Hakkımızda, İletişim) - tek satırlık ayar tablosu
    public class SiteContent
    {
        public int Id { get; set; }

        // --- Anasayfa ---
        [Required, StringLength(150)]
        [Display(Name = "Anasayfa Başlık")]
        public string HomeHeroTitle { get; set; } = "Sevdiklerinize Çiçek Gönderin";

        [Required, StringLength(250)]
        [Display(Name = "Anasayfa Alt Başlık")]
        public string HomeHeroSubtitle { get; set; } = "Taze çiçekler, hızlı teslimat, mutlu yüzler.";

        // --- Hakkımızda ---
        [Required, StringLength(150)]
        [Display(Name = "Hakkımızda Başlık")]
        public string AboutTitle { get; set; } = "Hakkımızda";

        [Required, StringLength(1000)]
        [Display(Name = "Hakkımızda Ana Metin")]
        public string AboutMainText { get; set; } = "Çiçekçi olarak 2010 yılından beri sevdiklerinize en taze çiçekleri ulaştırıyoruz.";

        [Required, StringLength(1500)]
        [Display(Name = "Hakkımızda Detay Metin")]
        public string AboutDetailText { get; set; } = "İstanbul merkezli işletmemiz, her buketi özenle hazırlar ve aynı gün teslimat hizmeti sunar.";

        [Required, StringLength(20)]
        [Display(Name = "Yıllık Tecrübe")]
        public string StatYears { get; set; } = "15+";

        [Required, StringLength(20)]
        [Display(Name = "Mutlu Müşteri Sayısı")]
        public string StatCustomers { get; set; } = "50.000+";

        [Required, StringLength(20)]
        [Display(Name = "Çiçek Çeşidi")]
        public string StatProducts { get; set; } = "500+";

        // --- İletişim ---
        [Required, StringLength(150)]
        [Display(Name = "Adres")]
        public string ContactAddress { get; set; } = "Antalya, Türkiye";

        [Required, Phone, StringLength(30)]
        [Display(Name = "Telefon")]
        public string ContactPhone { get; set; } = "+90 242 000 00 00";

        [Required, EmailAddress, StringLength(100)]
        [Display(Name = "E-posta")]
        public string ContactEmail { get; set; } = "info@cicekci.com";

        [Required, StringLength(150)]
        [Display(Name = "Çalışma Saatleri")]
        public string ContactWorkingHours { get; set; } = "Pazartesi - Cumartesi: 09:00 - 21:00 / Pazar: 10:00 - 18:00";
    }

    // Yönetim paneli kullanıcısı (kimlik doğrulama için)
    public class AdminUser
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string PasswordSalt { get; set; } = string.Empty;

        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = "Yönetici";
    }

    // Admin giriş formu için ViewModel
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; } = string.Empty;
    }
}
