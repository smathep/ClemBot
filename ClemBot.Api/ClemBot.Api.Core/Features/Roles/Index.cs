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
    public class Index
    {
        public class Query : IRequest<IEnumerable<Model>>
        {
        }

        public class Model
        {
            public int Id { get; set; }

            public string Name { get; set; } = null!;

            public int GuildId { get; set; }
        }

        public record Handler(ClemBotContext _context) : IRequestHandler<Query, IEnumerable<Model>>
        {
            public async Task<IEnumerable<Model>> Handle(Query request, CancellationToken cancellationToken)
        {
            var users = await _context.Roles.ToListAsync();
            return users.Select(x => new Model()
            {
                Id = x.Id,
                Name = x.Name,
                GuildId = x.GuildId
            });
        }
    }
}
}
