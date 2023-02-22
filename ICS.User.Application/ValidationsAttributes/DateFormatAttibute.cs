using System.ComponentModel.DataAnnotations;

namespace ICS.User.Application.ValidationsAttributes;

public class DateFormatAttibute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;
        string[] parts = ((string)value).Split("-");
        bool part1Has4Elements = parts.Length == 3 && parts[0].Length == 4;
        bool otherPartsHas2Elements = parts.Length == 3 && parts[1].Length == 2 && parts[2].Length == 2;
        return (part1Has4Elements && otherPartsHas2Elements) 
            ? ValidationResult.Success 
            : new ValidationResult("Date has format yyyy-MM-dd (Ex.: 2023-02-22)");
    }
}
