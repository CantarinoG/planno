using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
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
            var events = await _eventsService.GetEventsAsync(start_date, end_date);
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CalendarEvent>> GetById(string id)
        {
            var eventFound = await _eventsService.GetEventByIdAsync(id);

            if (eventFound == null)
            {
                return NotFound();
            }

            return Ok(eventFound);
        }

        [HttpPost]
        public async Task<ActionResult<CalendarEvent>> Post([FromBody] CalendarEvent calendarEvent)
        {
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
            try
            {
                await _eventsService.UpdateEventAsync(id, updatedEvent);
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
            var deleted = await _eventsService.DeleteEventAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
