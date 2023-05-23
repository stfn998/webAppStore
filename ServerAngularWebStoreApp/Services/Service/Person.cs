using AutoMapper;
using Common.DTOs;
using Common.Models;
using DataAcceess.IRepository;
using Microsoft.AspNetCore.Http;
using Services.IService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Service
{
    public class PersonService : IPersonService
    {
        private readonly IGenericRepository<Person> _genericRepositoryPerson;
        private readonly IGenericRepository<User> _genericRepositoryUser;
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public PersonService(IGenericRepository<Person> genericRepositoryPerson, IPersonRepository personRepository, IGenericRepository<User> genericRepositoryUser, IMapper mapper)
        {
            _genericRepositoryPerson = genericRepositoryPerson;
            _personRepository = personRepository;
            _genericRepositoryUser = genericRepositoryUser;
            _mapper = mapper;
        }

        public async Task<PersonDTO> GetPersonByEmail(string email)
        {
            Person person = await _personRepository.GetPersonByEmail(email);
            if (person == null)
            {
                throw new KeyNotFoundException("User does not exists.");
            }
            User user = await _genericRepositoryUser.GetByObject(person.IdUser);
            if (person == null)
            {
                throw new KeyNotFoundException("User does not exists.");
            }

            PersonDTO dto = _mapper.Map<PersonDTO>(person);
            dto.UserName = user.UserName;

            return dto;
        }

        public async Task<PersonDTO> GetPersonById(int id)
        {
            Person person = await _genericRepositoryPerson.GetByObject(id);
            if (person == null)
            {
                throw new KeyNotFoundException("User does not exists.");
            }
            User user = await _genericRepositoryUser.GetByObject(person.IdUser);
            if (person == null)
            {
                throw new KeyNotFoundException("User does not exists.");
            }

            PersonDTO dto = _mapper.Map<PersonDTO>(person);
            dto.UserName = user.UserName;

            return dto;
        }

        public async Task<bool> UpdatePerson(ProfileDTO dto)
        {
            bool isChanged = false;
            Person person = await _genericRepositoryPerson.GetByObject(dto.IdPerson);
            if (person == null)
            {
                throw new KeyNotFoundException("User does not exist.");
            }

            if (dto.UserName != null)
            {
                User user = await _genericRepositoryUser.GetByObject(person.IdUser);
                if (user == null)
                {
                    throw new KeyNotFoundException("User does not exist.");
                }
                user.UserName = dto.UserName;
                await _genericRepositoryUser.Update(user);
                await _genericRepositoryUser.Save();
            }
            if (dto.Address != person.Address)
            {
                person.Address = dto.Address;
                isChanged = true;
            }
            if (dto.Email != person.Email)
            {
                person.Email = dto.Email;
                isChanged = true;
            }
            if (dto.FirstName != person.FirstName)
            {
                person.FirstName = dto.FirstName;
                isChanged = true;
            }
            if (dto.LastName != person.LastName)
            {
                person.FirstName = dto.LastName;
                isChanged = true;
            }

            if (isChanged)
            {
                await _genericRepositoryPerson.Update(person);
                await _genericRepositoryPerson.Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<PersonDTO>> GetSeller()
        {
            IEnumerable<Person> persons = await _personRepository.GetPersonByType(Enums.PersonType.Seller);
            if (persons == null)
            {
                throw new KeyNotFoundException("User does not exists.");
            }
            var dto = await PersonToPersonDTO(persons);

            return dto;
        }

        public async Task<IEnumerable<PersonDTO>> GetAll()
        {
            IEnumerable<Person> persons = await _genericRepositoryPerson.GetAll();
            if (persons == null)
            {
                throw new KeyNotFoundException("User does not exists.");
            }
            var dto = await PersonToPersonDTO(persons);

            return dto;
        }

        private async Task<IEnumerable<PersonDTO>> PersonToPersonDTO(IEnumerable<Person> persons)
        {
            List<PersonDTO> listDto = new List<PersonDTO>();

            foreach (var p in persons)
            {
                User u = await _genericRepositoryUser.GetByObject(p.IdUser);
                if (u == null)
                {
                    throw new KeyNotFoundException("User does not exists.");
                }

                PersonDTO dto = _mapper.Map<PersonDTO>(p);
                dto.UserName = u.UserName;
                listDto.Add(dto);
            }

            return listDto;
        }

        public async Task<bool> DeletePerson(int idPerson)
        {
            Person person = await _genericRepositoryPerson.GetByObject(idPerson);
            if (person == null)
            {
                throw new KeyNotFoundException("User does not exist.");
            }

            await _genericRepositoryPerson.Delete(person);
            await _genericRepositoryPerson.Save();

            return true;
        }

        public async Task UploadImage(int idPerson, IFormFile file)
        {
            if (file != null && idPerson != null)
            {
                Person person = await _genericRepositoryPerson.GetByObject(idPerson);

                if (person == null)
                {
                    throw new KeyNotFoundException("User does not exist.");
                }

                person.ImageUrl = await SaveImage(file);

                await _genericRepositoryPerson.Update(person);
                await _genericRepositoryPerson.Save();
            }
            else
            {
                throw new Exception("Invalid parameters!");
            }
        }

        private async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\Common\Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imagePath;
        }

        public async Task<string> GetImage(int idPerson)
        {
            Person person = await _genericRepositoryPerson.GetByObject(idPerson);

            if (person == null)
            {
                throw new KeyNotFoundException("User does not exist.");
            }
            return person.ImageUrl;
        }
    }
}