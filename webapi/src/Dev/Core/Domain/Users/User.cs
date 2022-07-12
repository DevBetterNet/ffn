using System;

namespace Dev.Core.Domain.Users;

public class User : BaseEntity
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public int FailedLoginAttempts { get; set; }
    public bool Active { get; set; }
    public bool Deleted { get; set; }
    public string LastIpAddress { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? LastLoginDateUtc { get; set; }
    public DateTime LastActivityDateUtc { get; set; }
}
