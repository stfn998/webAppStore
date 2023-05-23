using AutoMapper;
using Common.DTOs;
using Common.Models;

namespace Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDTO>().ReverseMap();
            CreateMap<User, AuthenticateRequestDTO>().ReverseMap();
        }
    }
}