using System.Collections.Generic;

namespace ClemBot.Api.Data.Models
{
    public class Role
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public bool? IsAssignable { get; set; } = false;

        public ulong GuildId { get; set; }
        public Guild Guild { get; set; }

        public List<ClaimsMapping> Claims { get; set; }
    }
}
