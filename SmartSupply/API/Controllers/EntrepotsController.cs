// SmartSupply1.Controllers.EntrepotsController.cs
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SmartSupply.Domain.Models;
using SmartSupply.Application.Commands.Entrepots;
using SmartSupply.Application.Queries.Entrepots;

namespace SmartSupply1.Controllers
{
    public class EntrepotsController : Controller
    {
        private readonly IMediator _mediator;

        public EntrepotsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: Entrepots
        public async Task<IActionResult> Index()
        {
            var list = await _mediator.Send(new GetEntrepotsQuery());
            return View(list);
        }

        // GET: Entrepots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var entrepot = await _mediator.Send(new GetEntrepotByIdQuery(id.Value));
            if (entrepot == null) return NotFound();

            return View(entrepot);
        }

        // GET: Entrepots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Entrepots/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Adresse,CapaciteMax,DateCreation")] Entrepot entrepot)
        {
            if (!ModelState.IsValid)
                return View(entrepot);

            var created = await _mediator.Send(new CreateEntrepotCommand(entrepot.Nom, entrepot.Adresse, entrepot.CapaciteMax));
            if (!created)
            {
                ModelState.AddModelError("", "Impossible de créer l'entrepôt (nom peut-être déjà utilisé ou données invalides).");
                return View(entrepot);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Entrepots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var entrepot = await _mediator.Send(new GetEntrepotByIdQuery(id.Value));
            if (entrepot == null) return NotFound();

            return View(entrepot);
        }

        // POST: Entrepots/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Adresse,CapaciteMax,DateCreation")] Entrepot entrepot)
        {
            if (id != entrepot.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(entrepot);

            var updated = await _mediator.Send(new UpdateEntrepotCommand(entrepot.Id, entrepot.Nom, entrepot.Adresse, entrepot.CapaciteMax));
            if (updated == null)
            {
                ModelState.AddModelError("", "Impossible de mettre à jour (entrepôt introuvable ou nom déjà utilisé).");
                return View(entrepot);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Entrepots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var entrepot = await _mediator.Send(new GetEntrepotByIdQuery(id.Value));
            if (entrepot == null) return NotFound();

            return View(entrepot);
        }

        // POST: Entrepots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _mediator.Send(new DeleteEntrepotCommand(id));
            if (!deleted)
            {
                ModelState.AddModelError("", "Impossible de supprimer l'entrepôt (introuvable ou erreur).");
                var ent = await _mediator.Send(new GetEntrepotByIdQuery(id));
                return View(ent);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
