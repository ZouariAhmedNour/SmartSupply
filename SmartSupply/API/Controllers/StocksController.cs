// SmartSupply1.Controllers.StocksController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MediatR;
using SmartSupply.Domain.Models;
using SmartSupply.Application.Commands.Stocks;
using SmartSupply.Application.Queries.Stocks;
using SmartSupply.Application.Queries.Entrepots;
using SmartSupply.Application.Queries.Produits;

namespace SmartSupply1.Controllers
{
    public class StocksController : Controller
    {
        private readonly IMediator _mediator;

        public StocksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Index
        // GET: Stocks
        public async Task<IActionResult> Index()
        {
            var list = await _mediator.Send(new GetStocksQuery());
            return View(list);
        }
        #endregion

        #region Details
        // GET: Stocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var stock = await _mediator.Send(new GetStockByIdQuery(id.Value));
            if (stock == null) return NotFound();

            return View(stock);
        }
        #endregion

        #region Create
        // GET: Stocks/Create
        public async Task<IActionResult> Create()
        {
            await FillSelectLists();
            return View();
        }

        // POST: Stocks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProduitId,EntrepotId,QuantiteDisponible,SeuilAlerte")] Stock stock)
        {
            if (!ModelState.IsValid)
            {
                await FillSelectLists();
                return View(stock);
            }

            var created = await _mediator.Send(new CreateStockCommand(stock.ProduitId, stock.EntrepotId, stock.QuantiteDisponible, stock.SeuilAlerte));
            if (!created)
            {
                ModelState.AddModelError("", "Impossible de créer le stock (doublon produit+entrepôt, identifiants invalides ou données incorrectes).");
                await FillSelectLists();
                return View(stock);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        // GET: Stocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var stock = await _mediator.Send(new GetStockByIdQuery(id.Value));
            if (stock == null) return NotFound();

            await FillSelectLists();
            return View(stock);
        }

        // POST: Stocks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProduitId,EntrepotId,QuantiteDisponible,SeuilAlerte")] Stock stock)
        {
            if (id != stock.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                await FillSelectLists();
                return View(stock);
            }

            var updated = await _mediator.Send(new UpdateStockCommand(stock.Id, stock.ProduitId, stock.EntrepotId, stock.QuantiteDisponible, stock.SeuilAlerte));
            if (updated == null)
            {
                ModelState.AddModelError("", "Impossible de mettre à jour (stock introuvable, doublon produit+entrepôt ou données invalides).");
                await FillSelectLists();
                return View(stock);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        // GET: Stocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var stock = await _mediator.Send(new GetStockByIdQuery(id.Value));
            if (stock == null) return NotFound();

            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _mediator.Send(new DeleteStockCommand(id));
            if (!deleted)
            {
                ModelState.AddModelError("", "Impossible de supprimer le stock (introuvable ou erreur).");
                var s = await _mediator.Send(new GetStockByIdQuery(id));
                return View(s);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Helpers
        private async Task FillSelectLists()
        {
            // Utilise les queries existantes pour obtenir Entrepots et Produits
            var entrepots = await _mediator.Send(new GetEntrepotsQuery());
            var produits = await _mediator.Send(new GetProduitsQuery());

            // Remplacer les affichages "Id" par "Nom" si tu as ces propriétés dans les vues
            ViewData["EntrepotId"] = new SelectList(entrepots, "Id", "Nom");
            ViewData["ProduitId"] = new SelectList(produits, "Id", "Nom");
        }
        #endregion
    }
}
