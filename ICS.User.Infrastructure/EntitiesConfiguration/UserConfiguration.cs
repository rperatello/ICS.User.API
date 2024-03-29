﻿using ICS.User.Domain.Enumerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICS.User.Infrastructure.EntitiesConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<Domain.Entities.User>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
        builder.Property(u => u.Name).HasMaxLength(300).IsRequired();
        builder.Property(u => u.Login).HasMaxLength(100).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(320).IsRequired();
        builder.HasIndex(p => p.Login).IsUnique();

        builder.HasData(
            new Domain.Entities.User(1, "Admin", "admin", "admin@ics.com", Role.ADMIN, "21232f297a57a5a743894a0e4a801fc3", false)
        );
    }


}
