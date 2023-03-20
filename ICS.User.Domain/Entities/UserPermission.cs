namespace ICS.User.Domain.Entities;

public sealed class UserPermission
{
    public uint UserId { get; set; }
    public uint PermissionId { get; set; }
    public bool Allowed { get; set; }
    public User User { get; set; } = null!;
    public Permission Permission { get; set; } = null!;

    public UserPermission() { }

    public UserPermission(uint userId, uint permissionId, bool allowed)
    {
        UserId = userId;
        PermissionId = permissionId;
        Allowed = allowed;
    }
}
