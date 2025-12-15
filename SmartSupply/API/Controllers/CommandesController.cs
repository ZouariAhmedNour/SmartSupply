using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartSupply.Application.Commands.Commandes;
using SmartSupply.Application.Queries.Commandes;
using SmartSupply.Domain.Models;

namespace SmartSupply.API.Controllers
{
    public class CommandesController : Controller
    {
        private readonly IMediator _mediator;

        public CommandesController(IMediator mediator) => _mediator = mediator;

        // GET: Commandes
        public async Task<IActionResult> Index()
        {
            var commandes = await _mediator.Send(new GetCommandesQuery());

            return View(commandes); // commandes = List<Commande>
        }

        // GET: Commandes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var commande = await _mediator.Send(new GetCommandeByIdQuery(id.Value));
            if (commande is null) return NotFound();

            return View(commande);
        }

        // GET: Commandes/Create
        public IActionResult Create() => View();

        // POST: Commandes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Commande commande)
        {
            if (!ModelState.IsValid)
                return View(commande);

            await _mediator.Send(new CreateCommandeCommand(
                commande.ClientNom,
                commande.ClientEmail,
                commande.MontantTotal,
                new List<LigneCommande>()
            ));

            return RedirectToAction(nameof(Index));
        }

        // GET: Commandes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();

            var commande = await _mediator.Send(new GetCommandeByIdQuery(id.Value));
            if (commande is null) return NotFound();

            return View(commande);
        }

        // POST: Commandes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string ClientNom, string ClientEmail, string Statut)
        {
            if (string.IsNullOrWhiteSpace(ClientNom) || string.IsNullOrWhiteSpace(ClientEmail))
            {
                ModelState.AddModelError("", "Nom et email requis.");
                var commande = await _mediator.Send(new GetCommandeByIdQuery(id));
                return View(commande);
            }

            await _mediator.Send(new UpdateCommandeCommand(id, ClientNom, ClientEmail, Statut));
            return RedirectToAction(nameof(Index));
        }

        // GET: Commandes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var commande = await _mediator.Send(new GetCommandeByIdQuery(id.Value));
            if (commande is null) return NotFound();

            return View(commande);
        }

        // POST: Commandes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteCommandeCommand(id));
            return RedirectToAction(nameof(Index));
        }

        // POST: Commandes/ChangeStatut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatut(int id, string statut)
        {
            if (string.IsNullOrWhiteSpace(statut))
            {
                ModelState.AddModelError("", "Statut requis.");
                var commande = await _mediator.Send(new GetCommandeByIdQuery(id));
                return View("Edit", commande);
            }

            await _mediator.Send(new UpdateCommandeStatutCommand(id, statut));
            return RedirectToAction(nameof(Index));
        }
    }
}
