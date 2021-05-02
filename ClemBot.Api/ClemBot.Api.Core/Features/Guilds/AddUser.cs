using System;
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
    public class AddUser
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(p => p.GuildId).NotNull();
                RuleFor(p => p.UserId).NotNull();
            }
        }

        public class Command : IRequest<IResult<int>>
        {
            public int GuildId { get; set; }
            public int UserId { get; set; }
        }

        public record Handler(ClemBotContext _context) : IRequestHandler<Command, IResult<int>>
        {
            public async Task<IResult<int>> Handle(Command request, CancellationToken cancellationToken)
            {
                var guild = await _context.Guilds
                    .Where(x => x.Id == request.UserId)
                    .Include(y => y.Users)
                    .FirstAsync();

                var user = await _context.Users
                    .Where(x => x.Id == request.UserId)
                    .Include(y => y.Guilds)
                    .FirstAsync();

                if (guild.Users.Contains(user))
                {
                    return Result<int>.Conflict();
                }

                guild.Users.Add(user);
                await _context.SaveChangesAsync();

                return Result<int>.Success(guild.Id);
            }
        }
    }
}
