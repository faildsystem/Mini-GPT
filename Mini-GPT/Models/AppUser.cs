using Microsoft.AspNetCore.Identity;

namespace Mini_GPT.Models
{
    public class AppUser : IdentityUser
    {
        public string? ProfileImage { get; set; }

    }

}
