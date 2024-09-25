using Mini_GPT.DTOs.Chat;
using Mini_GPT.Models;

namespace Mini_GPT.Mappers
{
    public static class ChatMapper
    {
        public static ChatDto ToChatDto(this Chat chat)
        {
            return new ChatDto
            {
                Messages = chat.Messages.Select(m => m.ToMessageDto()).ToList(),
            };
        }
    }
}