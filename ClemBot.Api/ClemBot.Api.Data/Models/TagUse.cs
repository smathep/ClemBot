using System;

namespace ClemBot.Api.Data.Models
{
    public class TagUse
    {
        public ulong Id { get; set; }

        public DateTime Time { get; set; }

        public ulong UserId { get; set; }
        public User User { get; set; }

        public ulong TagId { get; set; }
        public Tag Tag { get; set; }

        public ulong ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}
