using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClemBot.Api.Core.Features.Roles.Bot;
using ClemBot.Api.Core.Security;
using ClemBot.Api.Core.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClemBot.Api.Core.Features.Roles
{
    [ApiController]
    [Route("api")]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
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
        public async Task<IActionResult> Details([FromRoute] Details.Query query) =>
            await _mediator.Send(query) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                _ => NoContent()
            };

        [HttpDelete("bot/[controller]/{Id}")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> Delete([FromRoute] Delete.Query query) =>
            await _mediator.Send(query) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NoContent(),
                _ => throw new InvalidOperationException()
            };

        [HttpPost("bot/[controller]")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> Create(Create.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.Conflict } => Conflict(),
                _ => throw new InvalidOperationException()
            };

        [HttpPatch("bot/[controller]")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> Edit(Edit.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                _ => NotFound()
            };
    }
}
