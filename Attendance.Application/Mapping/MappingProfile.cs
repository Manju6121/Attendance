using AttendanceTracker.Application.DTOs;
using AttendanceTracker.Application.DTOs.Role;
using AttendenceTracker.Domain.Entity;
using AutoMapper;
using AttendanceTracker.Application.DTOs;
namespace AttendanceTracker.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<UserDetails, UserDetailsDTO>().ReverseMap();

            CreateMap<User, UserResponseDTO>()
                .ForMember(dest => dest.RoleName,
                    opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : ""))
                .ForMember(dest => dest.UserDetails,
                    opt => opt.MapFrom(src => src.UserDetails));

            CreateMap<User, LoginResponseDTO>().ReverseMap();
        }
    }
}