using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System;

public static class EnumExtensions
{
    public static string GetEnumDescription<T>(this T enumSrc) where T : Enum
    {
        FieldInfo? fieldInfo = enumSrc.GetType().GetField(enumSrc.ToString());
        if (fieldInfo == null) return enumSrc.ToString();
        DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attributes != null && attributes?.Length > 0)
            return attributes[0].Description;
        else
            return enumSrc.ToString();
    }

    public static IEnumerable<string> GetAllValuesOfEnumType<T>(this T enumSrc) where T : Enum
    {
        IList<string> enumValuesList = new List<string>();
        foreach (string value in Enum.GetNames(typeof(T)))
        {
            enumValuesList.Add(value) ;
        }
        return enumValuesList;
    }


    public static IEnumerable<string> GetAllDescriptionOfEnumType<T>(this T enumSrc) where T : Enum
    {
        IList<string> enumDescriptionList = new List<string>();
        foreach (string value in Enum.GetNames(typeof(T)))
        {
            enumDescriptionList.Add( ((T)Enum.Parse(typeof(T), value)).GetEnumDescription() );
        }
        return enumDescriptionList;
    }

}
