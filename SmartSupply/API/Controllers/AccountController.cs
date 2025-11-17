using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;
using System.Security.Claims;

namespace SmartSupply1.Controllers
{
    public class AccountController : Controller
    {
        private readonly SmartSupplyDbContext _db;
        private readonly PasswordHasher<Utilisateur> _passwordHasher;
        public AccountController(SmartSupplyDbContext db)
        {
            _db = db;
            _passwordHasher = new PasswordHasher<Utilisateur>();
        }
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            // Si déjà connecté, on redirige immédiatement selon le rôle
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToRoleHomeFromClaims();
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View("~/Views/Account/Login.cshtml");
        }
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe = false, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Email"] = email;
            ViewData["RememberMe"] = rememberMe;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email et mot de passe sont requis.");
                return View("~/Views/Account/Login.cshtml");
            }

            var user = _db.Utilisateurs.SingleOrDefault(u => u.Email == email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email ou mot de passe invalide.");
                return View("~/Views/Account/Login.cshtml");
            }
            var verify = _passwordHasher.VerifyHashedPassword(user, user.MdpHashed, password);
            if (verify == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Email ou mot de passe invalide.");
                return View("~/Views/Account/Login.cshtml");
            }
            // Création des claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Prenom} {user.Nom}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? string.Empty)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var authProps = new AuthenticationProperties { IsPersistent = rememberMe };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);
            // Redirection : priorité au returnUrl si local, sinon rediriger selon rôle
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToRoleHomeFromUser(user);
        }
        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        // ---------- Helpers ----------
        // Redirige selon le rôle fourni (utiliser après authentification)
        private IActionResult RedirectToRoleHomeFromUser(Utilisateur user)
        {
            var role = user?.Role ?? string.Empty;
            if (role == "Administrateur") return RedirectToAction("Index", "Admin");
            if (role == "Magasinier" || role == "Gestionnaire de stock") return RedirectToAction("Index", "Stock");
            if (role == "ResponsableLogistique" || role == "Responsable") return RedirectToAction("Index", "Commandes");
            // défaut
            return RedirectToAction("Index", "Home");
        }
        // Redirige selon le rôle présent dans les claims (utilisé si l'utilisateur est déjà authentifié)
        private IActionResult RedirectToRoleHomeFromClaims()
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;
            if (role == "Administrateur") return RedirectToAction("Index", "Admin");
            if (role == "Magasinier" || role == "Gestionnaire de stock") return RedirectToAction("Index", "Stocker");
            if (role == "ResponsableLogistique" || role == "Responsable") return RedirectToAction("Index", "Commander");
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            // Si déjà connecté, redirige selon rôle
            if (User?.Identity?.IsAuthenticated == true)
                return RedirectToRoleHomeFromClaims();
            ViewBag.AvailableRoles = new List<string> { "Magasinier", "ResponsableLogistique", "Responsable" }; // Exclure Administrateur pour sécurité
            return View("~/Views/Account/SignUp.cshtml");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(string nom, string prenom, string email, string password, string confirmPassword, string role)
        {
            ViewData["Nom"] = nom;
            ViewData["Prenom"] = prenom;
            ViewData["Email"] = email;
            ViewData["Role"] = role;

            // Validation manuelle (remplace ModelState pour champs requis)
            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                ModelState.AddModelError("", "Tous les champs sont requis.");
                ViewBag.AvailableRoles = new List<string> { "Magasinier", "ResponsableLogistique", "Responsable" };
                return View("~/Views/Account/SignUp.cshtml");
            }
            if (password != confirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Les mots de passe ne correspondent pas.");
                ViewBag.AvailableRoles = new List<string> { "Magasinier", "ResponsableLogistique", "Responsable" };
                return View("~/Views/Account/SignUp.cshtml");
            }
            if (password.Length < 6)
            {
                ModelState.AddModelError("Password", "Le mot de passe doit faire au moins 6 caractères.");
                ViewBag.AvailableRoles = new List<string> { "Magasinier", "ResponsableLogistique", "Responsable" };
                return View("~/Views/Account/SignUp.cshtml");
            }
            if (_db.Utilisateurs.Any(u => u.Email == email))
            {
                ModelState.AddModelError("Email", "Cet email est déjà utilisé.");
                ViewBag.AvailableRoles = new List<string> { "Magasinier", "ResponsableLogistique", "Responsable" };
                return View("~/Views/Account/SignUp.cshtml");
            }
            // Security note: ne pas permettre création d'admins publics (meilleur pratique)
            // Ici on accepte le rôle venant du formulaire, mais je recommande de forcer un rôle par défaut.
            var roleToAssign = role;
            if (string.Equals(roleToAssign, "Administrateur", StringComparison.OrdinalIgnoreCase))
            {
                // empêcher la création publique d'admins — forcer au rôle par défaut
                roleToAssign = "Magasinier";
            }
            // Création de l'utilisateur
            var user = new Utilisateur
            {
                Nom = nom,
                Prenom = prenom,
                Email = email,
                Role = roleToAssign,
                DateCreation = DateTime.UtcNow
            };
            // Hash du mot de passe
            user.MdpHashed = _passwordHasher.HashPassword(user, password);
            _db.Utilisateurs.Add(user);
            _db.SaveChanges();
            // Auto-login après inscription : création des claims et sign in
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Prenom} {user.Nom}"),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role ?? string.Empty)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            // Redirection selon rôle
            return RedirectToRoleHomeFromUser(user);
        }
    }
}