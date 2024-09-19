using Mini_GPT.DTOs.Chat;
using Mini_GPT.DTOs.Messages;
using Mini_GPT.Models;

namespace Mini_GPT.Interfaces
{
    public interface IChatService
    {
        Task<Chat> CreateChatAsync(string prompt);
        Task<Message> SendPromptAsync(string chatId, string prompt);
        Task<Message> EditPromptAsync(string chatId, Message message, string prompt);
        Task<bool> DeleteChatAsync(string chatId);
        Task<Chat> GetChatAsync(string chatId);
    }
}
