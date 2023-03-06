using ICS.User.Domain.Interfaces;
using ICS.User.Infrastructure.Context;

namespace ICS.User.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public ICSDbContext _context;
    private ContactRepository _contactRepository;
    private UserRepository _userRepository;
    private PermissionRepository _permissionRepository;
    private UserPermissionRepository _userPermissionRepository;

    public UnitOfWork(ICSDbContext contexto)
    {
        _context = contexto;
    }

    public IContactRepository ContactRepository 
    { 
        get
        {
            return _contactRepository = _contactRepository ?? new ContactRepository(_context);
        }
    }

    public IUserRepository UserRepository
    {
        get
        {
            return _userRepository = _userRepository ?? new UserRepository(_context);
        }
    }

    public IPermissionRepository PermissionRepository
    {
        get
        {
            return _permissionRepository = _permissionRepository ?? new PermissionRepository(_context);
        }
    }

    public IUserPermissionRepository UserPermissionRepository
    {
        get
        {
            return _userPermissionRepository = _userPermissionRepository ?? new UserPermissionRepository(_context);
        }
    }

    public async Task Commit()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            string error = ex.InnerException == null ? $"Message: {ex.Message}" : $"Message: {ex.Message} | InnerException: {ex.InnerException.Message.Split("\r\n")[0]}";
            throw new Exception(error);
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public bool HasDatabaseConnection()
    {
        return _context.Database.CanConnect();
    }

}
