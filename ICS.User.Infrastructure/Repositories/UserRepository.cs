using ICS.User.Domain.Interfaces;
using ICS.User.Infrastructure.Context;

namespace ICS.User.Infrastructure.Repositories;

public class UserRepository : Repository<Domain.Entities.User>, IUserRepository
{
    private ICSDbContext _userContext;

    public UserRepository(ICSDbContext userContext) : base(userContext)
    {
        _userContext = userContext;
    }
}
