﻿using ICS.User.Domain.Enumerators;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICS.User.Domain.Entities;

public sealed class User : Entity
{   
    public string? Name { get; set; }
    public string? Login { get; set; }
    public string? Email { get; set; }
    public Role Role { get; set; } = Role.USER;
    public string? Password { get; set; }
    public bool isBlocked { get; set; }

    [NotMapped]
    public List<uint>? PermissionsIdList { get; set; }

    public ICollection<UserPermission> UserPermission { get; set; } = null!;


    public User() { }

    public User(uint id, string? name, string? login, string? email, Role role, string? password, bool isBlocked)
    {
        Id = id;
        Name = name;
        Login = login;
        Email = email;
        Role = role;
        Password = password;
        this.isBlocked = isBlocked;
    }

    public User(string? name, string? login, string? email, Role role, string? password, bool isBlocked)
    {
        Name = name;
        Email = email;
        Login = login;
        Role = role;
        Password = password;
        this.isBlocked = isBlocked;
    }
}
