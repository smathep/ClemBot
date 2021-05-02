using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ClemBot.Api.Data.Contexts;
using ClemBot.Api.Data.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClemBot.Api.Core.Features.Roles
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

        public class Command : IRequest<int>
        {
            public int Id { get; set; }

            public string Name { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, int>
        {
            public Handler(ClemBotContext _context)
            {
                this._context = _context;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var role = await _context.Roles
                   .FirstOrDefaultAsync(g => g.Id == request.Id);

                role.Name = request.Name;
                await _context.SaveChangesAsync();

                return role.Id;
            }

            public ClemBotContext _context { get; init; }

            public void Deconstruct(out ClemBotContext _context)
            {
                _context = this._context;
            }
        }
    }
}
