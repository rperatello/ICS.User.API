namespace ICS.User.Application.DTOs;

public class UserPermissionDTO
{   

    public uint PermissionId { get; set; }
    public string? Permission { get; set; }
    public bool Allowed { get; set; }

    public UserPermissionDTO() {}

}
