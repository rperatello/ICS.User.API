using ICS.User.Domain.Enumerators;

namespace ICS.User.Domain.Entities;

public sealed class User : Entity
{   
    public string? Name { get; set; }
    public string? Login { get; set; }
    public string? Email { get; set; }
    public Role Role { get; set; }
    public string? Password { get; set; }
    public bool isBlocked { get; set; }
    public ICollection<UserPermission> UserPermission { get; set; } = null!;

    public User(int id, string? name, string? login, string? email, Role role, string? password, bool isBlocked)
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
