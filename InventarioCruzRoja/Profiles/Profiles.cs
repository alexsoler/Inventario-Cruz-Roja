using AutoMapper;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Models;
using System.Linq;

namespace InventarioCruzRoja.Profiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserLoginDto, User>();
            CreateMap<User, LoginResponseDto>()
                .ForMember(dest =>
                dest.Roles, opt => opt.MapFrom(x =>
                    x.UserRoles.Select(role => role.Role.Name)));
        }
    }
}
