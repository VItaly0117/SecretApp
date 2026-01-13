using Microsoft.EntityFrameworkCore;
using SecretApp.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite("Data Source=secrets.db"));
builder.Services.AddHostedService<CleanupService>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();

app.UseStaticFiles();
app.MapControllerRoute(name: "default", pattern: "{controller=Messages}/{action=Index}/{id?}");
app.Run();