using Dev.Core;
using Dev.Core.Domain.Localization;
using Dev.Core.Domain.Users;
using System;
using System.Threading.Tasks;

namespace Dev.Api.Framework
{
    public class WebWorkContext : IWorkContext
    {
        public User OriginalUserIfImpersonated => throw new NotImplementedException();

        public Task<User> GetCurrentUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Language> GetWorkingLanguageAsync()
        {
            throw new NotImplementedException();
        }

        public Task SetCurrentUserAsync(User user = null)
        {
            throw new NotImplementedException();
        }

        public Task SetWorkingLanguageAsync(Language language)
        {
            throw new NotImplementedException();
        }
    }
}
