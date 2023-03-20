using System.ComponentModel;

namespace ICS.User.Domain.Enumerators;

public enum Role
{
    [Description("administrator")]
    ADMIN = 0,
    [Description("user")]
    USER = 1
}
