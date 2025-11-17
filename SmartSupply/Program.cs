using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MediatR;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
       .AddRazorRuntimeCompilation();

// DB context (vérifie la bonne chaîne dans appsettings.json)
builder.Services.AddDbContext<SmartSupplyDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("smartSupply")
                     ?? builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Assemblée Application ---
// OPTION A (recommandée lorsque SmartSupply.Application est référencé par le projet API)
// utilisez typeof d'un type de l'assembly Application quand il existe :
Assembly appAssembly = null;
try
{
    appAssembly = Assembly.Load("SmartSupply.Application"); // ne marche que si le projet est compilé et référencé
}
catch
{
    // fallback : si type connu (après avoir créé le projet Application) utilisez :
    // appAssembly = typeof(SmartSupply.Application.SomeType).Assembly;
}
// Si vous avez déjà des types définis dans Application, utilisez plutôt typeof(MyType).Assembly
// Exemple (décommentez après création du projet et ajout d'un type) :
// appAssembly = typeof(SmartSupply.Application.Products.Commands.CreateProductCommand).Assembly;
if (appAssembly != null)
{
    // MediatR
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(appAssembly));
    // AutoMapper
    builder.Services.AddAutoMapper(appAssembly);
    // FluentValidation : enregistre tous les validators trouvés dans l'assembly Application
    builder.Services.AddValidatorsFromAssembly(appAssembly);
}
else
{
    // Développement local : vous pouvez enregistrer manuellement les handlers/mappers/validators
    // ou recharger après avoir ajouté le projet Application
    // Exemple : builder.Services.AddMediatR(typeof(Program).Assembly);
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(Program).Assembly);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = "SmartSupply.Auth";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });
builder.Services.AddAuthorization(options =>
{
});

var app = builder.Build();

// Seed initial admin user (simple) et Apply migrations on startup
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

// Configure the HTTP request pipeline.
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
    pattern: "{controller=Account}/{action=Login}/{id?}");
app.Run();