using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClemBot.Api.Core.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClemBot.Api.Core.Features.Roles
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
            => Ok(await _mediator.Send(new Index.Query()));

        [HttpGet("{Id}")]
        public async Task<IActionResult> Details([FromRoute] Details.Query query) =>
            await _mediator.Send(query) switch
            {
                { Status: ResultStatus.Success } result => Ok(result.Value),
                _ => Ok(new List<int>())
            };


        [HttpPost]
        public async Task<IActionResult> Create(Create.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: ResultStatus.Success } result => Ok(result.Value),
                { Status: ResultStatus.Conflict } => Conflict(),
                _ => throw new InvalidOperationException()
            };

        [HttpPatch]
        public async Task<IActionResult> Edit(Edit.Command command) =>
            await _mediator.Send(command) switch
            {
                { Status: ResultStatus.Success } result => Ok(result.Value),
                _ => NotFound()
            };
    }
}
