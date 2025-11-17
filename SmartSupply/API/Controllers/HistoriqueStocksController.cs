using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;

namespace SmartSupply.API.Controllers
{
    public class HistoriqueStocksController : Controller
    {
        private readonly SmartSupplyDbContext _context;

        public HistoriqueStocksController(SmartSupplyDbContext context)
        {
            _context = context;
        }

        // GET: HistoriqueStocks
        public async Task<IActionResult> Index()
        {
            return View(await _context.HistoriqueStock.ToListAsync());
        }

        // GET: HistoriqueStocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historiqueStock = await _context.HistoriqueStock
                .FirstOrDefaultAsync(m => m.Id == id);
            if (historiqueStock == null)
            {
                return NotFound();
            }

            return View(historiqueStock);
        }

        // GET: HistoriqueStocks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HistoriqueStocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProduitId,EntrepotId,Quantite,DateMouvement,TypeMouvement,Commentaire")] HistoriqueStock historiqueStock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(historiqueStock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(historiqueStock);
        }

        // GET: HistoriqueStocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historiqueStock = await _context.HistoriqueStock.FindAsync(id);
            if (historiqueStock == null)
            {
                return NotFound();
            }
            return View(historiqueStock);
        }

        // POST: HistoriqueStocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProduitId,EntrepotId,Quantite,DateMouvement,TypeMouvement,Commentaire")] HistoriqueStock historiqueStock)
        {
            if (id != historiqueStock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historiqueStock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoriqueStockExists(historiqueStock.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(historiqueStock);
        }

        // GET: HistoriqueStocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historiqueStock = await _context.HistoriqueStock
                .FirstOrDefaultAsync(m => m.Id == id);
            if (historiqueStock == null)
            {
                return NotFound();
            }

            return View(historiqueStock);
        }

        // POST: HistoriqueStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var historiqueStock = await _context.HistoriqueStock.FindAsync(id);
            if (historiqueStock != null)
            {
                _context.HistoriqueStock.Remove(historiqueStock);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoriqueStockExists(int id)
        {
            return _context.HistoriqueStock.Any(e => e.Id == id);
        }
    }
}
