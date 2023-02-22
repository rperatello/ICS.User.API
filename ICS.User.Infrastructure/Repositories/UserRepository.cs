using ICS.User.Domain.Interfaces;
using ICS.User.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace ICS.User.Infrastructure.Repositories;

public class UserRepository : Repository<Domain.Entities.User>, IUserRepository
{
    private ICSDbContext _userContext;

    public UserRepository(ICSDbContext userContext) : base(userContext)
    {
        _userContext = userContext;
    }

    public async Task<IEnumerable<Domain.Entities.User>> GetAllUsersWithPermissions()
    {
        return await GetAll().Include(up => up.UserPermission).ToListAsync();
    }
}
