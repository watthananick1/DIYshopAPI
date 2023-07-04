using System.ComponentModel.DataAnnotations;

namespace DIYshopAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required] 
        public string Lastname { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Pos;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? Proflieimg { get; set; }

    }
}
