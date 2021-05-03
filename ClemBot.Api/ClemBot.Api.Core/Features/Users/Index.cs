using System.Collections.Generic;
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

namespace ClemBot.Api.Core.Features.Users
{
    public class Index
    {
        public class Query : IRequest<IResult<IEnumerable<Model>>>
        {
        }

        public class Model
        {
            public int Id { get; set; }

            public string Name { get; set; } = null!;
        }

        public record Handler(ClemBotContext _context) : IRequestHandler<Query, IResult<IEnumerable<Model>>>
        {
            public async Task<IResult<IEnumerable<Model>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _context.Users.ToListAsync();
                if (!users.Any())
                {
                    return Result<IEnumerable<Model>>.NotFound();
                }

                return Result<IEnumerable<Model>>.Success(users.Select(x => new Model()
                {
                    Id = x.Id,
                    Name = x.Name
                }));
            }
        }
    }
}
