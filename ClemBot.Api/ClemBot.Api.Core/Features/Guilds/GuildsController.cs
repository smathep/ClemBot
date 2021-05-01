using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql.Replication.PgOutput.Messages;

namespace ClemBot.Api.Core.Features.Guilds
{
    [ApiController]
    [Route("[controller]")]
    public class GuildsController : ControllerBase
    {
        private readonly ILogger<GuildsController> _logger;

        private readonly IMediator _mediator;

        public GuildsController(ILogger<GuildsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        [HttpGet("index")]
        public async Task<IActionResult> Index()
            => Ok(await _mediator.Send(new Index.Query()));

        [HttpGet("details/{Id}")]
        public async Task<IActionResult> Details([FromRoute]Details.Query query)
            => Ok(await _mediator.Send(query));

        [HttpPut("add")]
        public async Task<IActionResult> Create(Add.Command command)
        {
            var c = await _mediator.Send(command);
            return Ok(c);
        }
        
        [HttpPost("edit")]
        public async Task<IActionResult> Create(Edit.Command command)
        {
            var c = await _mediator.Send(command);
            return Ok(c);
        }
    }
}
