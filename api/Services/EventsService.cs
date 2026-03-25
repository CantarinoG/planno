using Backend.Models;
using MongoDB.Driver;
using Confluent.Kafka;
using System.Text.Json;

namespace Backend.Services
{
    public class EventsService : IEventsService
    {
        private readonly IMongoCollection<CalendarEvent> _events;
        private readonly IProducer<string, string> _kafkaProducer;
        private readonly string _eventsTopic;

        public EventsService(IMongoDatabase database, IProducer<string, string> kafkaProducer, IConfiguration configuration)
        {
            _events = database.GetCollection<CalendarEvent>("Events");
            _kafkaProducer = kafkaProducer;
            _eventsTopic = configuration["KafkaSettings:EventsTopic"] ?? "calendar-events";
        }

        private async Task PublishKafkaEventAsync(string eventId, string userId, string title, EventAction action)
        {
            var payload = JsonSerializer.Serialize(new { 
                EventId = eventId, 
                Title = title, 
                Action = action.ToString() 
            });

            await _kafkaProducer.ProduceAsync(_eventsTopic, new Message<string, string> { 
                Key = userId, 
                Value = payload 
            });
        }

        public async Task<List<CalendarEvent>> GetEventsAsync(string userId, DateTime? startDate, DateTime? endDate)
        {
            FilterDefinition<CalendarEvent> filter = Builders<CalendarEvent>.Filter.Eq(e => e.UserId, userId);

            if (startDate.HasValue && endDate.HasValue)
            {
                var start = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);
                var end = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);
                filter = Builders<CalendarEvent>.Filter.And(
                    filter,
                    Builders<CalendarEvent>.Filter.Gte(e => e.StartAt, start),
                    Builders<CalendarEvent>.Filter.Lte(e => e.StartAt, end)
                );
            }
            else if (startDate.HasValue)
            {
                var start = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);
                filter = Builders<CalendarEvent>.Filter.And(filter, Builders<CalendarEvent>.Filter.Gte(e => e.StartAt, start));
            }
            else if (endDate.HasValue)
            {
                var end = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);
                filter = Builders<CalendarEvent>.Filter.And(filter, Builders<CalendarEvent>.Filter.Lte(e => e.StartAt, end));
            }

            return await _events.Find(filter).ToListAsync();
        }

        public async Task<CalendarEvent?> GetEventByIdAsync(string id, string userId)
        {
            return await _events.Find(e => e.Id == id && e.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<CalendarEvent> CreateEventAsync(CalendarEvent calendarEvent)
        {
            ValidateEvent(calendarEvent);

            await _events.InsertOneAsync(calendarEvent);
            await PublishKafkaEventAsync(calendarEvent.Id, calendarEvent.UserId, calendarEvent.Title, EventAction.CREATED);

            return calendarEvent;
        }

        public async Task UpdateEventAsync(string id, string userId, CalendarEvent updatedEvent)
        {
            ValidateEvent(updatedEvent);

            var eventFound = await _events.Find(e => e.Id == id && e.UserId == userId).FirstOrDefaultAsync();
            if (eventFound == null)
            {
                throw new KeyNotFoundException("Event not found.");
            }

            updatedEvent.Id = id;
            updatedEvent.UserId = userId;
            updatedEvent.UpdatedAt = DateTime.UtcNow;
            await _events.ReplaceOneAsync(e => e.Id == id && e.UserId == userId, updatedEvent);

            await PublishKafkaEventAsync(id, userId, updatedEvent.Title, EventAction.UPDATED);
        }

        public async Task<bool> DeleteEventAsync(string id, string userId)
        {
            var eventFound = await _events.Find(e => e.Id == id && e.UserId == userId).FirstOrDefaultAsync();
            
            var result = await _events.DeleteOneAsync(e => e.Id == id && e.UserId == userId);
            
            if (result.DeletedCount > 0 && eventFound != null)
            {
                await PublishKafkaEventAsync(id, userId, eventFound.Title, EventAction.DELETED);
            }
            
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
