using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSupply.Helpers;

namespace SmartSupply.API.Controllers
{
    [Authorize(Roles = Roles.GestionnaireStock + "," + Roles.Magasinier)]
    public class StockerController : Controller
    {
        public IActionResult Index() => View();
    }
}
