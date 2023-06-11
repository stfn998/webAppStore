using Common.DTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IPersonService
    {
        Task<PersonDTO> GetPersonById(int id);

        Task<PersonDTO> GetPersonByEmail(string email);

        Task<IEnumerable<PersonDTO>> GetSellersAndCustomers();

        Task<IEnumerable<PersonDTO>> GetAll();

        Task<bool> UpdatePerson(ProfileDTO dto);

        Task<bool> DeletePerson(int idPerson);

        Task UploadImage(int idPerson, IFormFile file);

        Task<string> GetImage(int idPerson);
    }
}