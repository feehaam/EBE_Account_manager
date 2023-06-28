using AutoMapper;
using EcommerceBackend.DTO;
using EcommerceBackend.Models;

namespace EcommerceBackend.Helpers
{
    public class AutoMapper : Profile
    {
        public AutoMapper() 
        {
            CreateMap<User, CreateUserDto>();
        }
    }
}
