using ClemBot.Api.Data.Enums;
using ClemBot.Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ClemBot.Api.Data.Contexts
{
    public class ClemBotContext : DbContext
    {
        public ClemBotContext(DbContextOptions<ClemBotContext> options)
            : base(options)
        {
        }
        
        static ClemBotContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Claims>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<DesignatedChannels>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Infractions>();
        }

        public DbSet<Channel> Channel { get; set; }
        public DbSet<ClaimsMapping> ClaimsMapping { get; set; }
        public DbSet<CustomPrefix> CustomPrefix { get; set; }
        public DbSet<DesignatedChannelMapping> DesignatedChannelMapping { get; set; }
        public DbSet<Guild> Guild { get; set; }
        public DbSet<Infraction> Infraction { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Reminder> Reminder { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<TagUse> TagUse { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .Property(p => p.IsAssignable)
                .HasDefaultValue(true);

            modelBuilder.HasPostgresEnum<Claims>();
            modelBuilder.HasPostgresEnum<DesignatedChannels>();
            modelBuilder.HasPostgresEnum<Infractions>();
        }
    }
}