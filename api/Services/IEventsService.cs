using Backend.Models;

namespace Backend.Services
{
    public interface IEventsService
    {
        Task<List<CalendarEvent>> GetEventsAsync(string userId, DateTime? startDate, DateTime? endDate);
        Task<CalendarEvent?> GetEventByIdAsync(string id, string userId);
        Task<CalendarEvent> CreateEventAsync(CalendarEvent calendarEvent);
        Task UpdateEventAsync(string id, string userId, CalendarEvent updatedEvent);
        Task<bool> DeleteEventAsync(string id, string userId);
    }
}
