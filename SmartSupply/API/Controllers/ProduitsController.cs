// SmartSupply1.Controllers.ProduitsController.cs
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SmartSupply.Domain.Models;
using SmartSupply.Application.Commands.Produits;
using SmartSupply.Application.Queries.Produits;

namespace SmartSupply1.Controllers
{
    public class ProduitsController : Controller
    {
        private readonly IMediator _mediator;

        public ProduitsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: Produits
        public async Task<IActionResult> Index()
        {
            var list = await _mediator.Send(new GetProduitsQuery());
            return View(list);
        }

        // GET: Produits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var produit = await _mediator.Send(new GetProduitByIdQuery(id.Value));
            if (produit == null) return NotFound();

            return View(produit);
        }

        // GET: Produits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Description,CodeSKU,PrixUnitaire,DateCreation")] Produit produit)
        {
            if (!ModelState.IsValid)
                return View(produit);

            var created = await _mediator.Send(new CreateProduitCommand(produit.Nom, produit.Description, produit.CodeSKU, produit.PrixUnitaire));
            if (!created)
            {
                ModelState.AddModelError("", "Impossible de créer le produit (CodeSKU peut-être déjà utilisé ou données invalides).");
                return View(produit);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Produits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var produit = await _mediator.Send(new GetProduitByIdQuery(id.Value));
            if (produit == null) return NotFound();

            return View(produit);
        }

        // POST: Produits/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Description,CodeSKU,PrixUnitaire,DateCreation")] Produit produit)
        {
            if (id != produit.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(produit);

            var updated = await _mediator.Send(new UpdateProduitCommand(produit.Id, produit.Nom, produit.Description, produit.CodeSKU, produit.PrixUnitaire));
            if (updated == null)
            {
                ModelState.AddModelError("", "Impossible de mettre à jour (produit introuvable ou CodeSKU déjà utilisé).");
                return View(produit);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Produits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var produit = await _mediator.Send(new GetProduitByIdQuery(id.Value));
            if (produit == null) return NotFound();

            return View(produit);
        }

        // POST: Produits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _mediator.Send(new DeleteProduitCommand(id));
            if (!deleted)
            {
                ModelState.AddModelError("", "Impossible de supprimer le produit (introuvable ou erreur).");
                var p = await _mediator.Send(new GetProduitByIdQuery(id));
                return View(p);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
