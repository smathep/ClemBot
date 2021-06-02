using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClemBot.Api.Core.Utilities;
using ClemBot.Api.Data.Contexts;
using ClemBot.Api.Data.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClemBot.Api.Core.Features.Guilds.Bot
{
    public class UpdateRoles
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(p => p.Id).NotNull();
                RuleFor(p => p.Roles).NotEmpty();
            }
        }

        public class RoleDto
        {
            public ulong Id { get; set; }

            public string? Name { get; set; }

            public List<ulong> Members { get; set; } = new();
        }

        public record Command : IRequest<Result<ulong, QueryStatus>>
        {
            public ulong Id { get; set; }

            public List<RoleDto> Roles { get; set; } = new();
        }

        public record Handler(ClemBotContext _context)
            : IRequestHandler<Command, Result<ulong, QueryStatus>>
        {
            public async Task<Result<ulong, QueryStatus>> Handle(Command request, CancellationToken cancellationToken)
            {
                var guild = await _context.Guilds
                    .Where(x => x.Id == request.Id)
                    .Include(y => y.Roles)
                    .FirstOrDefaultAsync();

                var roles = guild?.Roles ?? new List<Role>();

                if (guild is null)
                {
                    return QueryResult<ulong>.NotFound();
                }

                // Get all roles that are common to both enumerables and check for a name change
                foreach (var roleId in roles
                    .Select(x => x.Id)
                    .Intersect(request.Roles
                        .Select(x => x.Id)))
                {
                    var role = roles.First(x => x.Id == roleId);
                    role.Name = request.Roles.First(x => x.Id == roleId).Name;
                }

                // Get all roles that have been deleted
                foreach (var role in roles.Where(x => request.Roles.All(y => y.Id != x.Id)).ToList())
                {
                    _context.Roles.Remove(role);
                }

                // get new roles
                foreach (var role in request.Roles.Where(x => roles.All(y => y.Id != x.Id)))
                {
                    var roleEntity = new Role
                    {
                        Id = role.Id,
                        Name = role.Name,
                        GuildId = request.Id,
                        IsAssignable = false
                    };
                    _context.Roles.Add(roleEntity);
                    guild.Roles.Add(roleEntity);
                }

                foreach (var roleDto in request.Roles)
                {
                    var role = await _context.Roles
                        .Include(y => y.Users)
                        .FirstOrDefaultAsync(x => roleDto.Id == x.Id);

                    var members = await _context.Users
                        .Where(x => roleDto.Members.Contains(x.Id))
                        .ToListAsync();

                    role.Users.Clear();
                    role.Users = members;
                }

                await _context.SaveChangesAsync();

                return QueryResult<ulong>.Success(request.Id);
            }
        }
    }
}
