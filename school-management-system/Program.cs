using Microsoft.EntityFrameworkCore;
using SchoolProject.Data;
// 1. ДОБАВИЛИ: Пространство имен для работы с куки
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Transient);

// 2. ДОБАВИЛИ: Включаем и настраиваем службу аутентификации куки
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Куда сервер отправит пользователя, если он не вошел в систему
        options.AccessDeniedPath = "/Account/AccessDenied"; // Куда отправить, если у роли нет прав (например, студент лезет к админу)
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// 3. ДОБАВИЛИ И НАСТРОИЛИ ПОРЯДОК:
// Сначала проверяем КТО пользователь (Аутентификация)
app.UseAuthentication();
// Затем проверяем ЧТО ему разрешено делать (Авторизация)
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

// 4. ДОБАВИЛИ: Чтобы работали обычные контроллеры (ваш LoginController)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
