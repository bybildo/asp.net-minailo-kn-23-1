using Microsoft.AspNetCore.Identity;
using Restaurant.Application.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Interfaces;
using Restaurant.Application.Services;
using Restaurant.Infrastructure;
using Restaurant.Infrastructure.Repositories;
using Restaurant.Infrastructure.Security;
using Restaurant.Presentation.Filters;
using Restaurant.Presentation.Middlewares;
using SQLitePCL;

Batteries.Init();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<GlobalRequestLogFilter>();
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
});

builder.Services.AddScoped<IUserRepository, IdentityUserRepository>();
builder.Services.AddScoped<IHallRepository, HallRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHallService, HallService>();
builder.Services.AddScoped<ITableService, TableService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ServiceLevelLogFilter>();
builder.Services.AddScoped<IAppEmailSender, AppEmailSender>();

builder.Services.AddAuthorization();
builder.Services.AddRazorPages();

var app = builder.Build();

await IdentitySeed.SeedAdminAsync(app.Services);

app.UseCustomExceptionHandler();

// Виконується на кожен HTTP-запит
// працює переж і після наступного компонента pipeline
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("PipelineUseMiddleware");
    logger.LogInformation("UseMiddleware before {Path}", context.Request.Path);
    await next();
    logger.LogInformation("UseMiddleware after {Path}", context.Request.Path);
});

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// локальний pipeline, незалежний від основного для певного path
app.Map("/lab4-map", mapApp =>
{
    mapApp.Use(async (context, next) =>
    {
        var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("MapBranchMiddleware");
        logger.LogInformation("Map branch middleware before");
        await next();
        logger.LogInformation("Map branch middleware after");
    });

    mapApp.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("MapBranchRun");
        logger.LogInformation("Map branch terminal RUN");
        await context.Response.WriteAsync("Lab4 map branch executed.");
    });
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Map("/lab4-run", runApp =>
{
    //Останній обробник у pipeline, який завершує запит, якщо до нього дійшли
    runApp.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("TerminalRunMiddleware");
        logger.LogInformation("Terminal RUN branch for path: {Path}", context.Request.Path);
        await context.Response.WriteAsync("Lab4 terminal run branch executed.");
    });
});

app.Run();
