using EventPlannerRSVPTracker.App.DTOs;

namespace EventPlannerRSVPTracker.App.Abstractions.Services;

public interface IEventsService
{
    Task<ResultModel<Guid>> CreateEvent(CreateEventDTO newEvent, CancellationToken cancellationToken = default);

    Task<ResultModel<List<UserEventDTO>>> GetUserEvents(string username, CancellationToken cancellationToken = default);

    Task<ResultModel<bool>> UpdateEvent(UpdateEventDTO updatedEvent, CancellationToken cancellationToken = default);

    Task<ResultModel<EventDTO>> GetEventById(Guid eventId, CancellationToken cancellationToken = default);

    Task<ResultModel<bool>> DeleteEvent(Guid eventId, CancellationToken cancellationToken = default);

    Task<ResultModel<bool>> RSVPToEvent(Guid eventId, CancellationToken cancellationToken = default);
}
