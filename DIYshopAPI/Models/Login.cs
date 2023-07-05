using System.ComponentModel.DataAnnotations;

namespace DIYshopAPI.Models
{
    public class Login
    {
        public required string UserName { get; set; }

        public required string Password { get; set; }
    }
}
