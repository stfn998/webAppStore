using AutoMapper;
using Common.DTOs;
using Common.Models;
using DataAcceess.IRepository;
using Microsoft.AspNetCore.Http;
using Services.IService;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Service
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<User> _genericUserRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Person> _genericPersonRepository;
        private readonly IMapper _mapper;

        public AccountService(IGenericRepository<User> genericUserRepository, IUserRepository userRepository, IPersonRepository personRepository, IGenericRepository<Person> genericPersonRepository, IMapper mapper)
        {
            _genericUserRepository = genericUserRepository;
            _userRepository = userRepository;
            _personRepository = personRepository;
            _genericPersonRepository = genericPersonRepository;
            _mapper = mapper;
        }

        public Task<PersonDTO> GetPerson(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Register(RegisterDTO dto, Enums.PersonType type)
        {
            User user = await _userRepository.GetUserByUsername(dto.UserName);
            if (user != null)
            {
                return false;
            }

            user = new User
            {
                UserName = dto.UserName,
            };

            if (!String.IsNullOrEmpty(dto.Password))
            {
                PasswordHasher.PasswordHasher hasher = new PasswordHasher.PasswordHasher();
                user.Password = hasher.Hash(dto.Password);
            }

            await _genericUserRepository.Insert(user);
            await _genericUserRepository.Save();
            user = await _userRepository.GetUserByUsername(dto.UserName);

            Person person = new Person
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Birth = Convert.ToDateTime(dto.Birth),
                Address = dto.Address,
                IdUser = user.Id,
                PersonType = type
            };

            if (type == Enums.PersonType.Seller)
            {
                person.Verification = Enums.VerificationStatus.OnHold;
                person.DecisionMade = false;
            }
            else
            {
                person.Verification = Enums.VerificationStatus.None;
                person.DecisionMade = true;
            }

            string mimeType;
            string fileExtension = "";
            byte[] imageBytes;

            if (!String.IsNullOrEmpty(dto.ImageUrl))
            {
                if (dto.ImageUrl.StartsWith("data:image"))
                {
                    var base64Data = dto.ImageUrl.Split(",").Last();
                    mimeType = dto.ImageUrl.Split(",").First().Split(";").First().Split(":").Last();
                    fileExtension = mimeType.Split("/").Last();
                    imageBytes = Convert.FromBase64String(base64Data);
                }
                else
                {
                    imageBytes = Convert.FromBase64String(dto.ImageUrl);
                }

                // Generate a unique file name or use any other logic as needed
                string imageName = $"pic_{person.IdUser}_name_{person.FirstName}.{fileExtension}";

                string imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..", "Common", "Images");

                // Normalize the path
                imagesDirectory = Path.GetFullPath(imagesDirectory);

                if (!Directory.Exists(imagesDirectory))
                {
                    Directory.CreateDirectory(imagesDirectory);
                }

                string imagePath = Path.Combine(imagesDirectory, imageName);

                try
                {
                    await File.WriteAllBytesAsync(imagePath, imageBytes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error writing to file: " + ex.Message);
                }

                person.ImageUrl = imagePath;
            }

            await _genericPersonRepository.Insert(person);
            await _genericPersonRepository.Save();

            return true;
        }
    }
}