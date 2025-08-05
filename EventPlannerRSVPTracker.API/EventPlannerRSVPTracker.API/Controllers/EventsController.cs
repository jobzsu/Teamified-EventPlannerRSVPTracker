using EventPlannerRSVPTracker.App.Abstractions.Services;
using EventPlannerRSVPTracker.App.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EventPlannerRSVPTracker.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private readonly IEventsService _eventsService;

    public EventsController(ILogger<EventsController> logger, IEventsService eventsService)
    {
        _logger = logger;
        _eventsService = eventsService;
    }

    [HttpGet]
    [Route("UserEvents/{username}")]
    public async Task<IActionResult> GetEventsByUser(string username, CancellationToken cancellationToken = default)
    {
        var result = await _eventsService.GetUserEvents(username, cancellationToken);

        if (result.IsSuccess)
        {
            var jsonResponse = JsonResponse.Success("Events retrieved successfully", result.Data);

            return Ok(jsonResponse);
        }
        else
        {
            var jsonResponse = JsonResponse.Fail(result.ErrorDetails![0], result.ErrorDetails![0].Message);

            return Ok(jsonResponse);
        }
    }

    [HttpGet]
    [Route("{eventId}")]
    public async Task<IActionResult> GetEventById(Guid eventId, CancellationToken cancellationToken = default)
    {
        var result = await _eventsService.GetEventById(eventId, cancellationToken);
        if (result.IsSuccess)
        {
            var jsonResponse = JsonResponse.Success("Event retrieved successfully", result.Data);

            return Ok(jsonResponse);
        }
        else
        {
            var jsonResponse = JsonResponse.Fail(result.ErrorDetails![0], result.ErrorDetails![0].Message);

            return Ok(jsonResponse);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO newEvent, CancellationToken cancellationToken = default)
    {
        var result = await _eventsService.CreateEvent(newEvent, cancellationToken);

        if(result.IsSuccess)
        {
            var jsonResponse = JsonResponse.Success("Event created successfully", result.Data);

            return Ok(jsonResponse);
        }
        else
        {
            var jsonResponse = JsonResponse.Fail(result.ErrorDetails![0], result.ErrorDetails![0].Message);

            return Ok(jsonResponse);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventDTO updatedEvent, CancellationToken cancellationToken = default)
    {
        var result = await _eventsService.UpdateEvent(updatedEvent, cancellationToken);

        if (result.IsSuccess)
        {
            var jsonResponse = JsonResponse.Success("Event updated successfully", result.Data);

            return Ok(jsonResponse);
        }
        else
        {
            var jsonResponse = JsonResponse.Fail(result.ErrorDetails![0], result.ErrorDetails![0].Message);

            return Ok(jsonResponse);
        }
    }

    [HttpDelete]
    [Route("{eventId}")]
    public async Task<IActionResult> DeleteEvent(Guid eventId, CancellationToken cancellationToken = default)
    {
        var result = await _eventsService.DeleteEvent(eventId, cancellationToken);

        if (result.IsSuccess)
        {
            var jsonResponse = JsonResponse.Success("Event deleted successfully", result.Data);

            return Ok(jsonResponse);
        }
        else
        {
            var jsonResponse = JsonResponse.Fail(result.ErrorDetails![0], result.ErrorDetails![0].Message);

            return Ok(jsonResponse);
        }
    }

    [HttpPost]
    [Route("RSVP/{eventId}")]
    public async Task<IActionResult> RSVPToEvent(Guid eventId, CancellationToken cancellationToken = default)
    {
        var result = await _eventsService.RSVPToEvent(eventId, cancellationToken);

        if (result.IsSuccess)
        {
            var jsonResponse = JsonResponse.Success("RSVP successful", result.Data);

            return Ok(jsonResponse);
        }
        else
        {
            var jsonResponse = JsonResponse.Fail(result.ErrorDetails![0], result.ErrorDetails![0].Message);

            return Ok(jsonResponse);
        }
    }

    [HttpGet]
    [Route("Public")]
    public async Task<IActionResult> GetPublicEvents(CancellationToken cancellationToken = default)
    {
        var result = await _eventsService.GetPublicEvents(cancellationToken);

        if (result.IsSuccess)
        {
            var jsonResponse = JsonResponse.Success("Public events retrieved successfully", result.Data);

            return Ok(jsonResponse);
        }
        else
        {
            var jsonResponse = JsonResponse.Fail(result.ErrorDetails![0], result.ErrorDetails![0].Message);

            return Ok(jsonResponse);
        }
    }
}
