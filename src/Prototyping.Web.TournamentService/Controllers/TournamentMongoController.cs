using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetTournamentsMongoQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _mediator.Send(new GetTournamentMongoQuery{Id = id}));
        }
    }

    public class AddTournamentCommandDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string Description { get; set; }
    }
}