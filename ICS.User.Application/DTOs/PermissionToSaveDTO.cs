using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICS.User.Application.DTOs;

public class PermissionToSaveDTO
{
    public int? Id { get; set; }

    [MaxLength(320)]
    [Required(ErrorMessage = "Permision name is required")]
    public string Name { get; set; } = null!;
}
