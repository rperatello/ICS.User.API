using ICS.User.Domain.Entities;
using ICS.User.Domain.Interfaces;
using ICS.User.Infrastructure.Context;

namespace ICS.User.Infrastructure.Repositories;

public class ContactRepository : Repository<Contact>, IContactRepository
{
    private ICSDbContext _contactContext;

    public ContactRepository(ICSDbContext contactContext) : base(contactContext)
    {
        _contactContext = contactContext;
    }
    
}
