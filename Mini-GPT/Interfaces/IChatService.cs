
using Mini_GPT.Models;

namespace Mini_GPT.Interfaces
{
    public interface IChatService
    {
        Task<Chat> CreateChatAsync(string prompt, string userId);
        Task<Message> SendPromptAsync(string chatId, string prompt);
        Task<Message> EditPromptAsync(string chatId, Message message, string prompt);
        Task<bool> DeleteChatAsync(string chatId);
        Task<Chat> GetChatAsync(string chatId);
        Task<List<Chat>> GetAllUserChats(string userId);
    }
}
