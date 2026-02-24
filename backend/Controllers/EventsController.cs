using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Backend.Models;
using Backend.Services;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        private readonly IEventsService _eventsService;

        public EventsController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarEvent>>> Get(
            [FromQuery] DateTime? start_date, 
            [FromQuery] DateTime? end_date)
        {
            var userId = GetUserId();
            var events = await _eventsService.GetEventsAsync(userId, start_date, end_date);
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CalendarEvent>> GetById(string id)
        {
            var userId = GetUserId();
            var eventFound = await _eventsService.GetEventByIdAsync(id, userId);

            if (eventFound == null)
            {
                return NotFound();
            }

            return Ok(eventFound);
        }

        [HttpPost]
        public async Task<ActionResult<CalendarEvent>> Post([FromBody] CalendarEvent calendarEvent)
        {
            calendarEvent.UserId = GetUserId();
            try
            {
                var createdEvent = await _eventsService.CreateEventAsync(calendarEvent);
                return CreatedAtAction(nameof(GetById), new { id = createdEvent.Id }, createdEvent);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] CalendarEvent updatedEvent)
        {
            var userId = GetUserId();
            try
            {
                await _eventsService.UpdateEventAsync(id, userId, updatedEvent);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = GetUserId();
            var deleted = await _eventsService.DeleteEventAsync(id, userId);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
