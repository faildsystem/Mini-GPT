using Mini_GPT.DTOs.Messages;
using Mini_GPT.Models;

namespace Mini_GPT.Mappers
{
    public static class MessageMapper
    {

        public static Message FromMessageDto(this MessageDto messageDto)
        {
            return new Message
            {
                MessageId = messageDto.MessageId,
                Prompt = messageDto.Prompt,
                Response = messageDto.Response
            };
        }
        public static MessageDto ToMessageDto(this Message message)
        {
            return new MessageDto
            {
                MessageId = message.MessageId,
                Prompt = message.Prompt,
                Response = message.Response,
            };
        }

    }
}