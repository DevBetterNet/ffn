using System.ComponentModel.DataAnnotations;

namespace Dev.Plugin.Sys.Auth.Models;

public class UserRegistrationModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
