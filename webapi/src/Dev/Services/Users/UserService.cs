using Dev.Core.Domain.Users;
using Dev.Data;
using Dev.Services.Localization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dev.Services.Users;

public class UserService : IUserService
{
    #region Fields
    private readonly IRepository<User> _userRepository;
    private readonly ILocalizationService _localizationService;
    #endregion

    #region Ctor
    public UserService(IRepository<User> userRepository,
                       ILocalizationService localizationService)
    {
        _userRepository = userRepository;
        _localizationService = localizationService;
    }
    #endregion

    public async Task<bool> CreateAsync(User newUser, string password)
    {
        await _userRepository.InsertAsync(newUser);
        return true;
    }

    public async Task<User> FindByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        var query = from c in _userRepository.Table
                    orderby c.Id
                    where c.Email == email
                    select c;
        var customer = await query.FirstOrDefaultAsync();

        return customer;
    }

    public async Task<UserRegistrationResult> RegisterUserAsync(UserRegistrationRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var result = new UserRegistrationResult();
        if (string.IsNullOrEmpty(request.Email))
        {
            result.AddError(await _localizationService.GetResourceAsync("Account.Register.Errors.EmailIsNotProvided"));
            return result;
        }

        return result;
    }
}
