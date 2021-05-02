using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ClemBot.Api.Core.Utilities;
using ClemBot.Api.Data.Contexts;
using ClemBot.Api.Data.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClemBot.Api.Core.Features.Guilds
{
    public class Edit
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(p => p.Id).NotNull();
                RuleFor(p => p.Name).NotNull();
            }
        }

        public class Command : IRequest<IResult<int>>
        {
            public int Id { get; set; }

            public string Name { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, IResult<int>>
        {
            public Handler(ClemBotContext _context)
            {
                this._context = _context;
            }

            public async Task<IResult<int>> Handle(Command request, CancellationToken cancellationToken)
            {
                var guild = await _context.Guilds
                   .FirstOrDefaultAsync(g => g.Id == request.Id);

                if (guild is null)
                {
                    return Result<int>.NotFound();
                }

                guild.Name = request.Name;
                await _context.SaveChangesAsync();

                return Result<int>.Success(guild.Id);
            }

            public ClemBotContext _context { get; init; }

            public void Deconstruct(out ClemBotContext _context)
            {
                _context = this._context;
            }
        }
    }
}
