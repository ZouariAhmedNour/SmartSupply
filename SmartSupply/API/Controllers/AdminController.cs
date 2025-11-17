using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartSupply.API.Controllers
{
    [Authorize(Roles = "Administrateur")]
    public class AdminController : Controller
    {
        public IActionResult Index() => View();
    }
}
