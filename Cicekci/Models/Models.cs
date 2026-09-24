using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

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

    // Sipariş (veri tabanına kaydedilir)
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
        [StringLength(100)]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [StringLength(20)]
        [Display(Name = "Telefon")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Teslimat adresi zorunludur.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Adres en az 10 karakter olmalıdır.")]
        [Display(Name = "Teslimat Adresi")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Sipariş Notu")]
        [StringLength(500)]
        public string? Note { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Toplam Tutar")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Sipariş Tarihi")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // "Hazırlanıyor", "Gönderildi", "Teslim Edildi", "İptal"
        [StringLength(20)]
        [Display(Name = "Durum")]
        public string Status { get; set; } = "Hazırlanıyor";

        // İlişki: siparişteki ürünler
        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

    // Sipariş içindeki tek bir ürün kalemi
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public virtual Order? Order { get; set; }

        public int ProductId { get; set; }

        [StringLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }
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
        public string HomeHeroTitle { get; set; } = string.Empty;

        [Required, StringLength(250)]
        [Display(Name = "Anasayfa Alt Başlık")]
        public string HomeHeroSubtitle { get; set; } = string.Empty;

        [Url(ErrorMessage = "Geçerli bir görsel URL'si giriniz.")]
        [StringLength(2048)]
        [Display(Name = "Anasayfa Kapak Görseli URL")]
        public string? HomeHeroImageUrl { get; set; }

        [StringLength(150)]
        [Display(Name = "Öne Çıkan Ürünler Başlığı")]
        public string? FeaturedTitle { get; set; }

        [StringLength(150)]
        [Display(Name = "Neden Biz Başlığı")]
        public string? WhyTitle { get; set; }

        [StringLength(100)]
        [Display(Name = "Avantaj 1 Başlığı")]
        public string? Benefit1Title { get; set; }

        [StringLength(300)]
        [Display(Name = "Avantaj 1 Açıklaması")]
        public string? Benefit1Description { get; set; }

        [StringLength(100)]
        [Display(Name = "Avantaj 2 Başlığı")]
        public string? Benefit2Title { get; set; }

        [StringLength(300)]
        [Display(Name = "Avantaj 2 Açıklaması")]
        public string? Benefit2Description { get; set; }

        [StringLength(100)]
        [Display(Name = "Avantaj 3 Başlığı")]
        public string? Benefit3Title { get; set; }

        [StringLength(300)]
        [Display(Name = "Avantaj 3 Açıklaması")]
        public string? Benefit3Description { get; set; }

        [StringLength(100)]
        [Display(Name = "Avantaj 4 Başlığı")]
        public string? Benefit4Title { get; set; }

        [StringLength(300)]
        [Display(Name = "Avantaj 4 Açıklaması")]
        public string? Benefit4Description { get; set; }

        [Url(ErrorMessage = "Geçerli bir görsel URL'si giriniz.")]
        [StringLength(2048)]
        [Display(Name = "Hakkımızda Görseli URL")]
        public string? AboutImageUrl { get; set; }

        [StringLength(150)]
        [Display(Name = "İletişim Sayfası Başlığı")]
        public string? ContactTitle { get; set; }

        // --- Hakkımızda ---
        [Required, StringLength(150)]
        [Display(Name = "Hakkımızda Başlık")]
        public string AboutTitle { get; set; } = string.Empty;

        [Required, StringLength(1000)]
        [Display(Name = "Hakkımızda Ana Metin")]
        public string AboutMainText { get; set; } = string.Empty;

        [Required, StringLength(1500)]
        [Display(Name = "Hakkımızda Detay Metin")]
        public string AboutDetailText { get; set; } = string.Empty;

        [Required, StringLength(20)]
        [Display(Name = "Yıllık Tecrübe")]
        public string StatYears { get; set; } = string.Empty;

        [Required, StringLength(20)]
        [Display(Name = "Mutlu Müşteri Sayısı")]
        public string StatCustomers { get; set; } = string.Empty;

        [Required, StringLength(20)]
        [Display(Name = "Çiçek Çeşidi")]
        public string StatProducts { get; set; } = string.Empty;

        // --- İletişim ---
        [Required, StringLength(150)]
        [Display(Name = "Adres")]
        public string ContactAddress { get; set; } = string.Empty;

        [Required, Phone, StringLength(30)]
        [Display(Name = "Telefon")]
        public string ContactPhone { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        [Display(Name = "E-posta")]
        public string ContactEmail { get; set; } = string.Empty;

        [Required, StringLength(150)]
        [Display(Name = "Çalışma Saatleri")]
        public string ContactWorkingHours { get; set; } = string.Empty;
    }

    // Yönetim paneli kullanıcısı — ASP.NET Core Identity tabanlı
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ad Soyad 2-100 karakter olmalıdır.")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = "Yönetici";
    }

    // Admin giriş formu için ViewModel
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }

    // İlk kurulum: yönetici hesabı oluşturma formu (hiç kullanıcı yokken çalışır)
    public class SetupViewModel
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ad Soyad 2-100 karakter olmalıdır.")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Şifre en az 8 karakter olmalıdır.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Şifre Tekrar")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    // --- Yönetim panelinde Anasayfa/Hakkımızda/İletişim içeriklerini
    // --- ayrı ayrı düzenlemek için kullanılan bölüm bazlı ViewModel'ler ---

    // Anasayfa içeriği düzenleme ViewModel'i
    public class HomeContentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(150)]
        [Display(Name = "Anasayfa Başlık")]
        public string HomeHeroTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Alt başlık zorunludur.")]
        [StringLength(250)]
        [Display(Name = "Anasayfa Alt Başlık")]
        public string HomeHeroSubtitle { get; set; } = string.Empty;
        [Url(ErrorMessage = "Geçerli bir görsel URL'si giriniz.")]
        [StringLength(2048)]
        [Display(Name = "Kapak Görseli URL")]
        public string? HomeHeroImageUrl { get; set; }

        [StringLength(150)]
        [Display(Name = "Öne Çıkan Ürünler Başlığı")]
        public string? FeaturedTitle { get; set; }

        [StringLength(150)]
        [Display(Name = "Neden Biz Başlığı")]
        public string? WhyTitle { get; set; }

        [StringLength(100)]
        [Display(Name = "Avantaj 1 Başlığı")]
        public string? Benefit1Title { get; set; }

        [StringLength(300)]
        [Display(Name = "Avantaj 1 Açıklaması")]
        public string? Benefit1Description { get; set; }

        [StringLength(100)]
        [Display(Name = "Avantaj 2 Başlığı")]
        public string? Benefit2Title { get; set; }

        [StringLength(300)]
        [Display(Name = "Avantaj 2 Açıklaması")]
        public string? Benefit2Description { get; set; }

        [StringLength(100)]
        [Display(Name = "Avantaj 3 Başlığı")]
        public string? Benefit3Title { get; set; }

        [StringLength(300)]
        [Display(Name = "Avantaj 3 Açıklaması")]
        public string? Benefit3Description { get; set; }

        [StringLength(100)]
        [Display(Name = "Avantaj 4 Başlığı")]
        public string? Benefit4Title { get; set; }

        [StringLength(300)]
        [Display(Name = "Avantaj 4 Açıklaması")]
        public string? Benefit4Description { get; set; }

    }

    // Hakkımızda içeriği düzenleme ViewModel'i
    public class AboutContentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(150)]
        [Display(Name = "Hakkımızda Başlık")]
        public string AboutTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ana metin zorunludur.")]
        [StringLength(1000)]
        [Display(Name = "Hakkımızda Ana Metin")]
        public string AboutMainText { get; set; } = string.Empty;

        [Required(ErrorMessage = "Detay metin zorunludur.")]
        [StringLength(1500)]
        [Display(Name = "Hakkımızda Detay Metin")]
        public string AboutDetailText { get; set; } = string.Empty;

        [Required, StringLength(20)]
        [Display(Name = "Yıllık Tecrübe")]
        public string StatYears { get; set; } = string.Empty;

        [Required, StringLength(20)]
        [Display(Name = "Mutlu Müşteri Sayısı")]
        public string StatCustomers { get; set; } = string.Empty;

        [Required, StringLength(20)]
        [Display(Name = "Çiçek Çeşidi")]
        public string StatProducts { get; set; } = string.Empty;
        [Url(ErrorMessage = "Geçerli bir görsel URL'si giriniz.")]
        [StringLength(2048)]
        [Display(Name = "Hakkımızda Görseli URL")]
        public string? AboutImageUrl { get; set; }

    }

    // İletişim içeriği düzenleme ViewModel'i
    public class ContactContentViewModel
    {
        public int Id { get; set; }

        [StringLength(150)]
        [Display(Name = "İletişim Sayfası Başlığı")]
        public string? ContactTitle { get; set; }

        [Required(ErrorMessage = "Adres zorunludur.")]
        [StringLength(150)]
        [Display(Name = "Adres")]
        public string ContactAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [StringLength(30)]
        [Display(Name = "Telefon")]
        public string ContactPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [StringLength(100)]
        [Display(Name = "E-posta")]
        public string ContactEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Çalışma saatleri zorunludur.")]
        [StringLength(150)]
        [Display(Name = "Çalışma Saatleri")]
        public string ContactWorkingHours { get; set; } = string.Empty;
    }
}
