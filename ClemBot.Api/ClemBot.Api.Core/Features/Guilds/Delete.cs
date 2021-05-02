using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClemBot.Api.Core.Utilities;
using ClemBot.Api.Data.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClemBot.Api.Core.Features.Guilds
{
    public class Delete
    {
        public class Query : IRequest<IResult<Model>>
        {
            public int Id { get; set; }
        }

        public class Model
        {
            public int Id { get; set; }

            public string? Name { get; set; }
        }

        public record QueryHandler(ClemBotContext _context) : IRequestHandler<Query, IResult<Model>>
        {
            public async Task<IResult<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                var guild = await _context.Guilds
                   .FirstOrDefaultAsync(g => g.Id == request.Id);

                if (guild is null)
                {
                    return Result<Model>.NotFound();
                }

                _context.Guilds.Remove(guild);
                await _context.SaveChangesAsync();

                return Result<Model>.Success(new Model()
                {
                    Id = guild.Id,
                    Name = guild.Name
                });
            }
        }
    }
}
