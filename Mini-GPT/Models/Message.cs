using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

using System.ComponentModel.DataAnnotations;

namespace Mini_GPT.Models
{
    public class Message
    {
        [BsonId] // MongoDB will use this field as the unique identifier for the document
        [BsonRepresentation(BsonType.ObjectId)] // Ensures MongoDB treats this as an ObjectId
        public string? MessageId { get; set; } = ObjectId.GenerateNewId().ToString();
        [Required]
        public string? Prompt { get; set; }
        public string? Response { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
