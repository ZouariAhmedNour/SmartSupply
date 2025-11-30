// SmartSupply1.Controllers.HistoriqueStocksController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MediatR;
using SmartSupply.Domain.Models;
using SmartSupply.Application.Commands.HistoriqueStocks;
using SmartSupply.Application.Queries.HistoriqueStocks;
using SmartSupply.Application.Queries.Produits;
using SmartSupply.Application.Queries.Entrepots;

namespace SmartSupply1.Controllers
{
    public class HistoriqueStocksController : Controller
    {
        private readonly IMediator _mediator;

        public HistoriqueStocksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Index
        // GET: HistoriqueStocks
        public async Task<IActionResult> Index()
        {
            var list = await _mediator.Send(new GetHistoriqueStocksQuery());
            return View(list);
        }
        #endregion

        #region Details
        // GET: HistoriqueStocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var h = await _mediator.Send(new GetHistoriqueStockByIdQuery(id.Value));
            if (h == null) return NotFound();

            return View(h);
        }
        #endregion

        #region Create
        // GET: HistoriqueStocks/Create
        public async Task<IActionResult> Create()
        {
            await FillSelectLists();
            return View();
        }

        // POST: HistoriqueStocks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProduitId,EntrepotId,Quantite,DateMouvement,TypeMouvement,Commentaire")] HistoriqueStock historiqueStock)
        {
            if (!ModelState.IsValid)
            {
                await FillSelectLists();
                return View(historiqueStock);
            }

            var created = await _mediator.Send(new CreateHistoriqueStockCommand(
                historiqueStock.ProduitId,
                historiqueStock.EntrepotId,
                historiqueStock.Quantite,
                historiqueStock.TypeMouvement,
                historiqueStock.Commentaire
            ));

            if (!created)
            {
                ModelState.AddModelError("", "Impossible de créer l'historique (références invalides ou données incorrectes).");
                await FillSelectLists();
                return View(historiqueStock);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        // GET: HistoriqueStocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var h = await _mediator.Send(new GetHistoriqueStockByIdQuery(id.Value));
            if (h == null) return NotFound();

            await FillSelectLists();
            return View(h);
        }

        // POST: HistoriqueStocks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProduitId,EntrepotId,Quantite,DateMouvement,TypeMouvement,Commentaire")] HistoriqueStock historiqueStock)
        {
            if (id != historiqueStock.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                await FillSelectLists();
                return View(historiqueStock);
            }

            var updated = await _mediator.Send(new UpdateHistoriqueStockCommand(
                historiqueStock.Id,
                historiqueStock.ProduitId,
                historiqueStock.EntrepotId,
                historiqueStock.Quantite,
                historiqueStock.DateMouvement,
                historiqueStock.TypeMouvement,
                historiqueStock.Commentaire
            ));

            if (updated == null)
            {
                ModelState.AddModelError("", "Impossible de mettre à jour l'historique (introuvable ou données invalides).");
                await FillSelectLists();
                return View(historiqueStock);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        // GET: HistoriqueStocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var h = await _mediator.Send(new GetHistoriqueStockByIdQuery(id.Value));
            if (h == null) return NotFound();

            return View(h);
        }

        // POST: HistoriqueStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _mediator.Send(new DeleteHistoriqueStockCommand(id));
            if (!deleted)
            {
                ModelState.AddModelError("", "Impossible de supprimer l'historique (introuvable ou erreur).");
                var h = await _mediator.Send(new GetHistoriqueStockByIdQuery(id));
                return View(h);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Helpers
        private async Task FillSelectLists()
        {
            var produits = await _mediator.Send(new GetProduitsQuery());
            var entrepots = await _mediator.Send(new GetEntrepotsQuery());

            ViewData["ProduitId"] = new SelectList(produits, "Id", "Nom");
            ViewData["EntrepotId"] = new SelectList(entrepots, "Id", "Nom");
        }
        #endregion
    }
}
