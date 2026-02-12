using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Models
{
    public class CalendarEvent
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("startAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime StartAt { get; set; }

        [BsonElement("endAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime EndAt { get; set; }

        [BsonElement("color")]
        public string Color { get; set; } = "#3b82f6";

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
