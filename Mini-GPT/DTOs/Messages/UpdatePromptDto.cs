using Mini_GPT.Models;

namespace Mini_GPT.DTOs.Messages
{
    public class UpdatePromptDto
    {
        public Message Message { get; set; }
        public string Prompt { get; set; }
    }
}
