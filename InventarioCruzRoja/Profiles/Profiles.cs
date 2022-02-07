using AutoMapper;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Models;

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
            CreateMap<Proveedor, ProveedorDto>().ReverseMap();
            CreateMap<Contacto, ContactoDto>().ReverseMap();
            CreateMap<EventoProducto, EventoDto>().ReverseMap();
            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<Producto, ProductoDto>()
                .ForMember(dest =>
                    dest.Estado, opt => opt.MapFrom(x => x.Estado.Nombre))
                .ForMember(dest =>
                    dest.Fabricante, opt => opt.MapFrom(x => x.Fabricante.Nombre))
                .ForMember(dest =>
                    dest.Categoria, opt => opt.MapFrom(x => x.Categoria.Nombre));
            CreateMap<ProductoDto, Producto>()
                .ForMember(dest =>
                    dest.Estado, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.Fabricante, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.Categoria, opt => opt.Ignore());
            CreateMap<Ingreso, IngresoDto>()
                .ForMember(dest =>
                    dest.Proveedor, opt => opt.MapFrom(x => x.Proveedor.Nombre))
                .ForMember(dest =>
                    dest.Producto, opt => opt.MapFrom(x => x.Producto.Nombre))
                .ForMember(dest =>
                    dest.Sede, opt => opt.MapFrom(x => x.Sede.Nombre))
                .ForMember(dest =>
                    dest.Usuario, opt => opt.MapFrom(x => x.User.Username))
                .ForMember(dest =>
                    dest.UsuarioAnula, opt => opt.MapFrom(x => x.UserAnula.Username));
            CreateMap<IngresoDto, Ingreso>()
                .ForMember(dest =>
                    dest.Proveedor, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.Sede, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.Producto, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.User, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.UserAnula, opt => opt.Ignore());
            CreateMap<Egreso, EgresoDto>()
                .ForMember(dest =>
                    dest.Producto, opt => opt.MapFrom(x => x.Producto.Nombre))
                .ForMember(dest =>
                    dest.Sede, opt => opt.MapFrom(x => x.Sede.Nombre))
                .ForMember(dest =>
                    dest.Usuario, opt => opt.MapFrom(x => x.User.Username))
                .ForMember(dest =>
                    dest.UsuarioAnula, opt => opt.MapFrom(x => x.UserAnula.Username));
            CreateMap<EgresoDto, Egreso>()
                .ForMember(dest =>
                    dest.Sede, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.Producto, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.User, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.UserAnula, opt => opt.Ignore());
            CreateMap<Traslado, TrasladoDto>()
                .ForMember(dest =>
                    dest.Producto, opt => opt.MapFrom(x => x.Producto.Nombre))
                .ForMember(dest =>
                    dest.SedeOrigen, opt => opt.MapFrom(x => x.EgresoOrigen.Sede.Nombre))
                .ForMember(dest =>
                    dest.SedeDestino, opt => opt.MapFrom(x => x.IngresoDestino.Sede.Nombre))
                .ForMember(dest =>
                    dest.Usuario, opt => opt.MapFrom(x => x.User.Username))
                .ForMember(dest =>
                    dest.UsuarioAnula, opt => opt.MapFrom(x => x.UserAnula.Username));
            CreateMap<TrasladoDto, Traslado>()
                .ForMember(dest =>
                    dest.IngresoDestino, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.EgresoOrigen, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.Producto, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.User, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.UserAnula, opt => opt.Ignore());
        }
    }
}
