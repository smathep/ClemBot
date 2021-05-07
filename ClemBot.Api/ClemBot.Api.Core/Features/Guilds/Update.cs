using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    public class Update
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(p => p.Id).NotNull();
                RuleFor(p => p.Name).NotNull();
                RuleFor(p => p.Users).NotEmpty();
            }
        }

        public class Command : IRequest<Result<ulong, QueryStatus>>
        {
            public ulong Id { get; set; }

            public string Name { get; set; } = null!;

            public List<ulong> Users { get; set; } = new();
        }

        public record Handler(ClemBotContext _context) : IRequestHandler<Command, Result<ulong, QueryStatus>>
        {
            public async Task<Result<ulong, QueryStatus>> Handle(Command request, CancellationToken cancellationToken)
            {
                var guild = await _context.Guilds
                    .Where(x => x.Id == request.Id)
                    .Include(y => y.Users)
                    .FirstOrDefaultAsync();

                if (guild is null)
                {
                    return QueryResult<ulong>.NotFound();
                }

                guild.Name = request.Name;

                var users = await _context.Users
                    .Where(x => request.Users.Contains(x.Id))
                    .ToListAsync();

                guild.Users = users;
                await _context.SaveChangesAsync();

                return QueryResult<ulong>.Success(guild.Id);
            }

        }
    }
}
