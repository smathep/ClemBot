using System.Collections.Generic;
using System.Diagnostics;
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

namespace ClemBot.Api.Core.Features.Guilds
{
    public class UpdateChannels
    {
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(p => p.Id).NotNull();
                RuleFor(p => p.Channels).NotEmpty();
            }
        }

        public class ChannelDto
        {
            public ulong Id { get; set; }

            public string? Name { get; set; }
        }

        public record Command : IRequest<Result<ulong, QueryStatus>>
        {
            public ulong Id { get; set; }

            public List<ChannelDto> Channels { get; set; } = new();
        }

        public record Handler(ClemBotContext _context)
            : IRequestHandler<Command, Result<ulong, QueryStatus>>
        {
            public async Task<Result<ulong, QueryStatus>> Handle(Command request, CancellationToken cancellationToken)
            {
                var guild = await _context.Guilds
                    .Where(x => x.Id == request.Id)
                    .Include(y => y.Channels)
                    .FirstOrDefaultAsync();

                var channels = guild?.Channels ?? new List<Channel>();

                if (guild is null)
                {
                    return QueryResult<ulong>.NotFound();
                }

                // Get all roles that are common to both enumerables and check for a name change
                foreach (var channelId in channels
                    .Select(x => x.Id)
                    .Intersect(request.Channels
                        .Select(x => x.Id)))
                {
                    var role = channels.First(x => x.Id == channelId);
                    role.Name = request.Channels.First(x => x.Id == channelId).Name;
                }

                // Get all roles that have been deleted
                foreach (var channel in channels
                    .Where(x =>
                        request.Channels.All(y => y.Id != x.Id))
                    .ToList())
                {
                    _context.Channels.Remove(channel);
                }

                // get new channels
                foreach (var channel in request.Channels
                    .Where(x =>
                        channels.All(y => y.Id != x.Id))
                    .ToList())
                {
                    var channelEntity = new Channel()
                    {
                        Id = channel.Id,
                        Name = channel.Name,
                        GuildId = request.Id
                    };
                    _context.Channels.Add(channelEntity);
                    guild?.Channels?.Add(channelEntity);
                }

                await _context.SaveChangesAsync();

                return QueryResult<ulong>.Success(request.Id);
            }
        }
    }
}
