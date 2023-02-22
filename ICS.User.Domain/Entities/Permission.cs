using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICS.User.Domain.Entities;

public sealed class Permission : Entity
{
    public string? Name { get; set; }
    public ICollection<UserPermission> UserPermission { get; set; } = null!;

    public Permission(string? name)
    {
        Name = name;
    }

    public Permission(int id, string? name)
    {
        Id = id;
        Name = name;
    }
}
