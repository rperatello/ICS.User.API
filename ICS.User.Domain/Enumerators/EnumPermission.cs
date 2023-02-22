﻿using System.ComponentModel;

namespace ICS.User.Domain.Enumerators;

public enum EnumPermission
{
    [Description("FullAccess")]
    FullAccess,
    [Description("Create")]
    Create,
    [Description("Delete")]
    Delete,
    [Description("Edit")]
    Edit,
    [Description("Read")]
    Read
}
