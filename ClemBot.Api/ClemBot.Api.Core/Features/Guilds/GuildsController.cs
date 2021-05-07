using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClemBot.Api.Core.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClemBot.Api.Core.Features.Guilds
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class GuildsController : ControllerBase
    {
        private readonly ILogger<GuildsController> _logger;

        private readonly IMediator _mediator;

        public GuildsController(ILogger<GuildsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index() =>
            await _mediator.Send(new Index.Query()) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                _ => Ok(new List<ulong>())
            };

        [HttpGet("{Id}")]
        public async Task<IActionResult> Details([FromRoute] Details.Query query) =>
            await _mediator.Send(query) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                _ => NoContent()
            };

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] Delete.Query query) =>
            await _mediator.Send(query) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                _ => Ok(new List<ulong>())
            };

        [HttpPost]
        public async Task<IActionResult> Create(Create.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.Conflict } => Conflict(),
                _ => throw new InvalidOperationException()
            };

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(AddUser.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.Conflict } => Conflict(),
                _ => throw new InvalidOperationException()
            };

        [HttpDelete("RemoveUser")]
        public async Task<IActionResult> RemoveUser(RemoveUser.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NotFound(),
                _ => throw new InvalidOperationException()
            };

        [HttpPatch]
        public async Task<IActionResult> Edit(Edit.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                _ => NotFound()
            };

        [HttpPatch("Update")]
        public async Task<IActionResult> Edit(Update.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                _ => NotFound()
            };
    }
}
