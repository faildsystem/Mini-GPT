using Microsoft.Identity.Client;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Mini_GPT.Models
{
    public class Chat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ChatId { get; set; }
        [Required]
        public string? UserId { get; set; }
        [Required]
        public List<Message>? Messages { get; set; }
    }


}


