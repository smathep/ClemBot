using System.Collections.Generic;

namespace ClemBot.Api.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        
        public int Name { get; set; }

        public List<Guild> Guilds { get; set; } = new();
    }
}