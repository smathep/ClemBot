using System.Collections.Generic;

namespace ClemBot.Api.Data.Models
{
    public class Channel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int GuildId { get; set; }
        public Guild Guild { get; set; }

        public List<DesignatedChannelMapping> DesignatedChannels { get; set; } = new();
    }
}