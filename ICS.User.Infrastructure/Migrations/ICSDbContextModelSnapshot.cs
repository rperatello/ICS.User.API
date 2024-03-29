﻿// <auto-generated />
using ICS.User.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ICS.User.Infrastructure.Migrations
{
    [DbContext(typeof(ICSDbContext))]
    partial class ICSDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ICS.User.Domain.Entities.Contact", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Birthday")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("character varying(320)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("ICS.User.Domain.Entities.Permission", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Permissions");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "read"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "create"
                        },
                        new
                        {
                            Id = 3L,
                            Name = "edit"
                        },
                        new
                        {
                            Id = 4L,
                            Name = "delete"
                        });
                });

            modelBuilder.Entity("ICS.User.Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("character varying(320)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<bool>("isBlocked")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Email = "admin@ics.com",
                            Login = "admin",
                            Name = "Admin",
                            Password = "21232f297a57a5a743894a0e4a801fc3",
                            Role = 0,
                            isBlocked = false
                        });
                });

            modelBuilder.Entity("ICS.User.Domain.Entities.UserPermission", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("PermissionId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Allowed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.HasKey("UserId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("UserPermissions");

                    b.HasData(
                        new
                        {
                            UserId = 1L,
                            PermissionId = 1L,
                            Allowed = true
                        },
                        new
                        {
                            UserId = 1L,
                            PermissionId = 2L,
                            Allowed = true
                        },
                        new
                        {
                            UserId = 1L,
                            PermissionId = 3L,
                            Allowed = true
                        },
                        new
                        {
                            UserId = 1L,
                            PermissionId = 4L,
                            Allowed = true
                        });
                });

            modelBuilder.Entity("ICS.User.Domain.Entities.UserPermission", b =>
                {
                    b.HasOne("ICS.User.Domain.Entities.Permission", "Permission")
                        .WithMany("UserPermission")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ICS.User.Domain.Entities.User", "User")
                        .WithMany("UserPermission")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ICS.User.Domain.Entities.Permission", b =>
                {
                    b.Navigation("UserPermission");
                });

            modelBuilder.Entity("ICS.User.Domain.Entities.User", b =>
                {
                    b.Navigation("UserPermission");
                });
#pragma warning restore 612, 618
        }
    }
}
