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
    public class Create
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(p => p.Id).NotNull();
                RuleFor(p => p.Name).NotNull();
            }
        }

        public class Command : IRequest<IResult<int>>
        {
            public int Id { get; set; }

            public string Name { get; set; } = null!;
        }

        public record Handler(ClemBotContext _context) : IRequestHandler<Command, IResult<int>>
        {
            public async Task<IResult<int>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = new User()
                {
                    Id = request.Id,
                    Name = request.Name,
                };

                if (await _context.Roles.Where(x => x.Id == user.Id).AnyAsync())
                {
                    return Result<int>.Conflict();
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Result<int>.Success(user.Id);
            }
        }
    }
}
