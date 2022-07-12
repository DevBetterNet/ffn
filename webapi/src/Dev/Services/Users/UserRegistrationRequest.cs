using Dev.Core.Domain.Users;
using System;

namespace Dev.Services.Users;

public class UserRegistrationRequest
{
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Password format
    /// </summary>
    public PasswordFormat PasswordFormat { get; set; }

    /// <summary>
    /// Store identifier
    /// </summary>
    public Guid WebAppId { get; set; }

    /// <summary>
    /// Is approved
    /// </summary>
    public bool IsApproved { get; set; }
}
