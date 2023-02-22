namespace ICS.User.Domain.Entities;

public sealed class Contact : Entity
{
    public string? Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? Birthday { get; set; }

}
