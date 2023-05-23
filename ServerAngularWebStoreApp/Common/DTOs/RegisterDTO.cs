using System.ComponentModel.DataAnnotations;

namespace Common.DTOs
{
    public class RegisterDTO
    {
        [StringLength(255, ErrorMessage = "Username length can't be more than 255.")]
        [MinLength(4, ErrorMessage = "Username should have more than 4 characters.")]
        public string UserName { get; set; }

        public string Email { get; set; }

        [MinLength(4, ErrorMessage = "Password should have more than 4 characters.")]
        public string Password { get; set; }

        [StringLength(255, ErrorMessage = "First name length can't be more than 255.")]
        public string FirstName { get; set; }

        [StringLength(255, ErrorMessage = "Last name length can't be more than 255.")]
        public string LastName { get; set; }

        public string Birth { get; set; }

        [StringLength(500, ErrorMessage = "Address length can't be more than 500.")]
        public string Address { get; set; }

        public string ImageUrl { get; set; }
    }
}