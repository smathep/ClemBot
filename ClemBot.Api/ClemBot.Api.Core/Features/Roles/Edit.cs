using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ClemBot.Api.Core.Utilities;
using ClemBot.Api.Data.Contexts;
using ClemBot.Api.Data.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClemBot.Api.Core.Features.Roles
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

        public record Handler(ClemBotContext _context) : IRequestHandler<Command,IResult<int>>
        {
            public async Task<IResult<int>> Handle(Command request, CancellationToken cancellationToken)
            {
                var role = await _context.Roles
                   .FirstOrDefaultAsync(g => g.Id == request.Id);


                if (role is null)
                {
                    return Result<int>.NotFound();
                }

                role.Name = request.Name;
                await _context.SaveChangesAsync();

                return Result<int>.Success(role.Id);
            }
        }
    }
}
