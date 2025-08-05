using EventPlannerRSVPTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventPlannerRSVPTracker.Database.ModelConfigurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");

        builder.HasKey(e => e.Id)
            .IsClustered();
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .IsRequired();


        builder.Property(e => e.Date)
            .IsRequired();

        builder.Property(e => e.Time)
            .IsRequired();

        builder.Property(e => e.Location)
            .IsRequired();

        builder.Property(e => e.CreatedByUserId)
            .IsRequired();

        builder.Property(e => e.DateAdded)
            .IsRequired();

        builder.Property(e => e.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();

        builder.HasOne(e => e.Host)
            .WithMany(u => u.EventsCreated)
            .HasForeignKey(e => e.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
