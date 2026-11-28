using BlogMVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC servislerini ekle
builder.Services.AddControllersWithViews();

// SQL Server bağlantı bilgisini al
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Veritabanı bağlantı bilgisi bulunamadı."
    );

// DbContext'i sisteme tanıt
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// ASP.NET Core Identity yapılandırması
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Kullanıcı ayarları
    options.User.RequireUniqueEmail = true;

    // Parola güvenliği
    options.Password.RequiredLength = 12;

    // Hesap kilitleme ayarları
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan =
        TimeSpan.FromMinutes(15);
    
    // E-posta doğrulaması gerektirmiyoruz
    // Admin hesabını kendimiz oluşturacağız
    options.SignIn.RequireConfirmedAccount = false;

})
.AddEntityFrameworkStores<BlogDbContext>()
.AddDefaultTokenProviders();

// Giriş çerezi ayarları
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "BlogMVC.Identity";
    options.Cookie.HttpOnly = true;

    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";

    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

// Yetkilendirme servisleri
builder.Services.AddAuthorization();

var app = builder.Build();

// GELİŞTİRME ORTAMINDA ADMIN HESABI OLUŞTUR
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    await IdentitySeeder.SeedAdminAsync(
        scope.ServiceProvider,
        app.Configuration
    );
}

// Hata yönetimi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Kullanıcı kimliğini doğrula
app.UseAuthentication();

// Kullanıcının yetkilerini kontrol et
app.UseAuthorization();

// Statik dosyalar
app.MapStaticAssets();

// ADMIN ALANI YÖNLENDİRMESİ
app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}"
);

// NORMAL MVC YÖNLENDİRMESİ
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();