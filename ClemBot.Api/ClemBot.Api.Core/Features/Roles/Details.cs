using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClemBot.Api.Data.Contexts;
using ClemBot.Api.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClemBot.Api.Core.Features.Roles
{
    public class Details
    {
        public class Query : IRequest<Model>
        {
            public int Id { get; set; }
        }

        public class Model
        {
            public int Id { get; set; }

            public string? Name { get; set; }

            public int GuildId { get; set; }
        }

        public record QueryHandler(ClemBotContext _context) : IRequestHandler<Query, Model>
        {
            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
        {
            var role = await _context.Roles
                .Where(x => x.Id == request.Id)
                .FirstAsync();

            return new Model()
            {
                Id = role.Id,
                Name = role.Name,
                GuildId = role.GuildId
            };
        }
    }
}
}
