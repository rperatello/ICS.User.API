using ICS.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.User.Infrastructure.Context;

public class ICSDbContext : DbContext
{
	public ICSDbContext(DbContextOptions<ICSDbContext> options) : base(options) {	}

	public DbSet<Contact> Contacts { get; set; }
	public DbSet<Domain.Entities.User> Users { get; set; }
	public DbSet<Permission> Permissions { get; set; }
	public DbSet<UserPermission> UserPermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ICSDbContext).Assembly);
    }
}
