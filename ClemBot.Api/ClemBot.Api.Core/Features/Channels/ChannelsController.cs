using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClemBot.Api.Core.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClemBot.Api.Core.Features.Channels
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChannelsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChannelsController(IMediator mediator)
        {
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
                { Status: QueryStatus.NotFound } => NoContent(),
                _ => throw new InvalidOperationException()
            };

        [HttpPost]
        public async Task<IActionResult> Create(Create.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.Conflict } => Conflict(),
                _ => throw new InvalidOperationException()
            };

        [HttpPatch]
        public async Task<IActionResult> Edit(Edit.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                _ => NotFound()
            };
    }
}
