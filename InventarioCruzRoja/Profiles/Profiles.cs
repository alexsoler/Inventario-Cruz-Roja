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
            CreateMap<UserEditDto, User>();
            CreateMap<UserLoginDto, User>();
            CreateMap<User, LoginResponseDto>()
                .ForMember(dest =>
                dest.Roles, opt => opt.MapFrom(x =>
                    x.UserRoles.Select(role => role.Role.Name)));
            CreateMap<User, UserTableDto>()
                .ForMember(dest =>
                dest.Roles, opt => opt.MapFrom(x =>
                    x.UserRoles.Select(role => role.Role.Name)));

            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Fabricante, FabricanteDto>().ReverseMap();
        }
    }
}
