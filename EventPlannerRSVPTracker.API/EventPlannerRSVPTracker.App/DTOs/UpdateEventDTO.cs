namespace EventPlannerRSVPTracker.App.DTOs;

public class UpdateEventDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public string Location { get; set; }

    public string? Description { get; set; }

    public int ReservedPax { get; set; }

    public int MaxPax { get; set; }
}
