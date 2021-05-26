using System;
using System.Threading.Tasks;
using ClemBot.Api.Core.Features.Tags;
using ClemBot.Api.Core.Security;
using ClemBot.Api.Core.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClemBot.Api.Core.Features.Tags
{
    [ApiController]
    [Route("api")]
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

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

        [HttpGet("bot/[controller]")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> Details(Bot.Details.Query query) =>
            await _mediator.Send(query) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                _ => NoContent()
            };

        [HttpDelete("bot/[controller]")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> Delete(Bot.Delete.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NoContent(),
                _ => throw new InvalidOperationException()
            };

        [HttpPost("bot/[controller]/invoke")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> AddUse(Bot.Invoke.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.Conflict } => Conflict(),
                _ => throw new InvalidOperationException()
            };
    }
}
