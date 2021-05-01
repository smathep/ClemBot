using System.Threading;
using System.Threading.Tasks;
using ClemBot.Api.Data.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClemBot.Api.Core.Features.Guilds
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
            
            public string? WelcomeMessage { get; set; }
        }
        
        public record QueryHandler(ClemBotContext _context) : IRequestHandler<Query, Model>
        {
            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                var guild = await _context.Guilds
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                return new Model()
                {
                    Id = guild.Id,
                    Name = guild.Name,
                    WelcomeMessage = guild.WelcomeMessage
                };
            }
        }
    }
}