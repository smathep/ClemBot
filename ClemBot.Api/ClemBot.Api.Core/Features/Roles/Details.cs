using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClemBot.Api.Core.Utilities;
using ClemBot.Api.Data.Contexts;
using ClemBot.Api.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClemBot.Api.Core.Features.Roles
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

            public int GuildId { get; set; }
        }

        public record QueryHandler(ClemBotContext _context) : IRequestHandler<Query, IResult<Model>>
        {
            public async Task<IResult<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                var role = await _context.Roles
                    .Where(x => x.Id == request.Id)
                    .FirstAsync();

                if (role is null)
                {
                    return Result<Model>.NotFound();
                }
                return Result<Model>.Success(new Model()
                {
                    Id = role.Id,
                    Name = role.Name,
                    GuildId = role.GuildId
                });
            }
        }
    }
}
