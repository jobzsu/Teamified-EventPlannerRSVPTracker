namespace EventPlannerRSVPTracker.App.DTOs;

public class UserEventDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Date { get; set; }

    public string Time { get; set; }

    public string Location { get; set; }

    public int ReservedPax { get; set; }

    public int MaxPax { get; set; }
}
