using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartSupply.Application.Commands.Commandes;
using SmartSupply.Application.Handlers.Commandes;
using SmartSupply.Application.Queries.Commandes;
using SmartSupply.Domain.Models;
using SmartSupply.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSupply.API.Controllers
{
    public class CommandesController : Controller
    {
        private readonly IMediator _mediator;

        public CommandesController(IMediator mediator) => _mediator = mediator;


        // GET: Commandes
        public async Task<IActionResult> Index()
        {
            var res = await _mediator.Send(new GetCommandesQuery());
            if (!res.IsSuccess) return View("Error", res.Error);
            return View(res.Data);
        }

        // GET: Commandes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var res = await _mediator.Send(new GetCommandeByIdQuery(id.Value));
            if (!res.IsSuccess) return NotFound();
            return View(res.Data);
        }

        // GET: Commandes/Create
        public IActionResult Create() => View();


        // POST: Commandes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string ClientNom, string ClientEmail, int ProduitId = 0, int Quantite = 0, decimal PrixUnitaire = 0m)
        {
            if (string.IsNullOrWhiteSpace(ClientNom) || string.IsNullOrWhiteSpace(ClientEmail))
            {
                ModelState.AddModelError("", "Le nom et l'email du client sont requis.");
                return View();
            }
            {
                var lignes = new List<LigneCommande>();
                if (ProduitId > 0 && Quantite > 0)
                {
                    lignes.Add(new LigneCommande
                    {
                        ProduitId = ProduitId,
                        Quantite = Quantite,
                        PrixUnitaire = PrixUnitaire // 0 signifie "pas fourni" selon le handler
                    });
                }

                var cmd = new CreateCommandeCommand(ClientNom, ClientEmail, lignes);
                var res = await _mediator.Send(cmd);
                if (!res.IsSuccess)
                {
                    ModelState.AddModelError("", res.Error);
                    return View();
                }

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Commandes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();


            var res = await _mediator.Send(new GetCommandeByIdQuery(id.Value));
            if (!res.IsSuccess) return NotFound();
            return View(res.Data);
        }

        // POST: Commandes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string ClientNom, string ClientEmail, string Statut)
        {
            // validation simple
            if (string.IsNullOrWhiteSpace(ClientNom) || string.IsNullOrWhiteSpace(ClientEmail))
            {
                ModelState.AddModelError("", "Nom et email requis.");
                // recharger la commande pour la vue
                var getErr = await _mediator.Send(new GetCommandeByIdQuery(id));
                return View(getErr.Data);
            }

            var cmd = new UpdateCommandeCommand(id, ClientNom, ClientEmail, Statut);
            var res = await _mediator.Send(cmd);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.Error);
                var get = await _mediator.Send(new GetCommandeByIdQuery(id));
                return View(get.Data);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Commandes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var res = await _mediator.Send(new GetCommandeByIdQuery(id.Value));
            if (!res.IsSuccess) return NotFound();
            return View(res.Data); // View attend Commande
        }

        // POST: Commandes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var res = await _mediator.Send(new DeleteCommandeCommand(id));
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.Error);
                var get = await _mediator.Send(new GetCommandeByIdQuery(id));
                return View(get.Data);
            }
            return RedirectToAction(nameof(Index));
        }


    }


    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatut(int id, string statut)
        {
            // validation basique
            if (string.IsNullOrWhiteSpace(statut))
            {
                ModelState.AddModelError("", "Statut requis.");
                var getErr = await _mediator.Send(new GetCommandeByIdQuery(id));
                return View("Edit", getErr.Data); // ou RedirectToAction("Edit", new { id })
            }

            var res = await _mediator.Send(new UpdateCommandeStatutCommand(id, statut));
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.Error);
                var get = await _mediator.Send(new GetCommandeByIdQuery(id));
                return View("Edit", get.Data);
            }

            return RedirectToAction(nameof(Index));
        }
    }
    }

