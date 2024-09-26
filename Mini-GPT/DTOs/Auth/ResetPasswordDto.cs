using System.ComponentModel.DataAnnotations;

namespace Mini_GPT.DTOs.Account
{
    public class ResetPasswordDto
    {

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        public string? ConfirmPassword { get; set; }


        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
