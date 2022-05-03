using System.Security.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
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

        [HttpGet("recordCount")]
        public async Task<IActionResult> GetRecordCount() => Ok(await this._mediator.Send(new GetTournamentRecordCountMongoQuery()));

        [HttpGet("noHandler/{id}")]
        public async Task<IActionResult> GetNoHandler(string id, [FromServices] IConfiguration configuration)
        {
             var mongoConnectionString = configuration.GetSection("MongoDb:ConnectionString").Value;

            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(mongoConnectionString)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

            var client = new MongoClient(settings);
            var database = client.GetDatabase("TournamentsDatabase");
            var tournaments = database.GetCollection<TournamentMongoDto>("Tournaments");

            return Ok(await tournaments.Find(_ => _.Id.Equals(id)).SingleOrDefaultAsync());
        }
    }
}