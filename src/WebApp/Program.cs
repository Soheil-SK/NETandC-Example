using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration des services MVC
builder.Services.AddControllersWithViews();

// Configuration de la session (pour stocker le token JWT)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configuration de l'authentification par cookies
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization();

// Configuration de HttpClient pour consommer la Core API
builder.Services.AddHttpClient<ICoreApiService, CoreApiService>(client =>
{
    var apiBaseUrl = builder.Configuration["CoreApi:BaseUrl"] ?? "https://localhost:7001";
    client.BaseAddress = new Uri(apiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Enregistrement des services
builder.Services.AddScoped<ICoreApiService, CoreApiService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configuration du pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
