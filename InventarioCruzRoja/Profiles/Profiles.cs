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
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Sede, SedeDto>().ReverseMap();
            CreateMap<Estado, EstadoDto>().ReverseMap();
            CreateMap<Producto, ProductoDto>()
                .ForMember(dest =>
                    dest.Estado, opt => opt.MapFrom(x => x.Estado.Nombre))
                .ForMember(dest =>
                    dest.Fabricante, opt => opt.MapFrom(x => x.Fabricante.Nombre))
                .ForMember(dest =>
                    dest.Sede, opt => opt.MapFrom(x => x.Sede.Nombre))
                .ForMember(dest =>
                    dest.Categoria, opt => opt.MapFrom(x => x.Categoria.Nombre));
            CreateMap<ProductoDto, Producto>()
                .ForMember(dest =>
                    dest.Estado, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.Fabricante, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.Sede, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.Categoria, opt => opt.Ignore());
        }
    }
}
