using Cicekci.Data;
using Cicekci.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// SQLite veri tabanı bağlantısı
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=cicekci.db";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Sepet için session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// CartService ve HttpContextAccessor (sepet için)
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CartService>();

// Yönetim paneli kimlik doğrulama (Cookie tabanlı)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Account/Login";
        options.LogoutPath = "/Admin/Account/Logout";
        options.AccessDeniedPath = "/Admin/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.Cookie.Name = "CicekciAdminAuth";
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Veri tabanı başlatma
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Önce DB yoksa oluştur
    db.Database.EnsureCreated();

    // Eski şemadan gelen DB'lerde Orders tablosu olmayabilir
    // Kontrol et, yoksa DB'yi sil ve yeniden oluştur
    bool needsRecreate = false;
    try
    {
        db.Database.ExecuteSqlRaw("SELECT 1 FROM Orders LIMIT 1");
    }
    catch
    {
        needsRecreate = true;
    }

    if (needsRecreate)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }

    // Örnek veri ekle
    DbSeeder.Seed(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Yönetim paneli (Admin Area) rotası
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
