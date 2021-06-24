using Dev.Core.Domain.Users;
using Dev.Services.Authentication;
using System;
using System.Threading.Tasks;

namespace Dev.Plugin.Sys.Auth.Services
{
    public class JWTAuthenticationService : IAuthenticationService
    {
        public Task<User> GetAuthenticatedUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task SignInAsync(User user, bool isPersistent)
        {
            throw new NotImplementedException();
        }

        public Task SignOutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
