﻿using Microsoft.AspNetCore.Mvc;
using NServiceBus.Saga.Demo.Contracts.Trips;

namespace NServiceBus.Saga.Demo.TripService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripController : ControllerBase
{
    private readonly IMessageSession _messageSession;

    public TripController(IMessageSession messageSession)
    {
        _messageSession = messageSession;
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Book([FromQuery] int requiredStars, [FromQuery] string destination,
        [FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var tripId = Guid.NewGuid();
        var message = new SubmitTrip
        {
            TripId = tripId,
            RequiredStars = requiredStars,
            Destination = destination,
            Start = start,
            End = end
        };
        var response = await _messageSession.Request<TripSubmissionResponse>(message);
        if (response is { Succeeded: true })
        {
            return AcceptedAtRoute("GetTripState", new { Id = tripId }, new { Id = tripId });
        }
        return BadRequest(new { response.Reason });
    }

    [HttpGet("{id}", Name = "GetTripState")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var request = new TripStateRequest
        {
            TripId = id
        };
        var trip = await _messageSession.Request<TripState>(request);

        if (trip.TripId != Guid.Empty)
        {
            return Ok(trip);
        }
        return NotFound(new TripNotFound(){ TripId = id});
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Cancel([FromRoute] Guid id, [FromQuery] string reason)
    {
        await _messageSession.Send(new TripCancellationRequest
        {
            TripId = id,
            Reason = reason
        });

        return Accepted();
    }
}