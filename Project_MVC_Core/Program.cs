using Microsoft.EntityFrameworkCore;
using Project_MVC_Core.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FoodDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("db")));
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseStaticFiles();
app.MapDefaultControllerRoute();

app.Run();
