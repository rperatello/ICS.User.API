using ICS.User.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ICS.User.Application.DTOs;

public class UserDTO
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    [MinLength(2)]
    [MaxLength(300)]
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }

    [DataMember(Order = 3)]
    [MinLength(2)]
    [MaxLength(320)]
    [Required(ErrorMessage = "Login is required")]
    public string? Login { get; set; }

    [DataMember(Order = 4)]
    [MinLength(6)]
    [MaxLength(320)]
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [DataMember(Order = 5)]
    public string? Role { get; set; }

    [DataMember(Order = 6)]
    //public List<int>? PermissionsIdList { get; set; }

    public ICollection<UserPermissionDTO> Permissions { get; set; }

}
