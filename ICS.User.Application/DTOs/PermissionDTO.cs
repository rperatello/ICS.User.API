using System.ComponentModel.DataAnnotations;

namespace ICS.User.Application.DTOs;

public class PermissionDTO
{
    [Required]
    [Range(1, int.MaxValue)]
    public int Id { get; set; }

    [MaxLength(320)]
    [Required(ErrorMessage = "Permision name is required")]
    public string? Name { get; set; }
}
