using System.ComponentModel.DataAnnotations;

namespace Mini_GPT.DTOs.Messages
{
    public class MessageDto
    {
        public string? MessageId { get; set; }
        [Required]
        public string? Prompt { get; set; }
        public string? Response { get; set; }
    }
}

