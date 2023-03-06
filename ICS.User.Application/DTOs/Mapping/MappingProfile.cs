using AutoMapper;
using ICS.User.Domain.Entities;

namespace ICS.User.Application.DTOs.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Contact
        CreateMap<Contact, ContactDTO>().ReverseMap();
        #endregion

        #region User
        CreateMap<Domain.Entities.User, UserDTO>()
            .ForMember(s => s.Role, opt => opt.MapFrom(f => f.Role.ToString()))
            .ForMember(s => s.Permissions, opt => opt.MapFrom(f => f.UserPermission))
            .ReverseMap();

        CreateMap<Domain.Entities.User, UserToSaveDTO>()
            .ReverseMap();
        #endregion

        #region UserPermission
        CreateMap<UserPermission, UserPermissionDTO>()
            .ForMember(s => s.Permission, opt => opt.MapFrom(f => f.Permission.Name))
            .ReverseMap();
        #endregion

        #region Permission
        CreateMap<Permission, PermissionDTO>().ReverseMap();
        CreateMap<PermissionToSaveDTO, Permission>();
        #endregion

    }
}
