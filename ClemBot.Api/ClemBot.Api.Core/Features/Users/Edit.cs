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
    public class Edit
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
                var user = await _context.Users
                   .FirstOrDefaultAsync(g => g.Id == request.Id);

                if (user is null)
                {
                    return Result<int>.NotFound();
                }

                user.Name = request.Name;
                await _context.SaveChangesAsync();

                return Result<int>.Success(user.Id);
            }
        }
    }
}
