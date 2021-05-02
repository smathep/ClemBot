using System;

namespace ClemBot.Api.Data.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime Time { get; set; }

        public int GuildId { get; set; }
        public Guild Guild { get; set; }

        public int ChannelId { get; set; }
        public Channel Channel { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
