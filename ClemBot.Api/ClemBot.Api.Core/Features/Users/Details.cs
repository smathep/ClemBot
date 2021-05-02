using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClemBot.Api.Data.Contexts;
using ClemBot.Api.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClemBot.Api.Core.Features.Users
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

            public List<int> Guilds { get; set; } = new();
        }

        public record QueryHandler(ClemBotContext _context) : IRequestHandler<Query, Model>
        {
            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .Where(x => x.Id == request.Id)
                    .Include(y => y.Guilds)
                    .FirstAsync();

                return new Model()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Guilds = user.Guilds.Select(x => x.Id).ToList()
                };
            }
        }
    }
}
