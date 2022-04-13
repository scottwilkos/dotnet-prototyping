using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prototyping.Business.Cqrs;

namespace Prototyping.Web.TournamentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentMongoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TournamentMongoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddTournamentMongoCommand command)
            =>  Ok(await _mediator.Send(command));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _mediator.Send(new GetTournamentsMongoQuery()));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(await _mediator.Send(new GetTournamentMongoQuery{Id = id}));
    }
}