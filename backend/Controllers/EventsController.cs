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
    }
}
