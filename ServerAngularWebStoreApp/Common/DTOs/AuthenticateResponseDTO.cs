using Common.Models;

namespace Common.DTOs
{
    public class AuthenticateResponseDTO
    {
        public int Id { get; set; }
        public int IdPerson { get; set; }
        public string Activate { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }

        public AuthenticateResponseDTO(User user, string activate, int idPerson, string token, string role)
        {
            Id = user.Id;
            IdPerson = idPerson;
            Username = user.UserName;
            Token = token;
            Activate = activate;
            Role = role;
        }

        public AuthenticateResponseDTO(string token)
        {
            Token = token;
            Username = "";
        }
    }
}