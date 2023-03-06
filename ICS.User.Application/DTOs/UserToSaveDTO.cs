using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ICS.User.Application.DTOs;

public class UserToSaveDTO
{
    public int Id { get; set; }

    [MinLength(2)]
    [MaxLength(300)]
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }

    [MinLength(2)]
    [MaxLength(320)]
    [Required(ErrorMessage = "Login is required")]
    public string? Login { get; set; }

    [MinLength(6)]
    [MaxLength(320)]
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [MinLength(6)]
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    public int? Role { get; set; }

    public List<int>? PermissionsIdList { get; set; }
}
