using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Core.Domain.Users;

public class UserPassword : BaseEntity
{
    public UserPassword()
    {
    }
    public Guid UserId { get; set; }
    public string Password { get; set; }
    public int PasswordFormatId { get; set; }
    public string PasswordSalt { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public PasswordFormat PasswordFormat
    {
        get => (PasswordFormat)PasswordFormatId;
        set => PasswordFormatId = (int)value;
    }
}
