using ICS.User.Domain.Entities;
using ICS.User.Domain.Interfaces;
using ICS.User.Infrastructure.Context;

namespace ICS.User.Infrastructure.Repositories;

public class PermissionRepository : Repository<Permission>, IPermissionRepository
{
    private ICSDbContext _permissionContext;

    public PermissionRepository(ICSDbContext permissionContext) : base(permissionContext)
    {
        _permissionContext = permissionContext;
    }
}
