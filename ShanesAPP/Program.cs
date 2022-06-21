using ShanesAPP.Infrastructure;
using ShanesAPP.Infrastructure.Interfaces;
using ShanesAPP.Services;
using ShanesAPP.Services.Interfaces;
using ILogger = ShanesAPP.Infrastructure.Interfaces.ILogger;

var builder = WebApplication.CreateBuilder(args);
// DI

builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>();
builder.Services.AddSingleton<IAppSettings, AppSettings>();
builder.Services.AddSingleton<IGoogleApiService, GoogleApiService>();
builder.Services.AddSingleton<ISessionService, SessionService>();
builder.Services.AddSingleton<ILogger, Logger>();

builder.Services.AddHttpClient<IAuthClient, AuthClient>();
builder.Services.AddHttpClient<IGoogleApiClient, GoogleApiClient>();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.UseEndpoints(endpoints => endpoints.MapControllerRoute("default", "{action=Index}/{id?}", new { controller = "Home" }));

app.Run();
