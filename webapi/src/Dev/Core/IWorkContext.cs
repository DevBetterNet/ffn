using Dev.Core.Domain.Localization;
using Dev.Core.Domain.Users;
using System.Threading.Tasks;

namespace Dev.Core
{
    public interface IWorkContext
    {
        /// <summary>
        /// Gets the current user
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<User> GetCurrentUserAsync();

        /// <summary>
        /// Sets the current user
        /// </summary>
        /// <param name="user">Current user</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task SetCurrentUserAsync(User user = null);

        /// <summary>
        /// Gets the original user (in case the current one is impersonated)
        /// </summary>
        User OriginalUserIfImpersonated { get; }

        /// <summary>
        /// Gets current user working language
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<Language> GetWorkingLanguageAsync();

        /// <summary>
        /// Sets current user working language
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task SetWorkingLanguageAsync(Language language);
    }
}
