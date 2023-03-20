using System.ComponentModel.DataAnnotations;

namespace ICS.User.Application.DTOs;

public class UserLoginDTO
{    
    private string _login { get; set; } = null!;

    [MinLength(2, ErrorMessage = "Login must contain at least 2 characters")]
    [MaxLength(320)]
    [Required(ErrorMessage = "Login is required")]
    public string Login
    {
        get { return _login; }
        set { _login = value?.ToLower()?.Trim(); }
    }

    private string _password { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    public string Password
    {
        get { return _password; }
        set { _password = value?.Trim(); }
    }

    private string _ip { get; set; } = "127.0.0.1";
    public string IP
    {
        get { return _ip; }
        set { _ip = value?.Trim(); }
    }
}
