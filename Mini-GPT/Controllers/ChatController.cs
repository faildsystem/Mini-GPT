using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mini_GPT.DTOs.Chat;
using Mini_GPT.DTOs.Messages;
using Mini_GPT.Extensions;
using Mini_GPT.Interfaces;
using Mini_GPT.Mappers;
using Mini_GPT.Models;
using System.Security.Claims;


namespace Mini_GPT.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[Controller]")]
    public class ChatController : ControllerBase
    {
         private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;

        }

        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] string prompt)
        {
            // Get the user's ID from the claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //return Ok(userId);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found");
            }

            // Create a new chat with the authenticated user's ID
            var newChat = await _chatService.CreateChatAsync(prompt, userId);

            return Ok(newChat);

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
        public async Task<IActionResult> SendPrompt([FromRoute] string chatId, [FromBody] string prompt)
        {
            var sentMessage = await _chatService.SendPromptAsync(chatId, prompt);

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

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUserChats()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var chats = await _chatService.GetAllUserChats(userId);
            return Ok(chats);
        }
    

    }
}
