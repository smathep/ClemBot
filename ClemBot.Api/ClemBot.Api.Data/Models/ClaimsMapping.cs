using ClemBot.Api.Data.Enums;

namespace ClemBot.Api.Data.Models
{
    public class ClaimsMapping
    {
        public ulong Id { get; set; }

        public Claims Claim { get; set; }

        public ulong RoleId { get; set; }
        public Role Role { get; set; }
    }
}
