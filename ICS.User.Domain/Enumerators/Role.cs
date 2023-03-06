using System.ComponentModel;

namespace ICS.User.Domain.Enumerators;

public enum Role
{
    [Description("Administrator")]
    ADMIN,
    [Description("User")]
    USER
}
