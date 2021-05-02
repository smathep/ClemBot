using System.Collections.Generic;
using System.Linq;
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
    public class Add
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

            public int GuildId { get; set; }
        }

        public record Handler(ClemBotContext _context) : IRequestHandler<Command, int>
        {
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var role = new Role()
            {
                Id = request.Id,
                Name = request.Name,
                GuildId = request.GuildId
            };
            _context.Roles.Add(role);

            await _context.SaveChangesAsync();

            return role.Id;
        }
    }
}
}
