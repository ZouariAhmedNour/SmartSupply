// SmartSupply1.Controllers.LigneCommandesController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.LigneCommandeCommand;
using SmartSupply.Application.Commands.LigneCommandes;
using SmartSupply.Application.Queries.LigneCommandes;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply1.Controllers
{
    public class LigneCommandesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly SmartSupplyDbContext _context;

        public LigneCommandesController(IMediator mediator, SmartSupplyDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        #region Index
        // GET: LigneCommandes
        public async Task<IActionResult> Index()
        {
            var list = await _mediator.Send(new GetLigneCommandesQuery());
            return View(list);
        }
        #endregion

        #region Details
        // GET: LigneCommandes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ligne = await _mediator.Send(new GetLigneCommandeByIdQuery(id.Value));
            if (ligne == null) return NotFound();

            return View(ligne);
        }
        #endregion

        #region Create
        // GET: LigneCommandes/Create
        public IActionResult Create()
        {
            FillSelectLists();
            return View();
        }

        // POST: LigneCommandes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CommandeId,ProduitId,Quantite,PrixUnitaire")] LigneCommande ligneCommande)
        {
            if (!ModelState.IsValid)
            {
                FillSelectLists();
                return View(ligneCommande);
            }

            var created = await _mediator.Send(new CreateLigneCommandeCommand(
                ligneCommande.CommandeId,
                ligneCommande.ProduitId,
                ligneCommande.Quantite,
                ligneCommande.PrixUnitaire
            ));

            if (!created)
            {
                ModelState.AddModelError("", "Impossible de créer la ligne (références invalides ou données incorrectes).");
                FillSelectLists();
                return View(ligneCommande);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        // GET: LigneCommandes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ligne = await _mediator.Send(new GetLigneCommandeByIdQuery(id.Value));
            if (ligne == null) return NotFound();

            FillSelectLists();
            return View(ligne);
        }

        // POST: LigneCommandes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CommandeId,ProduitId,Quantite,PrixUnitaire")] LigneCommande ligneCommande)
        {
            if (id != ligneCommande.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                FillSelectLists();
                return View(ligneCommande);
            }

            var updated = await _mediator.Send(new UpdateLigneCommandeCommand(
                ligneCommande.Id,
                ligneCommande.CommandeId,
                ligneCommande.ProduitId,
                ligneCommande.Quantite,
                ligneCommande.PrixUnitaire
            ));

            if (updated == null)
            {
                ModelState.AddModelError("", "Impossible de mettre à jour (ligne introuvable ou données invalides).");
                FillSelectLists();
                return View(ligneCommande);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        // GET: LigneCommandes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ligne = await _mediator.Send(new GetLigneCommandeByIdQuery(id.Value));
            if (ligne == null) return NotFound();

            return View(ligne);
        }

        // POST: LigneCommandes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _mediator.Send(new DeleteLigneCommandeCommand(id));
            if (!deleted)
            {
                ModelState.AddModelError("", "Impossible de supprimer la ligne (introuvable ou erreur).");
                var l = await _mediator.Send(new GetLigneCommandeByIdQuery(id));
                return View(l);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Helpers
        private void FillSelectLists()
        {
            // Utilise le DbContext pour remplir les SelectList (ou remplace par des queries MediatR si tu préfères)
            var commandes = _context.Commandes.AsNoTracking().ToList();
            var produits = _context.Produits.AsNoTracking().ToList();

            ViewData["CommandeId"] = new SelectList(commandes, "Id", "Id");
            ViewData["ProduitId"] = new SelectList(produits, "Id", "Nom");
        }

        private bool LigneCommandeExists(int id)
        {
            return _context.LignesCommande.Any(e => e.Id == id);
        }
        #endregion
    }
}
