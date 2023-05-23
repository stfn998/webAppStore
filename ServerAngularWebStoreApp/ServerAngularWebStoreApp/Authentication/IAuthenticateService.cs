using Common.DTOs;
using Common.Models;

namespace ServerAngularWebStoreApp.Authentication
{
    public interface IAuthenticateService
    {
        Task<AuthenticateResponseDTO> Authenticate(AuthenticateRequestDTO model);

        Task<User> GetById(int id);
    }
}