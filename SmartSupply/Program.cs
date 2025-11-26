using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add MVC + Razor runtime
builder.Services.AddControllersWithViews()
       .AddRazorRuntimeCompilation();

// DB context
builder.Services.AddDbContext<SmartSupplyDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("smartSupply")
                     ?? builder.Configuration.GetConnectionString("DefaultConnection")));

// ----------------------------------------------
// 1️⃣ Charger l'assembly SmartSupply.Application
// ----------------------------------------------
Assembly? appAssembly = null;
try
{
    appAssembly = Assembly.Load("SmartSupply.Application");
}
catch
{
    // pas grave → si pas encore créé
}

// ----------------------------------------------
// 2️⃣ MediatR
// ----------------------------------------------
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

    if (appAssembly != null)
        cfg.RegisterServicesFromAssembly(appAssembly);
});

// ----------------------------------------------
// 3️⃣ AutoMapper
// (UN SEUL appel ! scan complet, suffit)
// ----------------------------------------------
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ----------------------------------------------
// 4️⃣ FluentValidation
// ----------------------------------------------
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

if (appAssembly != null)
    builder.Services.AddValidatorsFromAssembly(appAssembly);

// ----------------------------------------------
// Swagger
// ----------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ----------------------------------------------
// Authentification Cookie
// ----------------------------------------------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = "SmartSupply.Auth";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ----------------------------------------------
// DB Migrations + Seed Admin
// ----------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SmartSupplyDbContext>();
    db.Database.Migrate();

    if (!db.Utilisateurs.Any(u => u.Email == "admin@smartsupply.local"))
    {
        var passwordHasher = new PasswordHasher<Utilisateur>();
        var admin = new Utilisateur
        {
            Nom = "Admin",
            Prenom = "System",
            Email = "admin@smartsupply.local",
            Role = "Administrateur",
            DateCreation = DateTime.UtcNow
        };
        admin.MdpHashed = passwordHasher.HashPassword(admin, "ahmed123");
        db.Utilisateurs.Add(admin);
        db.SaveChanges();
    }
}

// ----------------------------------------------
// Pipeline HTTP
// ----------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
);

app.Run();
