using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSupply.Helpers;

namespace SmartSupply.API.Controllers
{
    [Authorize(Roles = Roles.ResponsableLogistique + "," + Roles.Responsable)]
    public class CommanderController : Controller
    {
        public IActionResult Index() => View();
    }
}