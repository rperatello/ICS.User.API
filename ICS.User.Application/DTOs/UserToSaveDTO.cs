using System.ComponentModel.DataAnnotations;

namespace ICS.User.Application.DTOs;

public class UserToSaveDTO
{
    [Required]
    [Range(0, uint.MaxValue)]
    public uint Id { get; set; }

    [MinLength(2, ErrorMessage = "Name must contain at least 2 characters")]
    [MaxLength(300)]
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }

    private string? _login { get; set; }

    [MinLength(2, ErrorMessage = "Login must contain at least 2 characters")]
    [MaxLength(320)]
    [Required(ErrorMessage = "Login is required")]
    public string? Login
    {
        get { return _login; }
        set
        {
            _login = value?.ToLower().Trim();
        }
    }

    private string? _email { get; set; }

    [MinLength(6)]
    [MaxLength(320)]
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string? Email
    {
        get { return _email; }
        set
        {
            _email = value?.ToLower().Trim();
        }
    }

    public string? Password { get; set; }

    public int? Role { get; set; } = 1;

    public List<uint>? PermissionsIdList { get; set; }
}
