using EventPlannerRSVPTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventPlannerRSVPTracker.Database.ModelConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder
            .HasKey(u => u.Id)
            .IsClustered();
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.HasIndex(u => u.Username)
            .IsUnique();
        builder.Property(u => u.Username)
            .IsRequired();

        builder.Property(u => u.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();

        builder.HasMany(u => u.EventsCreated)
            .WithOne(e => e.Host)
            .HasForeignKey(e => e.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
