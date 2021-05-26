using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClemBot.Api.Core.Features.Users;
using ClemBot.Api.Core.Security;
using ClemBot.Api.Core.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClemBot.Api.Core.Features.Users
{
    [ApiController]
    [Route("api")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("bot/[controller]")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> Index() =>
            await _mediator.Send(new Bot.Index.Query()) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                _ => Ok(new List<ulong>())
            };


        [HttpGet("bot/[controller]/{Id}")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> Details([FromRoute] Bot.Details.Query query) =>
            await _mediator.Send(query) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                _ => NoContent()
            };

        [HttpPost("bot/[controller]")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> Create(Bot.Create.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.Conflict } => Conflict(),
                _ => throw new InvalidOperationException()
            };

        [HttpPatch("bot/[controller]")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> Edit(Bot.Edit.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                _ => NotFound()
            };
    }
}
