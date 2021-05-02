using ClemBot.Api.Data.Enums;

namespace ClemBot.Api.Data.Models
{
    public class ClaimsMapping
    {
        public int Id { get; set; }

        public Claims Claim { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
