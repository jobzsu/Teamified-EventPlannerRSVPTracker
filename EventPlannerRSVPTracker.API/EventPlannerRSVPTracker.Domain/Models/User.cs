

using System.ComponentModel.DataAnnotations.Schema;

namespace EventPlannerRSVPTracker.Domain.Models;

public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public byte[] RowVersion { get; set; }

    [NotMapped]
    public ICollection<Event> EventsCreated { get; set; }

    public static User Create(string username)
    {
        return new User()
        {
            Username = username,
            LastLoginDate = null,
            RowVersion = Array.Empty<byte>()
        };
    }
}
