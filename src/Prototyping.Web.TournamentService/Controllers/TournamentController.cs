using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prototyping.Business.Cqrs;
using Prototyping.Business.Cqrs.Queries;

namespace Prototyping.Web.TournamentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private IMediator _mediator { get; }

        public TournamentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTournamentCommand command)
        {
            var Tournament = await this._mediator.Send(command);
            return Ok(Tournament);
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await this._mediator.Send(new GetTournamentsQuery()));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            GetTournamentQuery query = new GetTournamentQuery { Id = id };
            var Tournament = await this._mediator.Send(query);
            return Ok(Tournament);
        }
    }
}
