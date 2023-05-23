using System.ComponentModel.DataAnnotations;

namespace Common.DTOs
{
    public class AuthenticateRequestDTO
    {
        [MinLength(4, ErrorMessage = "Username should have more than 4 characters.")]
        public string Username { get; set; }

        [MinLength(4, ErrorMessage = "Password should have more than 4 characters.")]
        public string Password { get; set; }
    }
}