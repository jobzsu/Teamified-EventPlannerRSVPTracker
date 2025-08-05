using System.ComponentModel.DataAnnotations.Schema;

namespace EventPlannerRSVPTracker.Domain.Models;

public class Event
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public string Location { get; set; }

    public string? Description { get; set; }

    public int MaxPax { get; set; }

    public Guid CreatedByUserId { get; set; }

    public DateOnly DateAdded { get; set; }

    public byte[] RowVersion { get; set; }

    [NotMapped]
    public virtual User Host { get; set; }

    public static Event Create(string name, 
        DateOnly date, 
        TimeOnly time,
        string location,
        string? description,
        int maxPax,
        Guid createdByUserId)
    {
        return new Event()
        {
            Name = name,
            Date = date,
            Time = time,
            Location = location,
            Description = description,
            MaxPax = maxPax,
            CreatedByUserId = createdByUserId,
            DateAdded = DateOnly.FromDateTime(DateTime.UtcNow),
            RowVersion = Array.Empty<byte>()
        };
    }
}
