using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ICS.User.Application.DTOs;

public class PermissionToSaveDTO
{
    [Required]
    [Range(0, uint.MaxValue)]
    public uint Id { get; set; }

    private string? _name { get; set; }

    [MaxLength(320)]
    [Required(ErrorMessage = "Permision name is required")]
    public string? Name
    {
        get { return _name; }
        set
        {
            _name = value?.ToLower().Trim();
        }
    }
}
