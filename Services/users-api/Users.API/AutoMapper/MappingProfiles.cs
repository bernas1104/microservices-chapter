using AutoMapper;
using Users.API.Dtos.InputModels;
using Users.API.Dtos.ViewModels;
using Users.Domain.Entities;

namespace Users.API.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserInputModel, User>();
            CreateMap<User, UserViewModel>();
        }
    }
}
