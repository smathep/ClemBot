using System;
using ClemBot.Api.Data.Enums;

namespace ClemBot.Api.Data.Models
{
    public class Infraction
    {
        public int Id { get; set; }
        
        public InfractionType Type { get; set; }
        
        public string? Reason { get; set; }
        
        public bool? IsActive { get; set; }
        
        public int? Duration { get; set; }
        
        public DateTime Time {get; set; }
        
        public int GuildId { get; set; }
        public Guild Guild { get; set; }
        
        public int AuthorId { get; set; }
        public User Author { get; set; }
        
        public int SubjectId { get; set; }
        public User Subject { get; set; }
    }
}