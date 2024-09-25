using Mini_GPT.Interfaces;
using MongoDB.Bson.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mini_GPT.Services
{
    public class LlmService : ILlmService
    {
        private static readonly string ApiUrl = "https://api-inference.huggingface.co/models/google/gemma-7b";
        private static readonly string ApiKey = "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"; // Replace with your Hugging Face API key
        private readonly HttpClient _httpClient;

        public LlmService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
        }
        public async Task<string> GetLlmResponseAsync(string prompt)
        {
            // Use System.Text.Json to serialize the object
            var payload = new { inputs = prompt };
            var jsonPayload = JsonSerializer.Serialize(payload); // Use System.Text.Json

            // Create the content with the correct content type
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Send POST request to the API
            var response = await _httpClient.PostAsync(ApiUrl, content);

            // Check if response is successful
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to call API. Status code: {response.StatusCode}");
            }

            // Read the response data as a string
            var responseData = await response.Content.ReadAsStringAsync();

            // Deserialize the response to match the format returned by the API
            var result = JsonSerializer.Deserialize<List<GeneratedTextResponse>>(responseData);

            // Return the generated text from the response
            return result?.FirstOrDefault()?.GeneratedText ?? "No response received";
        }

        // Define a class to deserialize the response
       

    }
    public class GeneratedTextResponse
    {
        [JsonPropertyName("generated_text")]
        public string GeneratedText { get; set; }
    }
}
