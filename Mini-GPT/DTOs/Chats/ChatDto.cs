﻿using Mini_GPT.DTOs.Messages;
using System.ComponentModel.DataAnnotations;

namespace Mini_GPT.DTOs.Chat
{
    public class ChatDto
    {
        [Required]
        public List<MessageDto>? Messages { get; set; }

    }
}

