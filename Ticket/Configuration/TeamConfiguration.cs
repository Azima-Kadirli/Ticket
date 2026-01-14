using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticket.Models;

namespace Ticket.Configuration
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.Property(t => t.FullName).IsRequired().HasMaxLength(255);
            builder.Property(t => t.Position).IsRequired().HasMaxLength(255);
            builder.Property(t => t.SocialMedia).IsRequired();
        }
    }
}
