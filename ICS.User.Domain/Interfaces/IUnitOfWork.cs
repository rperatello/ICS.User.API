namespace ICS.User.Domain.Interfaces;

public interface IUnitOfWork
{
    IContactRepository ContactRepository { get; }
    IUserRepository UserRepository { get; }
    IPermissionRepository PermissionRepository { get; }
    IUserPermissionRepository UserPermissionRepository { get; }
    Task Commit();
    bool HasDatabaseConnection();
}
