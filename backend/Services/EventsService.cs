using Backend.Models;
using MongoDB.Driver;

namespace Backend.Services
{
    public class EventsService : IEventsService
    {
        private readonly IMongoCollection<CalendarEvent> _events;

        public EventsService(IMongoDatabase database)
        {
            _events = database.GetCollection<CalendarEvent>("Events");
        }

        public async Task<List<CalendarEvent>> GetEventsAsync(DateTime? startDate, DateTime? endDate)
        {
            FilterDefinition<CalendarEvent> filter = Builders<CalendarEvent>.Filter.Empty;

            if (startDate.HasValue && endDate.HasValue)
            {
                var start = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);
                var end = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);
                filter = Builders<CalendarEvent>.Filter.And(
                    Builders<CalendarEvent>.Filter.Gte(e => e.StartAt, start),
                    Builders<CalendarEvent>.Filter.Lte(e => e.StartAt, end)
                );
            }
            else if (startDate.HasValue)
            {
                var start = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);
                filter = Builders<CalendarEvent>.Filter.Gte(e => e.StartAt, start);
            }
            else if (endDate.HasValue)
            {
                var end = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);
                filter = Builders<CalendarEvent>.Filter.Lte(e => e.StartAt, end);
            }

            return await _events.Find(filter).ToListAsync();
        }

        public async Task<CalendarEvent?> GetEventByIdAsync(string id)
        {
            return await _events.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<CalendarEvent> CreateEventAsync(CalendarEvent calendarEvent)
        {
            ValidateEvent(calendarEvent);

            await _events.InsertOneAsync(calendarEvent);
            return calendarEvent;
        }

        public async Task UpdateEventAsync(string id, CalendarEvent updatedEvent)
        {
            ValidateEvent(updatedEvent);

            var eventFound = await _events.Find(e => e.Id == id).FirstOrDefaultAsync();
            if (eventFound == null)
            {
                throw new KeyNotFoundException("Event not found.");
            }

            updatedEvent.Id = id;
            updatedEvent.UpdatedAt = DateTime.UtcNow;
            await _events.ReplaceOneAsync(e => e.Id == id, updatedEvent);
        }

        public async Task<bool> DeleteEventAsync(string id)
        {
            var result = await _events.DeleteOneAsync(e => e.Id == id);
            return result.DeletedCount > 0;
        }

        private void ValidateEvent(CalendarEvent calendarEvent)
        {
            if (string.IsNullOrWhiteSpace(calendarEvent.Title))
            {
                throw new ArgumentException("Title is required.");
            }

            if (string.IsNullOrWhiteSpace(calendarEvent.Color))
            {
                throw new ArgumentException("Color is required.");
            }

            if (calendarEvent.StartAt == default || calendarEvent.EndAt == default)
            {
                throw new ArgumentException("Start and end dates are required.");
            }

            if (calendarEvent.EndAt <= calendarEvent.StartAt)
            {
                throw new ArgumentException("Event must end after it starts.");
            }
        }
    }
}
