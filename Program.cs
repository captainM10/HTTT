using AspNetCoreHero.ToastNotification.Notyf.Models;
using AspNetCoreHero.ToastNotification;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using CanhGac.Models;
using CanhGac.Respository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("CanhGacContext");
builder.Services.AddDbContext<CanhGacContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddNotyf(config => { config.DurationInSeconds = 3; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
builder.Services.AddDbContext<CanhGacContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddScoped<IDaiDoiRespository, DaiDoiRespository>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Đặt thời gian tối đa để Session tồn tại
    options.Cookie.Name = ".MyPhamCheilinus.Session"; // Tên của Cookie Session
    options.Cookie.IsEssential = true; // Đảm bảo rằng Cookie này cần thiết
});






// Thêm dịch vụ Identity
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(p =>
    {
        p.LoginPath = "/dang-nhap.html";
        p.AccessDeniedPath = "/";
    });



builder.Services.AddDistributedMemoryCache();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.MapControllerRoute(
    name: "sanphamtheodanhmuc",
    pattern: "sanpham/sanphamtheodanhmuc/{maDanhMuc}",
    defaults: new { controller = "SanPham", action = "SanPhamTheoDanhMuc" });

app.MapControllerRoute(
    name: "filter",
    pattern: "home/filter",
    defaults: new { controller = "Home", action = "Filter" });

app.Run();
