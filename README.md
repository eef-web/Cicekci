# Çiçekçi 🌸 — ASP.NET Core MVC Çiçekçi Sitesi

Visual Studio'da açabileceğin, SQLite veri tabanı destekli, tam çalışır bir çiçek e-ticaret sitesi.

## 📋 İçindekiler

- **Ana Sayfa** — Öne çıkan ürünler, kategoriler, hero banner
- **Ürünler** — Tüm ürünler, kategori filtreleme, arama, ürün detay sayfası
- **Sepet** — Sepete ekleme, adet güncelleme, ürün çıkarma, sipariş tamamlama (Session tabanlı)
- **Hakkımızda** — Şirket bilgileri, istatistikler, özellikler
- **İletişim** — Mesaj gönderme formu (veri tabanına kaydedilir)
- **Veri Tabanı** — SQLite (Entity Framework Core ile, otomatik oluşturulur)

## 🚀 Çalıştırma

### Visual Studio ile
1. `Cicekci.sln` dosyasını Visual Studio'da aç
2. `F5` tuşuna bas (veya "Başlat" butonuna tıkla)
3. Site `http://localhost:5000` adresinde açılır

### Komut Satırı ile
```bash
cd Cicekci
dotnet restore
dotnet run
```

## 🗄️ Veri Tabanı

- **SQLite** kullanır (`cicekci.db` dosyası otomatik oluşturulur)
- Tablolar: `Categories`, `Products`, `ContactMessages`
- İlk açılışta 4 kategori ve 12 örnek ürün otomatik eklenir
- EF Core ile migration yapılabilir:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
(Alternatif olarak `EnsureCreated()` ile otomatik oluşturma zaten aktif.)

## 📁 Proje Yapısı

```
Cicekci/
├── Controllers/
│   ├── HomeController.cs       (Ana Sayfa, Hakkımızda, İletişim)
│   ├── ProductsController.cs  (Ürün listesi, detay, sepete ekle)
│   └── CartController.cs      (Sepet, adet güncelle, sipariş tamamla)
├── Data/
│   └── ApplicationDbContext.cs  (EF Core DbContext + Veri tabanı seeder)
├── Models/
│   └── Models.cs              (Category, Product, CartItem, ContactMessage)
├── Services/
│   └── CartService.cs         (Session tabanlı sepet yönetimi)
├── Views/
│   ├── Home/                  (Index, About, Contact)
│   ├── Products/              (Index, Details)
│   ├── Cart/                  (Index)
│   └── Shared/                (_Layout, Error)
├── wwwroot/
│   └── css/site.css           (Tüm stiller)
├── Program.cs                 (Uygulama başlatma yapılandırması)
├── appsettings.json           (Ayarlar)
└── Cicekci.csproj             (Proje dosyası)
```

## 🛠️ Teknolojiler

- **ASP.NET Core 8** (MVC)
- **Entity Framework Core 8** (SQLite)
- **Bootstrap 5** (UI framework)
- **Bootstrap Icons** (İkonlar)

## 📝 Notlar

- Sepet verileri Session'da saklanır (30 dakika timeout)
- İletişim mesajları veri tabanında saklanır
- Görseller Unsplash CDN'den gelir (internet bağlantısı gerekir)
- Geliştirme modunda hata sayfası detaylıdır
