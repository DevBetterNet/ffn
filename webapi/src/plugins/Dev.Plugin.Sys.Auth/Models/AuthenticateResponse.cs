using Dev.Core.Domain.Users;

namespace Dev.Plugin.Sys.Auth.Models;

public class AuthenticateResponse
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }


    public AuthenticateResponse(User user, string token)
    {
        Email = user.Email;
        Username = user.UserName;
        Token = token;
    }
}
