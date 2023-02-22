using ICS.User.Application.ValidationsAttributes;
using System.ComponentModel.DataAnnotations;

namespace ICS.User.Application.DTOs;

public class ContactDTO
{
    public int Id { get; set; }

    [MinLength(2)]
    [MaxLength(300)]
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }

    [MinLength(6)]
    [MaxLength(320)]
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [MinLength(6, ErrorMessage = "Email is required")]
    [MaxLength(10)]
    [DateFormatAttibute]
    public string? Birthday { get; set; }


}
