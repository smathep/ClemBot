using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ClemBot.Api.Data.Contexts;
using ClemBot.Api.Data.Models;
using FluentValidation;
using MediatR;

namespace ClemBot.Api.Core.Features.Guilds
{
    public class Add
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(p => p.Id).NotNull();
                RuleFor(p => p.Name).NotNull();
            }
        }

        public class Command : IRequest<int>
        {
            public int Id { get; set; }

            public string Name { get; set; } = null!;
        }

        public record Handler(ClemBotContext _context) : IRequestHandler<Command, int>
        {
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var guild = new Guild()
                {
                    Id = request.Id,
                    Name = request.Name
                };
                _context.Guilds.Add(guild);

                await _context.SaveChangesAsync();

                return guild.Id;
            }
        }
    }
}