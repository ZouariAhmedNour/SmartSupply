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
    public class EventMetiersController : Controller
    {
        private readonly SmartSupplyDbContext _context;

        public EventMetiersController(SmartSupplyDbContext context)
        {
            _context = context;
        }

        // GET: EventMetiers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        // GET: EventMetiers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventMetier = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventMetier == null)
            {
                return NotFound();
            }

            return View(eventMetier);
        }

        // GET: EventMetiers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventMetiers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TypeEvent,Donnees,DateEvent")] EventMetier eventMetier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventMetier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventMetier);
        }

        // GET: EventMetiers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventMetier = await _context.Events.FindAsync(id);
            if (eventMetier == null)
            {
                return NotFound();
            }
            return View(eventMetier);
        }

        // POST: EventMetiers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeEvent,Donnees,DateEvent")] EventMetier eventMetier)
        {
            if (id != eventMetier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventMetier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventMetierExists(eventMetier.Id))
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
            return View(eventMetier);
        }

        // GET: EventMetiers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventMetier = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventMetier == null)
            {
                return NotFound();
            }

            return View(eventMetier);
        }

        // POST: EventMetiers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventMetier = await _context.Events.FindAsync(id);
            if (eventMetier != null)
            {
                _context.Events.Remove(eventMetier);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventMetierExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
