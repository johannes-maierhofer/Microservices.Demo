var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services
    .AddHealthChecksUI()
    .AddInMemoryStorage();

var app = builder.Build();

app.UseRouting();

app.UseAuthorization();

app.MapHealthChecksUI();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
