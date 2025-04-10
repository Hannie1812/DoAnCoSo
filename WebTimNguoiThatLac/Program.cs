using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebTimNguoiThatLac.Data;
using WebTimNguoiThatLac.Models;
using WebTimNguoiThatLac.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    })
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    });

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<WordExportService>();// Xuất File Đăng Kí

builder.Services.AddTransient<EmailService>(); // email


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddDefaultTokenProviders()
.AddDefaultUI()
.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.LogoutPath = $"/Identity/Account/AccessDenied";

});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSession();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //n�y 

app.UseAuthorization();

app.MapRazorPages(); // n�y

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
