using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSupply.Application.Commands.Utilisateurs;
using SmartSupply.Domain.Models;
using SmartSupply.Helpers;
using System.Security.Claims;

namespace SmartSupply1.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            if (User?.Identity?.IsAuthenticated == true)
                return RedirectToRoleHomeFromClaims();

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

            var user = await _mediator.Send(new LoginUtilisateurCommand(email, password));

            if (user == null)
            {
                ModelState.AddModelError("", "Email ou mot de passe incorrect.");
                return View("~/Views/Account/Login.cshtml");
            }

            // Création des claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Prenom} {user.Nom}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties { IsPersistent = rememberMe });

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

        // GET: /Account/SignUp
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            if (User?.Identity?.IsAuthenticated == true)
                return RedirectToRoleHomeFromClaims();

            ViewBag.AvailableRoles = new List<string> { "Magasinier", "ResponsableLogistique", "Responsable" };
            return View("~/Views/Account/SignUp.cshtml");
        }

        // POST: /Account/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(string nom, string prenom, string email, string password, string confirmPassword, string role)
        {
            ViewData["Nom"] = nom;
            ViewData["Prenom"] = prenom;
            ViewData["Email"] = email;
            ViewData["Role"] = role;

            if (password != confirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Les mots de passe ne correspondent pas.");
                ViewBag.AvailableRoles = new List<string> { "Magasinier", "ResponsableLogistique", "Responsable" };
                return View("~/Views/Account/SignUp.cshtml");
            }

            var user = await _mediator.Send(new RegisterUtilisateurCommand(nom, prenom, email, password, confirmPassword, role));

            if (user == null)
            {
                ModelState.AddModelError("", "Impossible de créer l'utilisateur. Email peut-être déjà utilisé.");
                ViewBag.AvailableRoles = new List<string> { "Magasinier", "ResponsableLogistique", "Responsable" };
                return View("~/Views/Account/SignUp.cshtml");
            }

            // Auto-login
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Prenom} {user.Nom}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToRoleHomeFromUser(user);
        }

        // Helpers : redirections selon rôle
        private IActionResult RedirectToRoleHomeFromUser(Utilisateur user)
        {
            var role = user?.Role ?? string.Empty;

            if (role == Roles.Admin)
                return RedirectToAction("Index", "Admin");

            if (role == Roles.Magasinier || role == Roles.GestionnaireStock)
                return RedirectToAction("Index", "Stocker");

            if (role == Roles.ResponsableLogistique || role == Roles.Responsable)
                return RedirectToAction("Index", "Commander");

            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToRoleHomeFromClaims()
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";

            if (role == Roles.Admin)
                return RedirectToAction("Index", "Admin");

            if (role == Roles.Magasinier || role == Roles.GestionnaireStock)
                return RedirectToAction("Index", "Stocker");

            if (role == Roles.ResponsableLogistique || role == Roles.Responsable)
                return RedirectToAction("Index", "Commander");

            return RedirectToAction("Index", "Home");
        }
    }
}
