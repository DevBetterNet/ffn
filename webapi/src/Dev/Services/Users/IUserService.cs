using Dev.Core.Domain.Users;
using System.Threading.Tasks;

namespace Dev.Services.Users
{
    public interface IUserService
    {
        Task<User> FindByEmailAsync(string email);
        Task<bool> CreateAsync(User newUser, string password);
        Task<UserRegistrationResult> RegisterUserAsync(UserRegistrationRequest userRegistration);
    }
}
