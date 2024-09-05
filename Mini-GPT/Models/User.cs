namespace Mini_GPT.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ProfileImage { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? LastLogin { get; set; }

        public User()
        {
            CreatedAt = DateTime.UtcNow;  
        }
    }

}
