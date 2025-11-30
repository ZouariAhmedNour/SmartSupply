// SmartSupply1.Controllers.EventMetiersController.cs
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SmartSupply.Application.Queries.EventMetiers;

namespace SmartSupply1.Controllers
{
    public class EventMetiersController : Controller
    {
        private readonly IMediator _mediator;

        public EventMetiersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: EventMetiers
        public async Task<IActionResult> Index()
        {
            var eventsList = await _mediator.Send(new GetEventMetiersQuery());
            return View(eventsList);
        }

        // GET: EventMetiers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ev = await _mediator.Send(new GetEventMetierByIdQuery(id.Value));
            if (ev == null) return NotFound();

            return View(ev);
        }
    }
}
