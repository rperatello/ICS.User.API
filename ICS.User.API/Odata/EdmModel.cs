using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ICS.User.API.Odata;

public static class EdmModel
{    

    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

        builder.Namespace = "ICS.User.API";
        builder.ContainerName = "DefaultContainer";
        builder.EnableLowerCamelCase();

        foreach (Type item in GetTypesInNamespace(System.Reflection.Assembly.Load("ICS.User.Application"), "ICS.User.Application.DTOs"))
        {
            if (item.GetProperty("Id") == null)
                continue;

            EntityTypeConfiguration entityType = builder.AddEntityType(item);
            entityType.HasKey(item.GetProperty("Id"));
            builder.AddEntitySet(item.Name, entityType);
        }

        return builder.GetEdmModel();
    }

    private static Type[] GetTypesInNamespace(System.Reflection.Assembly assembly, string nameSpace)
    {
        var check = assembly.GetTypes();
        return assembly.GetTypes()
            .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
            .ToArray();
    }
}
