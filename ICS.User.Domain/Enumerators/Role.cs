using System.ComponentModel;

namespace ICS.User.Domain.Enumerators;

public enum Role
{
    [Description("Administrator")]
    Admin,
    [Description("User")]
    User
}
