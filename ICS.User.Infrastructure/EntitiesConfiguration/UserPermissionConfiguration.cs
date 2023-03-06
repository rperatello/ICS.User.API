using ICS.User.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICS.User.Infrastructure.EntitiesConfiguration;

public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.HasKey(up => new { up.UserId, up.PermissionId });
        builder.Property(up => up.UserId).IsRequired();
        builder.Property(up => up.PermissionId).IsRequired();
        builder.Property(up => up.Allowed).HasDefaultValue(false);

        builder.HasOne(up => up.User).WithMany(up => up.UserPermission).HasForeignKey(up => up.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(up => up.Permission).WithMany(up => up.UserPermission).HasForeignKey(up => up.PermissionId).OnDelete(DeleteBehavior.Cascade);
        
        builder.HasData(
            new UserPermission(1, 1, true),
            new UserPermission(1, 2, true),
            new UserPermission(1, 3, true),
            new UserPermission(1, 4, true)
        );
    }
}

