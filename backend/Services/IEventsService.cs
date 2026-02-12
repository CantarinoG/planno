using Backend.Models;

namespace Backend.Services
{
    public interface IEventsService
    {
        Task<List<CalendarEvent>> GetEventsAsync(DateTime? startDate, DateTime? endDate);
        Task<CalendarEvent?> GetEventByIdAsync(string id);
        Task<CalendarEvent> CreateEventAsync(CalendarEvent calendarEvent);
        Task UpdateEventAsync(string id, CalendarEvent updatedEvent);
        Task<bool> DeleteEventAsync(string id);
    }
}
