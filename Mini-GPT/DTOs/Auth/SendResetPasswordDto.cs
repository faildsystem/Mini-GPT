using System.ComponentModel.DataAnnotations;

namespace Mini_GPT.DTOs.Account
{
    public class SendResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        //[Required]
        //public string? ClientURI { get; set; }

    }
}
