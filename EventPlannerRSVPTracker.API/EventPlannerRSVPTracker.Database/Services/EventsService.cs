using EventPlannerRSVPTracker.App.Abstractions.Persistence;
using EventPlannerRSVPTracker.App.Abstractions.Repositories;
using EventPlannerRSVPTracker.App.Abstractions.Services;
using EventPlannerRSVPTracker.App.DTOs;
using EventPlannerRSVPTracker.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EventPlannerRSVPTracker.Database.Services;

public class EventsService : IEventsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EventsService> _logger;
    private readonly IEventRepository _eventRepository;
    private readonly IUserRepository _userRepository;

    public EventsService(IUnitOfWork unitOfWork,
        ILogger<EventsService> logger,
        IEventRepository eventRepository,
        IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _eventRepository = eventRepository;
        _userRepository = userRepository;
    }

    public async Task<ResultModel<Guid>> CreateEvent(CreateEventDTO newEvent, CancellationToken cancellationToken = default)
    {
        List<ErrorModel> errors = new();

        try
        {
            if(string.IsNullOrWhiteSpace(newEvent.Host))
            {
                ErrorModel error = new(nameof(Exception), "Invalid Host");

                errors.Add(error);

                return ResultModel<Guid>.Fail(errors);
            }

            var cleansedUsername = newEvent.Host.Trim().ToLower();

            var user = await _userRepository.GetByUsername(cleansedUsername, cancellationToken);

            // If User does not exist, create a new User and then create the Event
            if (user is null)
            {
                var newUser = User.Create(cleansedUsername);

                using var txn = _unitOfWork.BeginTransaction();

                try
                {
                    newUser = await _userRepository.InsertAsync(newUser, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    var eventToInsert = Event.Create(newEvent.Name,
                                            newEvent.Date,
                                            newEvent.Time,
                                            newEvent.Location,
                                            newEvent.Description,
                                            newEvent.MaxPax,
                                            newUser.Id);

                    eventToInsert = await _eventRepository.InsertAsync(eventToInsert, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await txn.CommitAsync(cancellationToken);

                    return ResultModel<Guid>.Success(eventToInsert.Id);
                }
                catch (Exception)
                {
                    await txn.RollbackAsync(cancellationToken);

                    throw;
                }
            }
            // Else just create the Event
            else
            {
                using var txn = _unitOfWork.BeginTransaction();

                try
                {
                    var eventToInsert = Event.Create(newEvent.Name,
                                            newEvent.Date,
                                            newEvent.Time,
                                            newEvent.Location,
                                            newEvent.Description,
                                            newEvent.MaxPax,
                                            user.Id);

                    eventToInsert = await _eventRepository.InsertAsync(eventToInsert, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await txn.CommitAsync(cancellationToken);

                    return ResultModel<Guid>.Success(eventToInsert.Id);
                }
                catch (Exception)
                {
                    await txn.RollbackAsync(cancellationToken);

                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Creating Event");

            ErrorModel error = new(nameof(Exception), "Error Creating Event");

            errors.Add(error);

            return ResultModel<Guid>.Fail(errors);
        }
    }

    public async Task<ResultModel<bool>> DeleteEvent(Guid eventId, CancellationToken cancellationToken = default)
    {
        List<ErrorModel> errors = new();

        try
        {
            var existingEvent = await _eventRepository.GetEventById(eventId, cancellationToken, true);

            if(existingEvent is null)
                return ResultModel<bool>.Success(true);

            using var txn = _unitOfWork.BeginTransaction();

            try
            {
                await _eventRepository.DeleteAsync(existingEvent, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await txn.CommitAsync(cancellationToken);

                return ResultModel<bool>.Success(true);
            }
            catch (Exception)
            {
                await txn.RollbackAsync(cancellationToken);

                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Deleting Event");

            ErrorModel error = new(nameof(Exception), "Error Deleting Event");

            errors.Add(error);

            return ResultModel<bool>.Fail(errors);
        }
    }

    public async Task<ResultModel<EventDTO>> GetEventById(Guid eventId, CancellationToken cancellationToken = default)
    {
        List<ErrorModel> errors = new();

        try
        {
            var existingEvent = await _eventRepository.GetEventById(eventId, cancellationToken);

            if (existingEvent is null)
            {
                ErrorModel error = new(nameof(Exception), "Event not found");

                errors.Add(error);

                return ResultModel<EventDTO>.Fail(errors);
            }

            var retVal = new EventDTO()
            {
                Id = existingEvent.Id,
                Name = existingEvent.Name,
                Date = existingEvent.Date,
                Time = existingEvent.Time,
                Location = existingEvent.Location,
                Description = existingEvent.Description,
                ReservedPax = existingEvent.ReservedPax,
                MaxPax = existingEvent.MaxPax,
            };

            return ResultModel<EventDTO>.Success(retVal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Event");

            ErrorModel error = new(nameof(Exception), "Error retrieving Event");

            errors.Add(error);

            return ResultModel<EventDTO>.Fail(errors);
        }
    }

    public async Task<ResultModel<List<UserEventDTO>>> GetUserEvents(string username, CancellationToken cancellationToken = default)
    {
        List<ErrorModel> errors = new();

        try
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                ErrorModel error = new(nameof(Exception), "Invalid Host");

                errors.Add(error);

                return ResultModel<List<UserEventDTO>>.Fail(errors);
            }

            var cleansedUsername = username.Trim().ToLower();

            var events = await _eventRepository.GetUserEvents(cleansedUsername, cancellationToken);

            var retVal = new List<UserEventDTO>();

            if (events is not null && events.Any())
            {
                retVal.AddRange(events.Select(e =>
                {
                    return new UserEventDTO()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Date = e.Date.ToString("MM/dd/yyyy"),
                        Time = e.Time.ToString(),
                        Location = e.Location,
                        ReservedPax = e.ReservedPax,
                        MaxPax = e.MaxPax
                    };
                }));
            }

            return ResultModel<List<UserEventDTO>>.Success(retVal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Events");

            ErrorModel error = new(nameof(Exception), "Error retrieving Events");

            errors.Add(error);

            return ResultModel<List<UserEventDTO>>.Fail(errors);
        }
    }

    public async Task<ResultModel<bool>> RSVPToEvent(Guid eventId, CancellationToken cancellationToken = default)
    {
        List<ErrorModel> errors = new();

        try
        {
            var existingEvent = await _eventRepository.GetEventById(eventId, cancellationToken, true);

            if(existingEvent is null)
            {
                ErrorModel error = new(nameof(Exception), "Event not found");

                errors.Add(error);

                return ResultModel<bool>.Fail(errors);
            }

            if(existingEvent.ReservedPax >= existingEvent.MaxPax)
            {
                ErrorModel error = new(nameof(Exception), "Event is full, cannot RSVP");

                errors.Add(error);

                return ResultModel<bool>.Fail(errors);
            }

            existingEvent.ReservedPax++;

            using var txn = _unitOfWork.BeginTransaction();

            try
            {
                await _eventRepository.UpdateAsync(existingEvent, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await txn.CommitAsync(cancellationToken);

                return ResultModel<bool>.Success(true);
            }
            catch (Exception)
            {
                await txn.RollbackAsync(cancellationToken);

                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Updating Event RSVP");

            ErrorModel error = new(nameof(Exception), "Error Updating Event RSVP");

            errors.Add(error);

            return ResultModel<bool>.Fail(errors);
        }
    }

    public async Task<ResultModel<bool>> UpdateEvent(UpdateEventDTO updatedEvent, CancellationToken cancellationToken = default)
    {
        List<ErrorModel> errors = new();

        try
        {
            var eventToUpdate = await _eventRepository.GetEventById(updatedEvent.Id, cancellationToken, true);

            if(eventToUpdate is null)
            {
                ErrorModel error = new(nameof(Exception), "Event not found");

                errors.Add(error);

                return ResultModel<bool>.Fail(errors);
            }

            if(updatedEvent.ReservedPax > updatedEvent.MaxPax)
            {
                ErrorModel error = new(nameof(Exception), "Reserved Pax cannot be greater than Max Pax");

                errors.Add(error);

                return ResultModel<bool>.Fail(errors);
            }

            eventToUpdate.Update(updatedEvent.Name,
                updatedEvent.Date,
                updatedEvent.Time,
                updatedEvent.Location,
                updatedEvent.Description,
                updatedEvent.ReservedPax,
                updatedEvent.MaxPax);

            using var txn = _unitOfWork.BeginTransaction();

            try
            {
                var updatedEventFromDb = await _eventRepository.UpdateAsync(eventToUpdate, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await txn.CommitAsync(cancellationToken);

                return ResultModel<bool>.Success(true);
            }
            catch (Exception)
            {
                await txn.RollbackAsync(cancellationToken);

                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Updating Event");

            ErrorModel error = new(nameof(Exception), "Error Updating Event");

            errors.Add(error);

            return ResultModel<bool>.Fail(errors);
        }
    }
}
