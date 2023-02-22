namespace ICS.User.Domain.Interfaces;

public interface IUserRepository : IRepository<Entities.User>
{
    Task<IEnumerable<Entities.User>> GetAllUsersWithPermissions();
}
