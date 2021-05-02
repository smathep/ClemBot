using ClemBot.Api.Data.Enums;

namespace ClemBot.Api.Data.Models
{
    public class DesignatedChannelMapping
    {
        public int Id { get; set; }

        public DesignatedChannels Type { get; set; }

        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}
