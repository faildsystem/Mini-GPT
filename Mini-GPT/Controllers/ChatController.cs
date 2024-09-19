using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Mini_GPT.DTOs.Chat;
using Mini_GPT.DTOs.Messages;
using Mini_GPT.Interfaces;
using Mini_GPT.Mappers;
using Mini_GPT.Models;
using Mini_GPT.Services;
using MongoDB.Bson;

namespace Mini_GPT.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;


        public ChatController(IChatService chatService) {
            _chatService = chatService; 
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] string prompt)
        {
            var NewChat = await _chatService.CreateChatAsync(prompt);
            
            return Ok(NewChat);
        }


        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChat([FromRoute] string chatId)
        {
            var chat = await _chatService.GetChatAsync(chatId);
            
            if (chat == null)
            {
                return NotFound();
            }

            return Ok(chat.ToChatDto());
        }

        [HttpPost("{chatId}/prompt")]
        public async Task<IActionResult> SendPrompt([FromRoute] string chatId,[FromBody] string prompt)
        {
           var sentMessage =  await _chatService.SendPromptAsync(chatId, prompt);
            var responseMessage = sentMessage.ToMessageDto();
            return Ok(responseMessage);
        }

        [HttpDelete("{chatId}")]
        public async Task<IActionResult> DeleteChat([FromRoute] string chatId)
        {
            var isDeleted = await _chatService.DeleteChatAsync(chatId);
            if (!isDeleted)
            {
                return NotFound();
            }
            return Ok();

        }

        [HttpPut("{chatId}")]
        public async Task<IActionResult> UpdatePrompt(string chatId, UpdatePromptDto updatePromptDto)
        {
            var message = await _chatService.EditPromptAsync(chatId, updatePromptDto.Message, updatePromptDto.Prompt);
            var responseMessage = message.ToMessageDto();
            return Ok(responseMessage);

        }


    }
}
