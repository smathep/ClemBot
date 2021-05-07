using ClemBot.Api.Data.Enums;

namespace ClemBot.Api.Data.Models
{
    public class DesignatedChannelMapping
    {
        public ulong Id { get; set; }

        public DesignatedChannels Type { get; set; }

        public ulong ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}
