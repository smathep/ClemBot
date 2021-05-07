namespace ClemBot.Api.Data.Models
{
    public class Role
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public bool? IsAssignable { get; set; }

        public ulong GuildId { get; set; }
        public Guild Guild { get; set; }
    }
}
