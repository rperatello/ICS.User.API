using ICS.User.Domain.Entities;
using ICS.User.Domain.Interfaces;
using ICS.User.Infrastructure.Context;

namespace ICS.User.Infrastructure.Repositories;

public class UserPermissionRepository : Repository<UserPermission>, IUserPermissionRepository
{
    private ICSDbContext _userPermissionContext;

    public UserPermissionRepository(ICSDbContext userPermissionContext) : base(userPermissionContext)
    {
        _userPermissionContext = userPermissionContext;
    }
}
