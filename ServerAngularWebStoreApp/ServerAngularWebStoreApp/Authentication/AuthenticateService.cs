using Common.DTOs;
using Common.Models;
using DataAcceess.IRepository;
using Microsoft.Extensions.Options;
using ServerAngularWebStoreApp.Authorization;
using Services.PasswordHasher;

namespace ServerAngularWebStoreApp.Authentication
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly AppSettings _appSettings;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<User> _genericRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IJwtUtils _jwtUtils;

        public AuthenticateService(IGenericRepository<User> genericRepository, IOptions<AppSettings> appSettings, IPersonRepository personRepository, IUserRepository userRepository, IJwtUtils jwtUtils)
        {
            _genericRepository = genericRepository;
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _personRepository = personRepository;
            _jwtUtils = jwtUtils;
        }

        public async Task<AuthenticateResponseDTO> Authenticate(AuthenticateRequestDTO model)
        {
            PasswordHasher hasher = new PasswordHasher();
            User user = await _userRepository.GetUserByUsername(model.Username);
            if (user == null)
            {
                return new AuthenticateResponseDTO("");
            }
            Person person = await _personRepository.GetPersonByUserId(user.Id);
            if (!String.IsNullOrEmpty(model.Password))
            {
                var checkedPasswordMatch = hasher.Check(user.Password, model.Password);
                // return null if user not found
                if (!checkedPasswordMatch.Verified)
                {
                    return new AuthenticateResponseDTO(""); ;
                }
            }
            // authentication successful so generate jwt token
            user.Password = "";
            var token = _jwtUtils.GenerateJwtToken(user);
            return new AuthenticateResponseDTO(user, person.Verification.ToString(), person.Id, token, person.PersonType.ToString());
        }

        public async Task<User> GetById(int id)
        {
            return await _genericRepository.GetByObject(id);
        }
    }
}