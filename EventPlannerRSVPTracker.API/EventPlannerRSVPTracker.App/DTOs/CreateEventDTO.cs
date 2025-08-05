namespace EventPlannerRSVPTracker.App.DTOs;

public class CreateEventDTO
{
    public string Name { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public string Location { get; set; }

    public string? Description { get; set; }

    public int MaxPax { get; set; }

    public string Host { get; set; }
}
