using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClemBot.Api.Core.Features.Guilds;
using ClemBot.Api.Core.Security;
using ClemBot.Api.Core.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClemBot.Api.Core.Features.Guilds
{
    [ApiController]
    [Route("api")]
    public class GuildsController : ControllerBase
    {
        private readonly ILogger<GuildsController> _logger;

        private readonly IMediator _mediator;

        public GuildsController(ILogger<GuildsController> logger, IMediator mediator)
        {
            _logger = logger;
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

        [HttpGet("bot/[controller]/{id}")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> Details([FromRoute] Bot.Details.Query query) =>
            await _mediator.Send(query) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                _ => NoContent()
            };

        [HttpDelete("bot/[controller]/{Id}")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> Delete([FromRoute] Bot.Delete.Query query) =>
            await _mediator.Send(query) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NoContent(),
                _ => throw new InvalidOperationException()
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

        [HttpPost("bot/[controller]/AddUser")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> AddUser(Bot.AddUser.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.Conflict } => Conflict(),
                _ => throw new InvalidOperationException()
            };

        [HttpDelete("bot/[controller]/RemoveUser")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> RemoveUser(Bot.RemoveUser.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NotFound(),
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

        [HttpPatch("bot/[controller]/{Id}/Update/Users")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> UpdateUsers(ulong Id, Bot.UpdateUsers.Command command) =>
            await _mediator.Send(command with { Id = Id }) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NotFound(),
                _ => throw new InvalidOperationException()
            };

        [HttpPatch("bot/[controller]/{Id}/Update/Roles")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> UpdateRoles(ulong Id, Bot.UpdateRoles.Command command) =>
            await _mediator.Send(command with { Id = Id }) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NotFound(),
                _ => throw new InvalidOperationException()
            };

        [HttpPatch("bot/[controller]/{Id}/Update/Channels")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> UpdateChannels(ulong Id, Bot.UpdateChannels.Command command) =>
            await _mediator.Send(command with { Id = Id }) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NotFound(),
                _ => throw new InvalidOperationException()
            };

        [HttpGet("bot/[controller]/{Id}/Roles")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> Roles([FromRoute] Bot.Roles.Query query) =>
            await _mediator.Send(query) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NoContent(),
                _ => throw new InvalidOperationException()
            };

        [HttpGet("bot/[controller]/{Id}/Tags")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> Tags([FromRoute] Bot.Tags.Query query) =>
            await _mediator.Send(query) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NoContent(),
                _ => throw new InvalidOperationException()
            };

        [HttpGet("bot/[controller]/{Id}/CustomPrefixes")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> Tags([FromRoute] Bot.CustomPrefixes.Query query) =>
            await _mediator.Send(query) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NoContent(),
                _ => throw new InvalidOperationException()
            };

        [HttpGet("bot/[controller]/{Id}/DesignatedChannels")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> Index([FromRoute] Bot.DesignatedChannels.Query command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NoContent(),
                _ => throw new InvalidOperationException()
            };

        [HttpGet("bot/[controller]/{Id}/Infractions")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> Index([FromRoute] Bot.Infractions.Query command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NoContent(),
                _ => throw new InvalidOperationException()
            };

        [HttpPost("bot/[controller]/{Id}/SetWelcomeMessage")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> SetWelcomeMessage(ulong Id, [FromBody] Bot.SetWelcomeMessage.Command command) =>
            await _mediator.Send(command with { Id = Id }) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NoContent(),
                _ => throw new InvalidOperationException()
            };

        [HttpGet("bot/[controller]/{Id}/GetWelcomeMessage")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> GetWelcomeMessage([FromRoute] Bot.GetWelcomeMessage.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NoContent(),
                _ => throw new InvalidOperationException()
            };

        [HttpGet("bot/[controller]/{Id}/DeleteWelcomeMessage")]
        [Authorize(Policy = Policies.BotMaster)]
        public async Task<IActionResult> DeleteWelcomeMessage(ulong Id, Bot.DeleteWelcomeMessage.Command command) =>
            await _mediator.Send(command with { Id = Id }) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NoContent(),
                _ => throw new InvalidOperationException()
            };
    }
}
