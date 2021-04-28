namespace ClemBot.Api.Data.Models
{
    public class Role
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public bool IsAssignable { get; set; }

        public int GuildId { get; set; }
        public Guild Guild { get; set; }
    }
}