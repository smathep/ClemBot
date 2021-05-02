using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Details([FromRoute] Details.Query query)
            => Ok(await _mediator.Send(query));

        [HttpPut]
        public async Task<IActionResult> Create(Add.Command command)
        {
            var c = await _mediator.Send(command);
            return Ok(c);
        }

        [HttpPatch]
        public async Task<IActionResult> Create(Edit.Command command)
        {
            var c = await _mediator.Send(command);
            return Ok(c);
        }
    }
}