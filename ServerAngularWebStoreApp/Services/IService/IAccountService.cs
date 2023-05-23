using Common.DTOs;
using Common.Models;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IAccountService
    {
        Task<bool> Register(RegisterDTO dto, Enums.PersonType type);

        Task<PersonDTO> GetPerson(string email);
    }
}