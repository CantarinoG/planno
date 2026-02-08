using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IMongoCollection<CalendarEvent> _events;

        public EventsController(IMongoDatabase database)
        {
            _events = database.GetCollection<CalendarEvent>("Events");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarEvent>>> Get(
            [FromQuery] DateTime? start_date, 
            [FromQuery] DateTime? end_date)
        {
            var filter = Builders<CalendarEvent>.Filter.Empty;

            if (start_date.HasValue && end_date.HasValue)
            {
                filter = Builders<CalendarEvent>.Filter.And(
                    Builders<CalendarEvent>.Filter.Gte(e => e.StartAt, start_date.Value),
                    Builders<CalendarEvent>.Filter.Lte(e => e.StartAt, end_date.Value)
                );
            }
            else if (start_date.HasValue)
            {
                filter = Builders<CalendarEvent>.Filter.Gte(e => e.StartAt, start_date.Value);
            }
            else if (end_date.HasValue)
            {
                filter = Builders<CalendarEvent>.Filter.Lte(e => e.StartAt, end_date.Value);
            }

            var events = await _events.Find(filter).ToListAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CalendarEvent>> GetById(string id)
        {
            var eventFound = await _events.Find(e => e.Id == id).FirstOrDefaultAsync();

            if (eventFound == null)
            {
                return NotFound();
            }

            return Ok(eventFound);
        }

        [HttpPost]
        public async Task<ActionResult<CalendarEvent>> Post([FromBody] CalendarEvent calendarEvent)
        {
            await _events.InsertOneAsync(calendarEvent);
            return CreatedAtAction(nameof(GetById), new { id = calendarEvent.Id }, calendarEvent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] CalendarEvent updatedEvent)
        {
            var eventFound = await _events.Find(e => e.Id == id).FirstOrDefaultAsync();

            if (eventFound == null)
            {
                return NotFound();
            }

            updatedEvent.Id = eventFound.Id;
            updatedEvent.UpdatedAt = DateTime.UtcNow;

            await _events.ReplaceOneAsync(e => e.Id == id, updatedEvent);

            return NoContent();
        }
    }
}
