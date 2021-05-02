using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClemBot.Api.Core.Utilities;
using ClemBot.Api.Data.Contexts;
using ClemBot.Api.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClemBot.Api.Core.Features.Guilds
{
    public class Details
    {
        public class Query : IRequest<IResult<Model>>
        {
            public int Id { get; set; }
        }

        public class Model
        {
            public int Id { get; set; }
            
            public string? Name { get; set; }
            
            public string? WelcomeMessage { get; set; }
            
            public List<int> Users { get; set; } = new();
        }
        
        public record QueryHandler(ClemBotContext _context) : IRequestHandler<Query, IResult<Model>>
        {
            public async Task<IResult<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                var guild = await _context.Guilds
                    .Where(x => x.Id == request.Id)
                    .Include(y => y.Users)
                    .FirstOrDefaultAsync();

                if (guild is null)
                {
                    return Result<Model>.NotFound();
                }
                
                return Result<Model>.Success(new Model()
                {
                    Id = guild.Id,
                    Name = guild.Name,
                    WelcomeMessage = guild.WelcomeMessage,
                    Users = guild.Users.Select(x => x.Id).ToList()
                });
            }
        }
    }
}