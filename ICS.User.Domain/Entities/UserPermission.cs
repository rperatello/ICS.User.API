namespace ICS.User.Domain.Entities;

public sealed class UserPermission
{
    public int UserId { get; set; }
    public int PermissionId { get; set; }
    public bool Allowed { get; set; }
    public User User { get; set; } = null!;
    public Permission Permission { get; set; } = null!;

    public UserPermission(int userId, int permissionId, bool allowed)
    {
        UserId = userId;
        PermissionId = permissionId;
        Allowed = allowed;
    }
}
