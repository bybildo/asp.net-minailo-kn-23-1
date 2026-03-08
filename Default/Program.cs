using Default.Application.Services;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

Batteries.Init();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Default.Infrastructure.AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<JwtService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<HallService>();
builder.Services.AddScoped<TableService>();
builder.Services.AddScoped<ReservationService>();

var app = builder.Build();

app.UseCustomExceptionHandler();

app.UseRouting();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();