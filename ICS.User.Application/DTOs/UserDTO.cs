using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICS.User.Application.DTOs;

public class UserDTO
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
}
