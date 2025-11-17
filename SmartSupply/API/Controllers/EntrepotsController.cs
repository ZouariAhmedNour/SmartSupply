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
    public class EntrepotsController : Controller
    {
        private readonly SmartSupplyDbContext _context;

        public EntrepotsController(SmartSupplyDbContext context)
        {
            _context = context;
        }

        // GET: Entrepots
        public async Task<IActionResult> Index()
        {
            return View(await _context.Entrepots.ToListAsync());
        }

        // GET: Entrepots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrepot = await _context.Entrepots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entrepot == null)
            {
                return NotFound();
            }

            return View(entrepot);
        }

        // GET: Entrepots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Entrepots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Adresse,CapaciteMax,DateCreation")] Entrepot entrepot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entrepot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entrepot);
        }

        // GET: Entrepots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrepot = await _context.Entrepots.FindAsync(id);
            if (entrepot == null)
            {
                return NotFound();
            }
            return View(entrepot);
        }

        // POST: Entrepots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Adresse,CapaciteMax,DateCreation")] Entrepot entrepot)
        {
            if (id != entrepot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entrepot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntrepotExists(entrepot.Id))
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
            return View(entrepot);
        }

        // GET: Entrepots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrepot = await _context.Entrepots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entrepot == null)
            {
                return NotFound();
            }

            return View(entrepot);
        }

        // POST: Entrepots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entrepot = await _context.Entrepots.FindAsync(id);
            if (entrepot != null)
            {
                _context.Entrepots.Remove(entrepot);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntrepotExists(int id)
        {
            return _context.Entrepots.Any(e => e.Id == id);
        }
    }
}
