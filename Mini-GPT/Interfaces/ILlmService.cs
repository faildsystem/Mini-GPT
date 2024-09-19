namespace Mini_GPT.Interfaces
{
    public interface ILlmService
    {
        Task<string> GetLlmResponseAsync(string prompt);
    }
}
