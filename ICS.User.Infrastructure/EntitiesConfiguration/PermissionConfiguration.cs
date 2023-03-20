using ICS.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICS.User.Infrastructure.EntitiesConfiguration;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
        builder.Property(p => p.Name).HasMaxLength(300).IsRequired();
        builder.HasIndex(p => p.Name).IsUnique();
 
        builder.HasData(
            new Permission(1, "read"),
            new Permission(2,"create"),
            new Permission(3, "edit"),
            new Permission(4, "delete")
        );
    }
}