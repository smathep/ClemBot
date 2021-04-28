using System.Collections.Generic;

namespace ClemBot.Api.Data.Models
{
    public class Guild
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string? WelcomeMessage { get; set; }

        public List<User> Users { get; set; } = new();

        public List<Channel> Channels { get; set; } = new();

        public List<Message> Messages { get; set; } = new();

        public List<CustomPrefix> CustomPrefixes { get; set; } = new();
    }
}